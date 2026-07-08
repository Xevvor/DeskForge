using System.IO;
using System.Text.Json;
using DeskForge.Models;

namespace DeskForge.Services;

/// <summary>Persists the "Your Games" list to disk so it survives app restarts.</summary>
public static class GameLibraryService
{
    private static readonly string LibraryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DeskForge", "my_games.json");

    public static List<AppShortcut> Load()
    {
        try
        {
            if (File.Exists(LibraryPath))
            {
                var json = File.ReadAllText(LibraryPath);
                return JsonSerializer.Deserialize<List<AppShortcut>>(json) ?? new List<AppShortcut>();
            }
        }
        catch (IOException) { }
        catch (JsonException) { }

        return new List<AppShortcut>();
    }

    public static void Save(IEnumerable<AppShortcut> games)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LibraryPath)!);
            File.WriteAllText(LibraryPath, JsonSerializer.Serialize(games.ToList()));
        }
        catch (IOException)
        {
            // Non-fatal: the list just won't persist for this run.
        }
        catch (JsonException)
        {
            // Non-fatal: same as above.
        }
    }
}
