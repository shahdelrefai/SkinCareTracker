namespace SkinCareTracker.Models
{
    public class RoutineSchedule
    {
        public int Id { get; set; }
        public int RoutineId { get; set; }
        public string ScheduledDays { get; set; } = string.Empty; // JSON
        public TimeSpan ScheduledTime { get; set; }
        public bool EnableReminders { get; set; }

        public virtual Routine Routine { get; set; } = null!;
    }
}