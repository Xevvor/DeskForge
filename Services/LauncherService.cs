using System.Diagnostics;

namespace DeskForge.Services;

/// <summary>Launches executables, shell folders, and URIs via the OS shell.</summary>
public static class LauncherService
{
    public static bool TryLaunch(string path, out string message)
    {
        try
        {
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            message = $"Launched \"{path}\".";
            return true;
        }
        catch (Exception ex)
        {
            message = $"Couldn't launch \"{path}\": {ex.Message}";
            return false;
        }
    }
}
