using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DeskForge.Services;

/// <summary>
/// App-wide singleton holding the numbers shown on the Dashboard's stat cards.
/// Other pages (Launcher, File Cleaner) update it as the user takes action, so the
/// Dashboard reflects real activity instead of static placeholder numbers.
/// </summary>
public sealed class StatsService : INotifyPropertyChanged
{
    public static StatsService Instance { get; } = new();

    private StatsService() { }

    public event PropertyChangedEventHandler? PropertyChanged;

    public int ToolsAvailable => 5;

    private int _filesScanned;
    public int FilesScanned
    {
        get => _filesScanned;
        set { if (_filesScanned != value) { _filesScanned = value; Raise(); } }
    }

    private int _shortcutsCount;
    public int ShortcutsCount
    {
        get => _shortcutsCount;
        set { if (_shortcutsCount != value) { _shortcutsCount = value; Raise(); } }
    }

    private long _storageSavedBytes;
    public long StorageSavedBytes
    {
        get => _storageSavedBytes;
        set
        {
            if (_storageSavedBytes == value) return;
            _storageSavedBytes = value;
            Raise();
            Raise(nameof(StorageSavedDisplay));
        }
    }

    public string StorageSavedDisplay => FormatBytes(StorageSavedBytes);

    public static string FormatBytes(long bytes)
    {
        string[] units = { "B", "KB", "MB", "GB", "TB" };
        double size = bytes;
        int unit = 0;
        while (size >= 1024 && unit < units.Length - 1)
        {
            size /= 1024;
            unit++;
        }

        return $"{size:0.#} {units[unit]}";
    }

    private void Raise([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
