using System.Globalization;

namespace SkinCareTracker.Converters
{
    public class BoolToArchiveTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool showArchived)
                return showArchived ? "Hide Archived" : "Show Archived";
            return "Show Archived";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}