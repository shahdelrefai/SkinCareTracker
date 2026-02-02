namespace SkinCareTracker.Models
{
    public class RoutineStep
    {
        public int Id { get; set; }
        public int RoutineId { get; set; }
        public int Order { get; set; }
        public int ProductId { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public int WaitTimeMinutes { get; set; }

        public virtual Routine Routine { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}