using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Desktop.Converters;

public class RackCellBrushConverter : IValueConverter
{
    public static readonly RackCellBrushConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var occupied = value is true;
        return occupied ? Brushes.Black : new SolidColorBrush(Color.Parse("#D0D0D0"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
