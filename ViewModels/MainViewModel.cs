using System.Collections.ObjectModel;
using System.Linq;
using DeskForge.Models;
using DeskForge.Services;

namespace DeskForge.ViewModels;

/// <summary>
/// Root view model for the shell: owns the sidebar navigation state and one instance
/// of each page view model (pages keep their state when you navigate away and back).
/// </summary>
public class MainViewModel : ViewModelBase
{
    public DashboardViewModel Dashboard { get; } = new();
    public LauncherViewModel Launcher { get; } = new();
    public FileCleanerViewModel FileCleaner { get; } = new();
    public FileOrganizerViewModel FileOrganizer { get; } = new();
    public QuickToolsViewModel QuickTools { get; } = new();

    public ObservableCollection<NavItem> NavItems { get; } = new()
    {
        new NavItem { Key = "Dashboard", Title = "Dashboard", Icon = "🏠" },
        new NavItem { Key = "Launcher", Title = "Launcher", Icon = "🚀" },
        new NavItem { Key = "FileCleaner", Title = "File Cleaner", Icon = "🧹" },
        new NavItem { Key = "FileOrganizer", Title = "File Organizer", Icon = "🗂️" },
        new NavItem { Key = "QuickTools", Title = "Quick Tools", Icon = "🧰" },
    };

    private NavItem? _selectedNavItem;
    public NavItem? SelectedNavItem
    {
        get => _selectedNavItem;
        set
        {
            if (SetField(ref _selectedNavItem, value) && value is not null)
                CurrentViewModel = ResolveViewModel(value.Key);
        }
    }

    private object? _currentViewModel;
    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetField(ref _currentViewModel, value);
    }

    public MainViewModel()
    {
        NavigationService.NavigationRequested += key =>
        {
            var target = NavItems.FirstOrDefault(n => n.Key == key);
            if (target is not null) SelectedNavItem = target;
        };

        SelectedNavItem = NavItems[0];
    }

    private object ResolveViewModel(string key) => key switch
    {
        "Dashboard" => Dashboard,
        "Launcher" => Launcher,
        "FileCleaner" => FileCleaner,
        "FileOrganizer" => FileOrganizer,
        "QuickTools" => QuickTools,
        _ => Dashboard
    };
}
