namespace DeskForge.Models;

/// <summary>A file found by the File Cleaner scan, with a reason it was flagged.</summary>
public class ScannedFile
{
    public bool IsSelected { get; set; }
    public string FileName { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public string SizeDisplay { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    /// <summary>Large File, Old File, Duplicate, or Temporary.</summary>
    public string Category { get; set; } = string.Empty;
}
