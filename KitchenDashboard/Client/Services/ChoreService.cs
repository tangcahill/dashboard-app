using KitchenDashboard.Shared.Models;
using System.Net.Http.Json;

namespace KitchenDashboard.Client.Services
{
    public class ChoreService
    {
        private readonly HttpClient _http;
        public List<RecurringChore> RecurringChores { get; private set; } = new();
        public List<OneOffChore> OneOffChores { get; private set; } = new();

        public ChoreService(HttpClient http) => _http = http;

        public async Task LoadTodayAsync()
        {
            var dto = await _http.GetFromJsonAsync<TodayDto>("api/chores/today");
            RecurringChores = dto.Recurring.ToList();
            OneOffChores = dto.OneOff.ToList();
        }

        public async Task CompleteRecurringAsync(Guid id)
        {
            await _http.PostAsync($"api/chores/recurring/{id}/complete", null);
            RecurringChores.RemoveAll(c => c.Id == id);
        }

        public async Task CompleteOneOffAsync(Guid id)
        {
            await _http.PostAsync($"api/chores/oneoff/{id}/complete", null);
            OneOffChores.RemoveAll(c => c.Id == id);
        }

        public Task DeleteRecurringAsync(Guid id)
    => _http.DeleteAsync($"api/chores/recurring/{id}");

        public Task DeleteOneOffAsync(Guid id)
            => _http.DeleteAsync($"api/chores/oneoff/{id}");

        public Task AddRecurringAsync(RecurringChore chore)
            => _http.PostAsJsonAsync("api/chores/recurring", chore);
        public Task AddOneOffAsync(OneOffChore chore)
            => _http.PostAsJsonAsync("api/chores/oneoff", chore);
        private record TodayDto(IEnumerable<RecurringChore> Recurring, IEnumerable<OneOffChore> OneOff);
    }
}
