using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DeskForge;

/// <summary>
/// The app shell. Hosts the sidebar, the custom title bar, and swaps page content;
/// all real logic lives in the view models.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        StateChanged += (_, _) => UpdateMaximizeIcon();
        SourceInitialized += (_, _) => ApplyRoundedCorners();
        UpdateMaximizeIcon();
    }

    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(nint hwnd, int attribute, ref int value, int valueSize);

    private const int DwmwaWindowCornerPreference = 33;
    private const int DwmwcpRoundSmall = 3;

    /// <summary>Windows 11 only; silently ignored (no-op) on older Windows since DWM rejects the unknown attribute.</summary>
    private void ApplyRoundedCorners()
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        var preference = DwmwcpRoundSmall;
        DwmSetWindowAttribute(hwnd, DwmwaWindowCornerPreference, ref preference, sizeof(int));
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

    private void UpdateMaximizeIcon()
    {
        var isMaximized = WindowState == WindowState.Maximized;
        MaximizeIcon.Visibility = isMaximized ? Visibility.Collapsed : Visibility.Visible;
        RestoreIcon.Visibility = isMaximized ? Visibility.Visible : Visibility.Collapsed;
    }
}
