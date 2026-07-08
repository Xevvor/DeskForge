using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DeskForge.Converters;

/// <summary>Shows the element when the bound enum's string form matches the converter parameter.</summary>
public class EnumEqualsToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is not null && parameter is not null && value.ToString() == parameter.ToString()
            ? Visibility.Visible
            : Visibility.Collapsed;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
