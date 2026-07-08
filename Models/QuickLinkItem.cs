namespace DeskForge.Models;

/// <summary>A saved shortcut to a folder or URL shown on the Quick Tools page.</summary>
public class QuickLinkItem
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}
