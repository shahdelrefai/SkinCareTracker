using System.ComponentModel.DataAnnotations.Schema;

namespace SkinCareTracker.Models
{
    public class SkinPhoto
    {
        public int Id { get; set; }
        public int SkinRemarkId { get; set; }
        
        // Store ONLY the filename (not full path) so it survives app reinstalls
        public string FilePath { get; set; } = string.Empty;
        public DateTime TakenAt { get; set; } = DateTime.Now;
        public string Caption { get; set; } = string.Empty;

        public virtual SkinRemark SkinRemark { get; set; } = null!;

        // Reconstructs full path at runtime (handles app container UUID changes on iOS)
        [NotMapped]
        public string FullFilePath => Path.IsPathRooted(FilePath) 
            ? FilePath 
            : Path.Combine(FileSystem.AppDataDirectory, FilePath);

        // Cached image source - not stored in database
        private ImageSource? _cachedImageSource;
        
        [NotMapped]
        public ImageSource? PhotoSource
        {
            get
            {
                if (_cachedImageSource == null)
                {
                    var fullPath = FullFilePath;
                    if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                    {
                        var bytes = File.ReadAllBytes(fullPath);
                        _cachedImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                    }
                }
                return _cachedImageSource;
            }
        }
    }
}