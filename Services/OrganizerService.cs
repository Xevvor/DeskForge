using System.IO;
using DeskForge.Models;

namespace DeskForge.Services;

/// <summary>
/// Computes what File Organizer *would* do to a folder, based on extension rules.
/// Preview-only: nothing here ever calls File.Move.
/// </summary>
public static class OrganizerService
{
    public static List<OrganizeRule> DefaultRules() => new()
    {
        new OrganizeRule { SourceType = "Images", TargetFolder = "Pictures", Extensions = ".jpg,.jpeg,.png,.gif,.bmp,.webp" },
        new OrganizeRule { SourceType = "Videos", TargetFolder = "Videos", Extensions = ".mp4,.mkv,.mov,.avi,.wmv" },
        new OrganizeRule { SourceType = "Documents", TargetFolder = "Documents", Extensions = ".pdf,.docx,.doc,.xlsx,.pptx,.txt" },
        new OrganizeRule { SourceType = "Music", TargetFolder = "Music", Extensions = ".mp3,.wav,.flac,.aac" },
        new OrganizeRule { SourceType = "Archives", TargetFolder = "Archives", Extensions = ".zip,.rar,.7z" },
    };

    public static List<OrganizePreviewItem> PreviewOrganize(string folderPath, IEnumerable<OrganizeRule> rules)
    {
        var preview = new List<OrganizePreviewItem>();
        if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            return preview;

        var activeRules = rules.Where(r => r.IsEnabled).ToList();

        IEnumerable<string> files;
        try
        {
            files = Directory.EnumerateFiles(folderPath);
        }
        catch (UnauthorizedAccessException)
        {
            return preview;
        }

        foreach (var filePath in files)
        {
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            var rule = activeRules.FirstOrDefault(r => r.Extensions.Split(',').Contains(ext));
            if (rule is null) continue;

            preview.Add(new OrganizePreviewItem
            {
                FileName = Path.GetFileName(filePath),
                CurrentPath = folderPath,
                DestinationFolder = Path.Combine(folderPath, rule.TargetFolder),
                RuleApplied = $"{rule.SourceType} → {rule.TargetFolder}"
            });
        }

        return preview;
    }
}
