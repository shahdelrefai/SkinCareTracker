namespace SkinCareTracker.Models
{
    public class SkinPhoto
    {
        public int Id { get; set; }
        public int SkinRemarkId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public DateTime TakenAt { get; set; } = DateTime.Now;
        public string Caption { get; set; } = string.Empty;

        public virtual SkinRemark SkinRemark { get; set; } = null!;
    }
}