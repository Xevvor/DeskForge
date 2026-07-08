using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace DeskForge.Models;

/// <summary>A pre-listed game the user can pick from in "Find Game", before it's confirmed installed.</summary>
public class GameCatalogEntry : INotifyPropertyChanged
{
    public string Name { get; init; } = string.Empty;
    public string Icon { get; init; } = "🎮";
    public string AccentColor { get; init; } = "#7C5CFC";

    /// <summary>Path relative to a Steam "steamapps\common" folder, e.g. "Grand Theft Auto V\GTA5.exe".</summary>
    public string? SteamRelativePath { get; init; }

    /// <summary>Fully expandable absolute path templates (may use %ProgramFiles%, %LocalAppData%, etc).</summary>
    public List<string> AbsoluteCandidates { get; init; } = new();

    /// <summary>Steam App ID, when known — lets GameArtworkService build Steam CDN artwork URLs directly.</summary>
    public int? SteamAppId { get; init; }

    private string? _coverImageUrl;
    /// <summary>Resolved real cover art URL (Steam CDN or RAWG), set asynchronously after lookup.</summary>
    public string? CoverImageUrl
    {
        get => _coverImageUrl;
        set
        {
            if (_coverImageUrl == value) return;
            _coverImageUrl = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CoverImageUrl)));
        }
    }

    private string? _headerImageUrl;
    public string? HeaderImageUrl
    {
        get => _headerImageUrl;
        set
        {
            if (_headerImageUrl == value) return;
            _headerImageUrl = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeaderImageUrl)));
        }
    }

    /// <summary>Placeholder "cover art" brush computed from AccentColor, shown until/unless real artwork loads.</summary>
    [JsonIgnore]
    public Brush AccentBrush => (Brush)(new BrushConverter().ConvertFromString(AccentColor) ?? Brushes.SlateGray);

    public event PropertyChangedEventHandler? PropertyChanged;
}
