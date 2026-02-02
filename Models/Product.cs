namespace SkinCareTracker.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public DateTime? ExpiryDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        
        public virtual ICollection<ProductIngredient> ProductIngredients { get; set; } 
            = new List<ProductIngredient>();
    }
}