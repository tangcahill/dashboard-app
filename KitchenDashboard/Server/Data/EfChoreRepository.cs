using Microsoft.EntityFrameworkCore;
using KitchenDashboard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenDashboard.Server.Data
{
    public class EfChoreRepository : IChoreRepository
    {
        private readonly ChoresDbContext _db;
        public EfChoreRepository(ChoresDbContext db) => _db = db;

        public async Task<IEnumerable<RecurringChore>> GetTodaysRecurringAsync(DayOfWeek day)
        {
            var today = DateTime.Now.Date;
            return await _db.RecurringChores
                .Where(r => r.Active
                         && ((day == DayOfWeek.Monday && r.Monday)
                          || (day == DayOfWeek.Tuesday && r.Tuesday)
                          || (day == DayOfWeek.Wednesday && r.Wednesday)
                          || (day == DayOfWeek.Thursday && r.Thursday)
                          || (day == DayOfWeek.Friday && r.Friday)
                          || (day == DayOfWeek.Saturday && r.Saturday)
                          || (day == DayOfWeek.Sunday && r.Sunday)))
                .ToListAsync();
        }

        public async Task<IEnumerable<OneOffChore>> GetTodaysOneOffAsync(DateTime date)
        {
            return await _db.OneOffChores
                .Where(o => o.Date.HasValue
                         && o.Date.Value.Date == date.Date
                         && !o.Completed)
                .ToListAsync();
        }

        public async Task AddRecurringChoreAsync(RecurringChore chore)
        {
            // SERVER-SIDE ENFORCE
            chore.Id = Guid.NewGuid();
            chore.Active = true;

            _db.RecurringChores.Add(chore);
            await _db.SaveChangesAsync();
        }

        public async Task AddOneOffChoreAsync(OneOffChore chore)
        {
            chore.Id = Guid.NewGuid();
            chore.Completed = false;

            _db.OneOffChores.Add(chore);
            await _db.SaveChangesAsync();
        }

        public async Task MarkRecurringCompletedAsync(Guid id, DateTime date)
        {
            // example: just deactivate
            var chore = await _db.RecurringChores.FindAsync(id);
            if (chore != null)
            {
                chore.Active = false;
                await _db.SaveChangesAsync();
            }
        }

        public async Task MarkOneOffCompletedAsync(Guid id)
        {
            var chore = await _db.OneOffChores.FindAsync(id);
            if (chore != null)
            {
                chore.Completed = true;
                await _db.SaveChangesAsync();
            }
        }
    }
}
