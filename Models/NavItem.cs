namespace DeskForge.Models;

/// <summary>A single entry in the sidebar navigation list.</summary>
public class NavItem
{
    public string Key { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
}
