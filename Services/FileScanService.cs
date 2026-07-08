using System.IO;
using DeskForge.Models;

namespace DeskForge.Services;

/// <summary>
/// Scans a real folder on disk and flags files as Large, Old, Duplicate, or Temporary.
/// Read-only: this service never deletes or moves anything.
/// </summary>
public static class FileScanService
{
    private static readonly string[] TempExtensions = { ".tmp", ".temp", ".log", ".bak", ".dmp" };
    private const long LargeFileThresholdBytes = 25L * 1024 * 1024; // 25 MB
    private const int OldFileThresholdDays = 30;

    public static List<ScannedFile> ScanFolder(string folderPath)
    {
        var results = new List<ScannedFile>();
        if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            return results;

        List<FileInfo> files;
        try
        {
            // Capped for prototype responsiveness on very large trees.
            files = new DirectoryInfo(folderPath)
                .EnumerateFiles("*", SearchOption.AllDirectories)
                .Take(2000)
                .ToList();
        }
        catch (UnauthorizedAccessException)
        {
            return results;
        }
        catch (IOException)
        {
            return results;
        }

        var duplicatePaths = files
            .GroupBy(f => (f.Name.ToLowerInvariant(), f.Length))
            .Where(g => g.Count() > 1)
            .SelectMany(g => g)
            .Select(f => f.FullName)
            .ToHashSet();

        foreach (var file in files)
        {
            var category = Categorize(file, duplicatePaths.Contains(file.FullName));
            if (category is null) continue;

            results.Add(new ScannedFile
            {
                FileName = file.Name,
                SizeBytes = file.Length,
                SizeDisplay = StatsService.FormatBytes(file.Length),
                FileType = string.IsNullOrEmpty(file.Extension) ? "File" : file.Extension.TrimStart('.').ToUpperInvariant(),
                Location = file.DirectoryName ?? folderPath,
                Category = category
            });
        }

        return results.OrderByDescending(r => r.SizeBytes).ToList();
    }

    private static string? Categorize(FileInfo file, bool isDuplicate)
    {
        if (isDuplicate) return "Duplicate";
        if (TempExtensions.Contains(file.Extension.ToLowerInvariant())) return "Temporary";
        if (file.Length >= LargeFileThresholdBytes) return "Large File";
        if ((DateTime.Now - file.LastWriteTime).TotalDays >= OldFileThresholdDays) return "Old File";
        return null;
    }
}
