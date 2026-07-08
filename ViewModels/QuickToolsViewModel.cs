using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using DeskForge.Models;
using DeskForge.Services;

namespace DeskForge.ViewModels;

/// <summary>Backs the Quick Tools page: scratch notes, folder/link shortcuts, and system utilities.</summary>
public class QuickToolsViewModel : ViewModelBase
{
    public ObservableCollection<QuickLinkItem> QuickLinks { get; } = new();

    private static readonly string NotesFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DeskForge", "notes.txt");

    private string _noteText = string.Empty;
    public string NoteText
    {
        get => _noteText;
        set => SetField(ref _noteText, value);
    }

    private string _statusMessage = "Ready.";
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetField(ref _statusMessage, value);
    }

    public RelayCommand SaveNoteCommand { get; }
    public RelayCommand CopyNoteCommand { get; }
    public RelayCommand ClearNoteCommand { get; }
    public RelayCommand OpenLinkCommand { get; }
    public RelayCommand RunShortcutCommand { get; }

    public QuickToolsViewModel()
    {
        SaveNoteCommand = new RelayCommand(() =>
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(NotesFilePath)!);
                File.WriteAllText(NotesFilePath, NoteText);
                StatusMessage = "Note saved.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Couldn't save note: {ex.Message}";
            }
        });

        CopyNoteCommand = new RelayCommand(() =>
        {
            if (string.IsNullOrEmpty(NoteText)) return;
            Clipboard.SetText(NoteText);
            StatusMessage = "Note copied to clipboard.";
        });

        ClearNoteCommand = new RelayCommand(() =>
        {
            NoteText = string.Empty;
            StatusMessage = "Note cleared.";
        });

        OpenLinkCommand = new RelayCommand(param =>
        {
            if (param is not QuickLinkItem link) return;
            LauncherService.TryLaunch(link.Path, out var message);
            StatusMessage = message;
        });

        RunShortcutCommand = new RelayCommand(param =>
        {
            if (param is not string command) return;
            LauncherService.TryLaunch(command, out var message);
            StatusMessage = message;
        });

        LoadNote();
        SeedLinks();
    }

    private void LoadNote()
    {
        try
        {
            if (File.Exists(NotesFilePath))
                NoteText = File.ReadAllText(NotesFilePath);
        }
        catch (IOException)
        {
            // Non-fatal for a prototype; the note simply starts empty.
        }
    }

    private void SeedLinks()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var downloads = Path.Combine(userProfile, "Downloads");

        QuickLinks.Add(new QuickLinkItem { Name = "Downloads Folder", Path = downloads, Icon = "📥" });
        QuickLinks.Add(new QuickLinkItem { Name = "Documents Folder", Path = docs, Icon = "📄" });
        QuickLinks.Add(new QuickLinkItem { Name = "GitHub", Path = "https://github.com", Icon = "🔗" });
        QuickLinks.Add(new QuickLinkItem { Name = "Windows Settings", Path = "ms-settings:", Icon = "⚙️" });
    }
}
