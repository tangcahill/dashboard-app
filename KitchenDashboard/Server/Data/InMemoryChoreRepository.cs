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
    }
}
