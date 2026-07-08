using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DeskForge.Converters;

/// <summary>Shows the element only when the bound count is zero; used for "nothing here yet" empty states.</summary>
public class ZeroCountToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is int count && count == 0 ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
