using DeskForge.Services;

namespace DeskForge.ViewModels;

/// <summary>Backs the Dashboard page: quick stats and shortcut buttons to the other tools.</summary>
public class DashboardViewModel : ViewModelBase
{
    /// <summary>Exposed directly so the view can bind Stats.FilesScanned etc; updates live as other pages act.</summary>
    public StatsService Stats => StatsService.Instance;

    public RelayCommand CleanDownloadsCommand { get; }
    public RelayCommand OpenLauncherCommand { get; }
    public RelayCommand ScanFilesCommand { get; }

    public DashboardViewModel()
    {
        CleanDownloadsCommand = new RelayCommand(() => NavigationService.RequestNavigate("FileCleaner"));
        OpenLauncherCommand = new RelayCommand(() => NavigationService.RequestNavigate("Launcher"));
        ScanFilesCommand = new RelayCommand(() => NavigationService.RequestNavigate("FileCleaner"));
    }
}
