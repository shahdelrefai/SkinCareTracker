namespace SkinCareTracker.Models
{
    public class Routine
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TimeOfDay { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public bool IsArchived { get; set; } = false;

        public virtual ICollection<RoutineStep> Steps { get; set; } = new List<RoutineStep>();
        public virtual RoutineSchedule? Schedule { get; set; }
    }
}