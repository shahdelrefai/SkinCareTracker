using System.Globalization;

namespace SkinCareTracker.Converters
{
    public class StringEqualsConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string stringValue && parameter is string parameterValue)
            {
                return stringValue.Equals(parameterValue, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
