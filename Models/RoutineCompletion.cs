namespace SkinCareTracker.Models
{
    public class RoutineCompletion
    {
        public int Id { get; set; }
        public int DailyLogId { get; set; }
        public int RoutineId { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.Now;
        public string CompletedStepIds { get; set; } = string.Empty; // JSON
        public string Notes { get; set; } = string.Empty;

        public virtual DailyLog DailyLog { get; set; } = null!;
        public virtual Routine Routine { get; set; } = null!;
    }
}