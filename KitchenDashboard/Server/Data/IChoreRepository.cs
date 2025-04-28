using KitchenDashboard.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitchenDashboard.Server.Data
{
    public interface IChoreRepository
    {
        Task<IEnumerable<RecurringChore>> GetTodaysRecurringAsync(DayOfWeek day);
        Task<IEnumerable<OneOffChore>> GetTodaysOneOffAsync(DateTime date);
        Task AddRecurringChoreAsync(RecurringChore chore);
        Task AddOneOffChoreAsync(OneOffChore chore);
        Task MarkRecurringCompletedAsync(Guid id, DateTime date);
        Task MarkOneOffCompletedAsync(Guid id);
    }
}
