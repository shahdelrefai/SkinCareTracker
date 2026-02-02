namespace SkinCareTracker.Models
{
    public class PeriodCycle
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime? EndDate { get; set; }
        public int FlowIntensity { get; set; }
        public string Symptoms { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}