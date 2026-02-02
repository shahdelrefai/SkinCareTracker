using Microsoft.EntityFrameworkCore;
using SkinCareTracker.Models;

namespace SkinCareTracker.Services.Database
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllActiveAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.ProductIngredients)
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductIngredients)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetByIdForEditAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductIngredients)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product == null) return false;

            product.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> SearchAsync(string query)
        {
            query = query.ToLower();
            return await _context.Products
                .Where(p => p.IsActive &&
                       (p.Name.ToLower().Contains(query) ||
                        p.Brand.ToLower().Contains(query) ||
                        p.ProductIngredients.Any(pi => pi.IngredientName.ToLower().Contains(query))))
                .ToListAsync();
        }
    }
}