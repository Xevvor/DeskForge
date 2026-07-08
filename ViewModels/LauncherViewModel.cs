using System.Collections.ObjectModel;
using System.Windows;
using DeskForge.Models;
using DeskForge.Services;
using Microsoft.Win32;

namespace DeskForge.ViewModels;

/// <summary>
/// Backs the Launcher page. Flow: Home -&gt; "Launch" shows games already added, "Find Game" shows
/// a big pre-listed catalog; picking one triggers a real install-location scan, and either adds
/// it automatically or asks the user to cancel or locate the executable manually.
/// </summary>
public class LauncherViewModel : ViewModelBase
{
    public ObservableCollection<AppShortcut> MyGames { get; } = new();
    public ObservableCollection<GameCatalogEntry> FilteredCatalog { get; } = new();

    private readonly List<GameCatalogEntry> _catalog = GameCatalogService.All;

    private LauncherMode _mode = LauncherMode.Home;
    public LauncherMode Mode
    {
        get => _mode;
        set => SetField(ref _mode, value);
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetField(ref _searchText, value)) ApplyFilter();
        }
    }

    private bool _isDetecting;
    public bool IsDetecting
    {
        get => _isDetecting;
        set => SetField(ref _isDetecting, value);
    }

    private string _detectingGameName = string.Empty;
    public string DetectingGameName
    {
        get => _detectingGameName;
        set => SetField(ref _detectingGameName, value);
    }

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetField(ref _statusMessage, value);
    }

    public RelayCommand GoHomeCommand { get; }
    public RelayCommand GoToMyGamesCommand { get; }
    public RelayCommand GoToFindGameCommand { get; }
    public RelayCommand LaunchCommand { get; }
    public RelayCommand SelectCatalogGameCommand { get; }

    public LauncherViewModel()
    {
        GoHomeCommand = new RelayCommand(() => Mode = LauncherMode.Home);
        GoToMyGamesCommand = new RelayCommand(() => Mode = LauncherMode.MyGames);
        GoToFindGameCommand = new RelayCommand(() =>
        {
            StatusMessage = string.Empty;
            Mode = LauncherMode.FindGame;
        });

        LaunchCommand = new RelayCommand(param =>
        {
            if (param is AppShortcut game)
            {
                LauncherService.TryLaunch(game.Path, out var message);
                StatusMessage = message;
            }
        });

        SelectCatalogGameCommand = new RelayCommand(async param =>
        {
            if (param is GameCatalogEntry entry) await DetectAndAddAsync(entry);
        });

        // Restore any games saved from a previous session, then save again on every future change.
        foreach (var game in GameLibraryService.Load()) MyGames.Add(game);
        StatsService.Instance.ShortcutsCount = MyGames.Count;
        MyGames.CollectionChanged += (_, _) => GameLibraryService.Save(MyGames);

        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredCatalog.Clear();
        var query = _catalog.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
            query = query.Where(g => g.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var game in query)
        {
            FilteredCatalog.Add(game);
            _ = GameArtworkService.ResolveAsync(game); // fire-and-forget; entry raises PropertyChanged when art arrives
        }
    }

    private async Task DetectAndAddAsync(GameCatalogEntry entry)
    {
        if (MyGames.Any(g => g.Name == entry.Name))
        {
            StatusMessage = $"{entry.Name} is already in your Launch list.";
            return;
        }

        IsDetecting = true;
        DetectingGameName = entry.Name;
        StatusMessage = string.Empty;

        // A short delay makes this read as a real disk scan rather than an instant, unconvincing check.
        var foundPath = await Task.Run(async () =>
        {
            await Task.Delay(550);
            return GameDetectionService.TryFind(entry);
        });

        // Make sure artwork is resolved before it's copied onto the new AppShortcut below.
        await GameArtworkService.ResolveAsync(entry);

        IsDetecting = false;

        if (foundPath is not null)
        {
            MyGames.Add(new AppShortcut
            {
                Name = entry.Name,
                Path = foundPath,
                Category = "Detected",
                ImageUrl = entry.CoverImageUrl,
                Icon = entry.Icon,
                AccentColor = entry.AccentColor
            });
            StatsService.Instance.ShortcutsCount = MyGames.Count;
            StatusMessage = "Added to Launch list.";
            return;
        }

        var choice = MessageBox.Show(
            $"DeskForge couldn't find {entry.Name} in its usual install location.\n\n" +
            "Add it manually by picking its executable, or cancel.",
            "Game Not Found",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (choice != MessageBoxResult.Yes)
        {
            StatusMessage = $"Skipped {entry.Name}.";
            return;
        }

        var dialog = new OpenFileDialog
        {
            Title = $"Locate {entry.Name}",
            Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*"
        };

        if (dialog.ShowDialog() == true)
        {
            MyGames.Add(new AppShortcut
            {
                Name = entry.Name,
                Path = dialog.FileName,
                Category = "Manual",
                ImageUrl = entry.CoverImageUrl,
                Icon = entry.Icon,
                AccentColor = entry.AccentColor
            });
            StatsService.Instance.ShortcutsCount = MyGames.Count;
            StatusMessage = "Added to Launch list.";
        }
        else
        {
            StatusMessage = $"Skipped {entry.Name}.";
        }
    }
}
