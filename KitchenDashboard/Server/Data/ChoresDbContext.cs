using KitchenDashboard.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace KitchenDashboard.Server.Data
{
    public class ChoresDbContext : DbContext
    {
        public ChoresDbContext(DbContextOptions<ChoresDbContext> options)
            : base(options)
        {
        }

        public DbSet<RecurringChore> RecurringChores { get; set; }
        public DbSet<OneOffChore> OneOffChores { get; set; }
        public DbSet<CompletedRecurring> CompletedRecurring { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompletedRecurring>()
                        .HasKey(c => new { c.ChoreId, c.Date });
        }
    }

    public class CompletedRecurring
    {
        public Guid ChoreId { get; set; }
        public DateTime Date { get; set; }
    }
}