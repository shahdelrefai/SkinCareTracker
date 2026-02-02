using Microsoft.EntityFrameworkCore;
using SkinCareTracker.Models;

namespace SkinCareTracker.Services.Database
{
    public class RoutineRepository
    {
        private readonly AppDbContext _context;

        public RoutineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Routine>> GetAllActiveAsync()
        {
            return await _context.Routines
                .Where(r => !r.IsArchived)
                .Include(r => r.Steps)
                .ThenInclude(s => s.Product)
                .Include(r => r.Schedule)
                .OrderBy(r => r.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Routine>> GetArchivedAsync()
        {
            return await _context.Routines
                .Where(r => r.IsArchived)
                .Include(r => r.Steps)
                .ThenInclude(s => s.Product)
                .Include(r => r.Schedule)
                .OrderByDescending(r => r.EndDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Routine?> GetByIdAsync(int id)
        {
            return await _context.Routines
                .Include(r => r.Steps)
                .ThenInclude(s => s.Product)
                .Include(r => r.Schedule)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Routine> AddAsync(Routine routine)
        {
            _context.Routines.Add(routine);
            await _context.SaveChangesAsync();
            return routine;
        }

        public async Task<Routine> UpdateAsync(Routine routine)
        {
            _context.Routines.Update(routine);
            await _context.SaveChangesAsync();
            return routine;
        }

        public async Task<bool> ArchiveAsync(int id)
        {
            var routine = await GetByIdAsync(id);
            if (routine == null) return false;

            routine.IsArchived = true;
            routine.EndDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var routine = await GetByIdAsync(id);
            if (routine == null) return false;

            _context.Routines.Remove(routine);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}