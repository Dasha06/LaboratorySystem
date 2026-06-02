using System;
using Avalonia.Data.Converters;

namespace Desktop.Converters;

public class StringEqualsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is string str && parameter is string param)
            return str.Equals(param, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
