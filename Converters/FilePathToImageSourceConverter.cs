using System.Globalization;

namespace SkinCareTracker.Converters
{
    public class FilePathToImageSourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string filePath && !string.IsNullOrEmpty(filePath))
            {
                if (File.Exists(filePath))
                {
                    try
                    {
                        // Read file bytes into memory - this is required for iOS
                        // because ImageSource.FromFile doesn't work with app data paths
                        var bytes = File.ReadAllBytes(filePath);
                        return ImageSource.FromStream(() => new MemoryStream(bytes));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ImageConverter] Error: {ex.Message}");
                    }
                }
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}