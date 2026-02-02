namespace SkinCareTracker.Models
{
    public class DailyLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;

        public virtual ICollection<RoutineCompletion> CompletedRoutines { get; set; } 
            = new List<RoutineCompletion>();
        public virtual ICollection<FoodLog> FoodLogs { get; set; } = new List<FoodLog>();
        public virtual SkinRemark? SkinRemark { get; set; }
    }
}