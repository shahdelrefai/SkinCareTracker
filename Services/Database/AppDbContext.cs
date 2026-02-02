using Microsoft.EntityFrameworkCore;
using SkinCareTracker.Models;

namespace SkinCareTracker.Services.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<RoutineStep> RoutineSteps { get; set; }
        public DbSet<RoutineSchedule> RoutineSchedules { get; set; }
        public DbSet<DailyLog> DailyLogs { get; set; }
        public DbSet<RoutineCompletion> RoutineCompletions { get; set; }
        public DbSet<FoodLog> FoodLogs { get; set; }
        public DbSet<SkinRemark> SkinRemarks { get; set; }
        public DbSet<SkinPhoto> SkinPhotos { get; set; }
        public DbSet<PeriodCycle> PeriodCycles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product relationships
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductIngredients)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Routine relationships
            modelBuilder.Entity<Routine>()
                .HasMany(r => r.Steps)
                .WithOne(rs => rs.Routine)
                .HasForeignKey(rs => rs.RoutineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Routine>()
                .HasOne(r => r.Schedule)
                .WithOne(rs => rs.Routine)
                .HasForeignKey<RoutineSchedule>(rs => rs.RoutineId)
                .OnDelete(DeleteBehavior.Cascade);

            // DailyLog relationships
            modelBuilder.Entity<DailyLog>()
                .HasMany(dl => dl.CompletedRoutines)
                .WithOne(rc => rc.DailyLog)
                .HasForeignKey(rc => rc.DailyLogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DailyLog>()
                .HasMany(dl => dl.FoodLogs)
                .WithOne(fl => fl.DailyLog)
                .HasForeignKey(fl => fl.DailyLogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DailyLog>()
                .HasOne(dl => dl.SkinRemark)
                .WithOne(sr => sr.DailyLog)
                .HasForeignKey<SkinRemark>(sr => sr.DailyLogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SkinRemark>()
                .HasMany(sr => sr.Photos)
                .WithOne(sp => sp.SkinRemark)
                .HasForeignKey(sp => sp.SkinRemarkId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore non-mapped properties on SkinPhoto
            modelBuilder.Entity<SkinPhoto>()
                .Ignore(sp => sp.FullFilePath)
                .Ignore(sp => sp.PhotoSource);

            // Indexes
            modelBuilder.Entity<DailyLog>()
                .HasIndex(dl => dl.Date)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.IsActive);

            modelBuilder.Entity<Routine>()
                .HasIndex(r => r.IsArchived);
        }
    }
}