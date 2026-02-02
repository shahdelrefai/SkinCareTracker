using System.Globalization;

namespace SkinCareTracker.Converters
{
    public class BoolToStatusConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isArchived)
                return isArchived ? "Archived" : "Active";
            return "Active";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}