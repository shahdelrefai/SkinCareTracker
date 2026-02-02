using Microsoft.EntityFrameworkCore;
using SkinCareTracker.Models;

namespace SkinCareTracker.Services.Database
{
    public class DailyLogRepository
    {
        private readonly AppDbContext _context;

        public DailyLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DailyLog?> GetByDateAsync(DateTime date)
        {
            var dateOnly = date.Date;
            return await _context.DailyLogs
                .Include(dl => dl.CompletedRoutines)
                    .ThenInclude(rc => rc.Routine)
                        .ThenInclude(r => r.Steps)
                .Include(dl => dl.FoodLogs)
                .Include(dl => dl.SkinRemark)
                    .ThenInclude(sr => sr.Photos)
                .FirstOrDefaultAsync(dl => dl.Date.Date == dateOnly);
        }

        public async Task<DailyLog> GetOrCreateByDateAsync(DateTime date)
        {
            var existing = await GetByDateAsync(date);
            if (existing != null)
                return existing;

            var newLog = new DailyLog
            {
                Date = date.Date
            };

            _context.DailyLogs.Add(newLog);
            await _context.SaveChangesAsync();
            return newLog;
        }

        public async Task<DailyLog> UpdateAsync(DailyLog dailyLog)
        {
            _context.DailyLogs.Update(dailyLog);
            await _context.SaveChangesAsync();
            return dailyLog;
        }

        public async Task<RoutineCompletion> AddRoutineCompletionAsync(int dailyLogId, RoutineCompletion completion)
        {
            completion.DailyLogId = dailyLogId;
            _context.RoutineCompletions.Add(completion);
            await _context.SaveChangesAsync();
            return completion;
        }

        public async Task<bool> RemoveRoutineCompletionAsync(int completionId)
        {
            var completion = await _context.RoutineCompletions.FindAsync(completionId);
            if (completion == null) return false;

            _context.RoutineCompletions.Remove(completion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FoodLog> AddFoodLogAsync(int dailyLogId, FoodLog foodLog)
        {
            foodLog.DailyLogId = dailyLogId;
            _context.FoodLogs.Add(foodLog);
            await _context.SaveChangesAsync();
            return foodLog;
        }

        public async Task<bool> RemoveFoodLogAsync(int foodLogId)
        {
            var foodLog = await _context.FoodLogs.FindAsync(foodLogId);
            if (foodLog == null) return false;

            _context.FoodLogs.Remove(foodLog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SkinRemark> SaveSkinRemarkAsync(int dailyLogId, SkinRemark skinRemark)
        {
            var existingRemark = await _context.SkinRemarks
                .Include(sr => sr.Photos)
                .FirstOrDefaultAsync(sr => sr.DailyLogId == dailyLogId);

            if (existingRemark != null)
            {
                existingRemark.Observation = skinRemark.Observation;
                existingRemark.AcneLevel = skinRemark.AcneLevel;
                existingRemark.DrynessLevel = skinRemark.DrynessLevel;
                existingRemark.OilinessLevel = skinRemark.OilinessLevel;
                existingRemark.RednessLevel = skinRemark.RednessLevel;
                existingRemark.SkinFeeling = skinRemark.SkinFeeling;
                _context.SkinRemarks.Update(existingRemark);
            }
            else
            {
                skinRemark.DailyLogId = dailyLogId;
                _context.SkinRemarks.Add(skinRemark);
            }

            await _context.SaveChangesAsync();
            return existingRemark ?? skinRemark;
        }

        public async Task<SkinPhoto> AddSkinPhotoAsync(int skinRemarkId, SkinPhoto photo)
        {
            photo.SkinRemarkId = skinRemarkId;
            _context.SkinPhotos.Add(photo);
            await _context.SaveChangesAsync();
            return photo;
        }

        public async Task<bool> RemoveSkinPhotoAsync(int photoId)
        {
            var photo = await _context.SkinPhotos.FindAsync(photoId);
            if (photo == null) return false;

            // Delete file
            if (File.Exists(photo.FilePath))
            {
                File.Delete(photo.FilePath);
            }

            _context.SkinPhotos.Remove(photo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DailyLog>> GetLogsInRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.DailyLogs
                .Where(dl => dl.Date.Date >= startDate.Date && dl.Date.Date <= endDate.Date)
                .Include(dl => dl.CompletedRoutines)
                .Include(dl => dl.SkinRemark)
                .OrderByDescending(dl => dl.Date)
                .ToListAsync();
        }
    }
}