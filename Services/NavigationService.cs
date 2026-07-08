namespace DeskForge.Services;

/// <summary>
/// Lightweight event hub that lets any view model request a sidebar page switch
/// (e.g. Dashboard's "Open Launcher" button) without taking a direct dependency on MainViewModel.
/// </summary>
public static class NavigationService
{
    public static event Action<string>? NavigationRequested;

    public static void RequestNavigate(string targetKey) => NavigationRequested?.Invoke(targetKey);
}
