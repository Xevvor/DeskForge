using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using DeskForge.Services;
using Microsoft.Win32;

namespace DeskForge.ViewModels;

/// <summary>
/// Backs the File Cleaner page. Scanning reads real files from disk; cleaning is simulated
/// (nothing is deleted) but requires an explicit confirmation, matching the prototype scope.
/// </summary>
public class FileCleanerViewModel : ViewModelBase
{
    public ObservableCollection<Models.ScannedFile> Results { get; } = new();

    private string _selectedFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    public string SelectedFolder
    {
        get => _selectedFolder;
        set => SetField(ref _selectedFolder, value);
    }

    private bool _isScanning;
    public bool IsScanning
    {
        get => _isScanning;
        set => SetField(ref _isScanning, value);
    }

    private string _statusMessage = "Choose a folder and click Scan Folder to begin.";
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetField(ref _statusMessage, value);
    }

    public RelayCommand ChooseFolderCommand { get; }
    public RelayCommand ScanFolderCommand { get; }
    public RelayCommand CleanSelectedCommand { get; }

    public FileCleanerViewModel()
    {
        ChooseFolderCommand = new RelayCommand(() =>
        {
            var dialog = new OpenFolderDialog { Title = "Choose a folder to scan" };
            if (dialog.ShowDialog() == true)
                SelectedFolder = dialog.FolderName;
        });

        ScanFolderCommand = new RelayCommand(async () => await ScanAsync());
        CleanSelectedCommand = new RelayCommand(CleanSelected);
    }

    private async Task ScanAsync()
    {
        if (!Directory.Exists(SelectedFolder))
        {
            StatusMessage = "That folder doesn't exist. Choose another one.";
            return;
        }

        IsScanning = true;
        StatusMessage = "Scanning...";
        Results.Clear();

        var found = await Task.Run(() => FileScanService.ScanFolder(SelectedFolder));
        foreach (var file in found) Results.Add(file);

        StatsService.Instance.FilesScanned += found.Count;
        StatusMessage = found.Count == 0
            ? "No large, old, duplicate, or temporary files found."
            : $"Found {found.Count} file(s) worth reviewing.";
        IsScanning = false;
    }

    private void CleanSelected(object? _)
    {
        var selected = Results.Where(r => r.IsSelected).ToList();
        if (selected.Count == 0)
        {
            StatusMessage = "Select one or more files first.";
            return;
        }

        long totalBytes = selected.Sum(f => f.SizeBytes);
        var confirm = MessageBox.Show(
            $"This prototype won't delete real files.\n\nIn the full version, {selected.Count} file(s) totaling " +
            $"{StatsService.FormatBytes(totalBytes)} would be removed.\n\nSimulate the cleanup now?",
            "Clean Selected Files",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes) return;

        foreach (var file in selected) Results.Remove(file);
        StatsService.Instance.StorageSavedBytes += totalBytes;
        StatusMessage = $"Simulated cleanup of {selected.Count} file(s), freeing {StatsService.FormatBytes(totalBytes)}.";
    }
}
