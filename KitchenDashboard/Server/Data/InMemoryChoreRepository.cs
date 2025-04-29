using KitchenDashboard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenDashboard.Server.Data
{
    public class InMemoryChoreRepository : IChoreRepository
    {
        private readonly List<RecurringChore> _recurring = new();
        private readonly List<OneOffChore> _oneOff = new();
        private readonly HashSet<(Guid, DateTime)> _completedRecurring = new();

        public InMemoryChoreRepository()
        {
            // seed some examples
            _recurring.Add(new RecurringChore
            {
                Id = Guid.NewGuid(),
                Description = "Wash dishes",
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Active = true
            });
            _recurring.Add(new RecurringChore
            {
                Id = Guid.NewGuid(),
                Description = "Take out trash",
                Tuesday = true,
                Thursday = true,
                Active = true
            });
        }

        public Task<IEnumerable<RecurringChore>> GetTodaysRecurringAsync(DayOfWeek day)
        {
            // use local today
            var today = DateTime.Now.Date;
            var list = _recurring
                .Where(r => r.Active
                         && !_completedRecurring.Contains((r.Id, today)))
                .Where(r => day switch {
                    DayOfWeek.Monday => r.Monday,
                    DayOfWeek.Tuesday => r.Tuesday,
                    DayOfWeek.Wednesday => r.Wednesday,
                    DayOfWeek.Thursday => r.Thursday,
                    DayOfWeek.Friday => r.Friday,
                    DayOfWeek.Saturday => r.Saturday,
                    DayOfWeek.Sunday => r.Sunday,
                    _ => false
                });
            return Task.FromResult(list);
        }

        public Task<IEnumerable<OneOffChore>> GetTodaysOneOffAsync(DateTime date)
        {
            var target = date.Date;
            var list = _oneOff
                .Where(o =>
                    o.Date.HasValue &&               // ensure it's not null
                    o.Date.Value.Date == target &&   // compare the actual DateTime
                    !o.Completed);
            return Task.FromResult(list);
        }


        public Task AddRecurringChoreAsync(RecurringChore chore)
        {
            chore.Id = Guid.NewGuid();
            chore.Active = true;
            _recurring.Add(chore);
            return Task.CompletedTask;
        }

        public Task AddOneOffChoreAsync(OneOffChore chore)
        {
            chore.Id = Guid.NewGuid();
            chore.Completed = false;
            _oneOff.Add(chore);
            return Task.CompletedTask;
        }

        public Task MarkRecurringCompletedAsync(Guid id, DateTime date)
        {
            _completedRecurring.Add((id, date.Date));
            return Task.CompletedTask;
        }

        public Task MarkOneOffCompletedAsync(Guid id)
        {
            var item = _oneOff.FirstOrDefault(o => o.Id == id);
            if (item != null) item.Completed = true;
            return Task.CompletedTask;
        }

        public Task UpdateRecurringChoreAsync(RecurringChore chore)
        {
            var existing = _recurring.FirstOrDefault(c => c.Id == chore.Id);
            if (existing != null)
            {
                existing.Description = chore.Description;
                existing.Monday = chore.Monday;
                existing.Tuesday = chore.Tuesday;
                existing.Wednesday = chore.Wednesday;
                existing.Thursday = chore.Thursday;
                existing.Friday = chore.Friday;
                existing.Saturday = chore.Saturday;
                existing.Sunday = chore.Sunday;
            }
            return Task.CompletedTask;
        }

        public Task DeleteRecurringChoreAsync(Guid id)
        {
            _recurring.RemoveAll(c => c.Id == id);
            return Task.CompletedTask;
        }

        public Task UpdateOneOffChoreAsync(OneOffChore chore)
        {
            var existing = _oneOff.FirstOrDefault(o => o.Id == chore.Id);
            if (existing != null)
            {
                existing.Description = chore.Description;
                existing.Date = chore.Date;
            }
            return Task.CompletedTask;
        }

        public Task DeleteOneOffChoreAsync(Guid id)
        {
            _oneOff.RemoveAll(o => o.Id == id);
            return Task.CompletedTask;
        }
    }
}
