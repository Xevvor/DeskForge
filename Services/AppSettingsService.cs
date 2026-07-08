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
/// If the file is missing (fresh install, or the folder got wiped), it's recreated with the
/// default key on first run so artwork lookups keep working without manual intervention.
/// </summary>
public static class AppSettingsService
{
    private const string DefaultRawgApiKey = "ab30d06afa05445b87eb9ddc629bb833";

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

        _cached = new AppSettings { RawgApiKey = DefaultRawgApiKey };
        TrySave(_cached);
        return _cached;
    }

    private static void TrySave(AppSettings settings)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(settings));
        }
        catch (IOException) { }
    }

    /// <summary>Environment variable takes priority over the settings file, so CI/dev machines can override it.</summary>
    public static string? GetRawgApiKey()
    {
        var envKey = Environment.GetEnvironmentVariable("DESKFORGE_RAWG_API_KEY");
        return !string.IsNullOrWhiteSpace(envKey) ? envKey : Load().RawgApiKey;
    }
}
