using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Desktop.Converters;

public class MaterialTypeToBrushConverter : IValueConverter
{
    public static readonly MaterialTypeToBrushConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var materialType = (value as string)?.ToLowerInvariant() ?? string.Empty;

        if (materialType.Contains("сыворот") || materialType.Contains("сывор"))
            return new SolidColorBrush(Color.Parse("#E74C3C"));
        if (materialType.Contains("веноз") || materialType.Contains("венозн"))
            return new SolidColorBrush(Color.Parse("#8E44AD"));
        if (materialType.Contains("плазм") || materialType.Contains("плазма"))
            return new SolidColorBrush(Color.Parse("#5DADE2"));

        return new SolidColorBrush(Color.Parse("#D0D0D0"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
