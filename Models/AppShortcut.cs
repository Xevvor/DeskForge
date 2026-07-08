using System.Text.Json.Serialization;
using System.Windows.Media;

namespace DeskForge.Models;

/// <summary>An app or game entry saved in the Launcher's "Your Games" list.</summary>
public class AppShortcut
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Category { get; set; } = "App";

    /// <summary>Resolved cover art URL, copied from the catalog entry at add-time; null falls back to the placeholder tile.</summary>
    public string? ImageUrl { get; set; }

    public string Icon { get; set; } = "🎮";
    public string AccentColor { get; set; } = "#7C5CFC";

    /// <summary>Computed for display only — WPF Brush objects have circular references and must never be serialized.</summary>
    [JsonIgnore]
    public Brush AccentBrush => (Brush)(new BrushConverter().ConvertFromString(AccentColor) ?? Brushes.SlateGray);
}
