using System;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Desktop.Converters;

public class BoolToSelectionColorConverter : IValueConverter
{
    public static readonly BoolToSelectionColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return value is true ? new SolidColorBrush(Color.FromRgb(255, 215, 0)) : Brushes.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}