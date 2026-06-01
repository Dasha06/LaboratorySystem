using System;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Desktop.Converters;

public class SelectedAnalysisBackgroundConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is bool isSelected && isSelected)
        {
            return Brushes.LightBlue;
        }

        return Brushes.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
