using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Desktop.Converters;

public class GridSizeToWidthConverter : IValueConverter
{
    public static readonly GridSizeToWidthConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        if (!int.TryParse(value.ToString(), out var gridSize))
            return null;

        // Default item total width (including margins) if no parameter provided
        var itemTotal = 36;
        if (parameter != null && int.TryParse(parameter.ToString(), out var p))
            itemTotal = p;

        return (double)(gridSize * itemTotal);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
