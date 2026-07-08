using System.Collections.ObjectModel;
using System.IO;
using DeskForge.Models;
using DeskForge.Services;
using Microsoft.Win32;

namespace DeskForge.ViewModels;

/// <summary>Backs the File Organizer page. Preview-only: no files are ever moved here.</summary>
public class FileOrganizerViewModel : ViewModelBase
{
    public ObservableCollection<OrganizeRule> Rules { get; } = new(OrganizerService.DefaultRules());
    public ObservableCollection<OrganizePreviewItem> PreviewResults { get; } = new();

    private string _selectedFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    public string SelectedFolder
    {
        get => _selectedFolder;
        set => SetField(ref _selectedFolder, value);
    }

    private string _statusMessage = "Choose a folder, then preview how DeskForge would organize it.";
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetField(ref _statusMessage, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetField(ref _isBusy, value);
    }

    public RelayCommand ChooseFolderCommand { get; }
    public RelayCommand OrganizePreviewCommand { get; }

    public FileOrganizerViewModel()
    {
        ChooseFolderCommand = new RelayCommand(() =>
        {
            var dialog = new OpenFolderDialog { Title = "Choose a folder to organize" };
            if (dialog.ShowDialog() == true)
                SelectedFolder = dialog.FolderName;
        });

        OrganizePreviewCommand = new RelayCommand(async () => await PreviewAsync());
    }

    private async Task PreviewAsync()
    {
        if (!Directory.Exists(SelectedFolder))
        {
            StatusMessage = "That folder doesn't exist. Choose another one.";
            return;
        }

        IsBusy = true;
        PreviewResults.Clear();

        var preview = await Task.Run(async () =>
        {
            await Task.Delay(400);
            return OrganizerService.PreviewOrganize(SelectedFolder, Rules);
        });

        foreach (var item in preview) PreviewResults.Add(item);
        IsBusy = false;

        StatusMessage = preview.Count == 0
            ? "Nothing to organize — no files matched the active rules."
            : $"Preview ready: {preview.Count} file(s) would be moved. No files have been touched.";
    }
}
