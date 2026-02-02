namespace SkinCareTracker.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public bool IsFemale { get; set; }
        public int? AverageCycleLength { get; set; }
        public string SkinType { get; set; } = string.Empty;
        public string SkinConcerns { get; set; } = string.Empty; // JSON
    }
}