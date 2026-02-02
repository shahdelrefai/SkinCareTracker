namespace SkinCareTracker.Models
{
    public class SkinRemark
    {
        public int Id { get; set; }
        public int DailyLogId { get; set; }
        public string Observation { get; set; } = string.Empty;
        public int? AcneLevel { get; set; }
        public int? DrynessLevel { get; set; }
        public int? OilinessLevel { get; set; }
        public int? RednessLevel { get; set; }
        public string SkinFeeling { get; set; } = string.Empty;
        
        public virtual DailyLog DailyLog { get; set; } = null!;
        public virtual ICollection<SkinPhoto> Photos { get; set; } = new List<SkinPhoto>();
    }
}