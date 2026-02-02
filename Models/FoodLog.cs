namespace SkinCareTracker.Models
{
    public class FoodLog
    {
        public int Id { get; set; }
        public int DailyLogId { get; set; }
        public string MealType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FoodItems { get; set; } = string.Empty; // JSON
        public DateTime LoggedAt { get; set; } = DateTime.Now;

        public virtual DailyLog DailyLog { get; set; } = null!;
    }
}