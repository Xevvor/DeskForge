namespace DeskForge.Models;

/// <summary>A rule mapping a file category to a destination folder, e.g. "Images" -&gt; "Pictures".</summary>
public class OrganizeRule
{
    public string SourceType { get; set; } = string.Empty;
    public string TargetFolder { get; set; } = string.Empty;

    /// <summary>Comma-separated, lower-case extensions this rule matches, e.g. ".jpg,.png".</summary>
    public string Extensions { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = true;
}
