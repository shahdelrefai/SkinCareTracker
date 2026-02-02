using Microsoft.EntityFrameworkCore;

namespace SkinCareTracker.Services.Database
{
    public class DatabaseService
    {
        private readonly AppDbContext _context;

        public DatabaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task InitializeDatabaseAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }
    }
}