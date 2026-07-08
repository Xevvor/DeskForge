using System.Windows;

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
        UpdateMaximizeIcon();
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
