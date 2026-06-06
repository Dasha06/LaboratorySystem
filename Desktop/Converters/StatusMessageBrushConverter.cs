using System;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Desktop.Converters;

public class StatusMessageBrushConverter : IValueConverter
{
    public static readonly StatusMessageBrushConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is string message)
        {
            if (message.StartsWith("Ошибка", StringComparison.OrdinalIgnoreCase))
                return new SolidColorBrush(Colors.Red);
            if (message.Contains("не найден", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("нет в базе", StringComparison.OrdinalIgnoreCase))
                return new SolidColorBrush(Colors.Red);
            return new SolidColorBrush(Colors.DarkGreen);
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}