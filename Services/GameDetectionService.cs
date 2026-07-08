using System.IO;
using System.Text.RegularExpressions;
using DeskForge.Models;

namespace DeskForge.Services;

/// <summary>
/// Looks for a catalog game's executable in the places Windows games usually land: every
/// discovered Steam library (including custom library drives via libraryfolders.vdf), plus
/// any absolute path templates the entry declares for non-Steam launchers.
/// </summary>
public static class GameDetectionService
{
    public static string? TryFind(GameCatalogEntry entry)
    {
        foreach (var candidate in entry.AbsoluteCandidates)
        {
            var expanded = Environment.ExpandEnvironmentVariables(candidate);
            if (File.Exists(expanded)) return expanded;
        }

        if (entry.SteamRelativePath is not null)
        {
            foreach (var libraryRoot in GetSteamLibraryRoots())
            {
                var candidate = Path.Combine(libraryRoot, entry.SteamRelativePath);
                if (File.Exists(candidate)) return candidate;
            }
        }

        return null;
    }

    private static List<string> GetSteamLibraryRoots()
    {
        var roots = new List<string>();
        var steamRoot = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\Steam");
        AddCommonFolder(roots, steamRoot);

        var libraryFile = Path.Combine(steamRoot, "steamapps", "libraryfolders.vdf");
        if (!File.Exists(libraryFile)) return roots;

        try
        {
            var text = File.ReadAllText(libraryFile);
            foreach (Match match in Regex.Matches(text, "\"path\"\\s*\"([^\"]+)\""))
            {
                var libraryPath = match.Groups[1].Value.Replace(@"\\", @"\");
                AddCommonFolder(roots, libraryPath);
            }
        }
        catch (IOException)
        {
            // Ignore: fall back to whatever roots we already found.
        }

        return roots;
    }

    private static void AddCommonFolder(List<string> roots, string steamInstallRoot)
    {
        var common = Path.Combine(steamInstallRoot, "steamapps", "common");
        if (Directory.Exists(common) && !roots.Contains(common)) roots.Add(common);
    }
}
