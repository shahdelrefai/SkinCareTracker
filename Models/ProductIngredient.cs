namespace SkinCareTracker.Models
{
    public class ProductIngredient
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string ComedogenicRating { get; set; } = string.Empty;
        public string IrritancyRating { get; set; } = string.Empty;

        public virtual Product Product { get; set; } = null!;
    }
}