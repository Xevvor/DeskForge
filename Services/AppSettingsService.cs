using System.IO;
using System.Text.Json;

namespace DeskForge.Services;

public class AppSettings
{
    public string? RawgApiKey { get; set; }
}

/// <summary>
/// Reads local app settings (currently just the RAWG API key) from
/// %AppData%\DeskForge\settings.json, so secrets never need to live in source or the UI.
/// </summary>
public static class AppSettingsService
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DeskForge", "settings.json");

    private static AppSettings? _cached;

    public static AppSettings Load()
    {
        if (_cached is not null) return _cached;

        try
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);
                _cached = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                return _cached;
            }
        }
        catch (IOException) { }
        catch (JsonException) { }

        _cached = new AppSettings();
        return _cached;
    }

    /// <summary>Environment variable takes priority over the settings file, so CI/dev machines can override it.</summary>
    public static string? GetRawgApiKey()
    {
        var envKey = Environment.GetEnvironmentVariable("DESKFORGE_RAWG_API_KEY");
        return !string.IsNullOrWhiteSpace(envKey) ? envKey : Load().RawgApiKey;
    }
}
