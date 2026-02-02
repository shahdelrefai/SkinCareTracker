using System.Globalization;

namespace SkinCareTracker.Converters
{
    public class MealTypeToEmojiConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string mealType)
            {
                return mealType.ToLower() switch
                {
                    "breakfast" => "🍳",
                    "lunch" => "🥗",
                    "dinner" => "🍽️",
                    "snack" => "🍎",
                    _ => "🍴"
                };
            }
            return "🍴";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}