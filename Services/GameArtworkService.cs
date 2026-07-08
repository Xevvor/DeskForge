using System.IO;
using System.Net.Http;
using System.Text.Json;
using DeskForge.Models;

namespace DeskForge.Services;

/// <summary>
/// Resolves real cover art for catalog/launch-list games: Steam's public CDN when a
/// SteamAppId is known, otherwise a RAWG.io search by name. Results are cached to disk
/// (including "no image found") so the app doesn't hit the network again on every launch.
/// Never scrapes third-party sites — only Steam's official CDN and the RAWG API.
/// </summary>
public static class GameArtworkService
{
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(6) };

    private static readonly string CachePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DeskForge", "artwork_cache.json");

    private static readonly HashSet<string> InFlight = new();
    private static Dictionary<string, string?>? _cache;

    public static async Task ResolveAsync(GameCatalogEntry entry)
    {
        if (entry.SteamAppId is int appId)
        {
            entry.HeaderImageUrl = $"https://cdn.cloudflare.steamstatic.com/steam/apps/{appId}/header.jpg";
            entry.CoverImageUrl = $"https://cdn.cloudflare.steamstatic.com/steam/apps/{appId}/library_600x900.jpg";
            return;
        }

        var cache = LoadCache();
        if (cache.TryGetValue(entry.Name, out var cached))
        {
            entry.CoverImageUrl = cached;
            return;
        }

        lock (InFlight)
        {
            if (!InFlight.Add(entry.Name)) return; // Already being resolved elsewhere; avoid duplicate calls.
        }

        var apiKey = AppSettingsService.GetRawgApiKey();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            // No key configured: cache "no image" so we don't keep re-checking, and fall back to the placeholder tile.
            cache[entry.Name] = null;
            SaveCache(cache);
            return;
        }

        try
        {
            var url = $"https://api.rawg.io/api/games?search={Uri.EscapeDataString(entry.Name)}&page_size=1&key={apiKey}";
            var json = await Http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);

            string? image = null;
            if (doc.RootElement.TryGetProperty("results", out var results) && results.GetArrayLength() > 0)
            {
                var first = results[0];
                image = first.TryGetProperty("background_image", out var img) ? img.GetString() : null;
            }

            entry.CoverImageUrl = image;
            cache[entry.Name] = image;
        }
        catch (HttpRequestException)
        {
            // Network unavailable/blocked: leave CoverImageUrl unset, UI falls back to the placeholder tile.
        }
        catch (TaskCanceledException)
        {
            // Request timed out.
        }
        catch (JsonException)
        {
            // Unexpected response shape.
        }

        SaveCache(cache);
    }

    private static Dictionary<string, string?> LoadCache()
    {
        if (_cache is not null) return _cache;

        try
        {
            if (File.Exists(CachePath))
            {
                var json = File.ReadAllText(CachePath);
                _cache = JsonSerializer.Deserialize<Dictionary<string, string?>>(json) ?? new();
                return _cache;
            }
        }
        catch (IOException) { }
        catch (JsonException) { }

        _cache = new Dictionary<string, string?>();
        return _cache;
    }

    private static void SaveCache(Dictionary<string, string?> cache)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CachePath)!);
            File.WriteAllText(CachePath, JsonSerializer.Serialize(cache));
        }
        catch (IOException)
        {
            // Non-fatal: cache just won't persist for this run.
        }
    }
}
