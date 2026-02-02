using System.Globalization;

namespace SkinCareTracker.Converters
{
    public class BoolToBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isArchived && isArchived)
            {
                return Application.Current?.RequestedTheme == AppTheme.Dark 
                    ? Color.FromArgb("#2C2C2E") 
                    : Color.FromArgb("#F5F5F5");
            }
            return Colors.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}