using System;
using Avalonia.Data.Converters;

namespace Desktop.Converters;

public class IsSelectedToThicknessConverter : IValueConverter
{
    public static readonly IsSelectedToThicknessConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return value is true ? 3 : 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}