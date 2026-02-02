using System.Globalization;

namespace SkinCareTracker.Converters
{
    public class BoolToReminderTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool enabled)
                return enabled ? "Reminders ON" : "Reminders OFF";
            return "Reminders OFF";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}