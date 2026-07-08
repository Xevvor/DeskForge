namespace DeskForge.Models;

/// <summary>One planned (not yet executed) file move produced by the Organize Preview.</summary>
public class OrganizePreviewItem
{
    public string FileName { get; set; } = string.Empty;
    public string CurrentPath { get; set; } = string.Empty;
    public string DestinationFolder { get; set; } = string.Empty;
    public string RuleApplied { get; set; } = string.Empty;
}
