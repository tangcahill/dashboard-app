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

        public Task AddRecurringAsync(RecurringChore chore)
            => _http.PostAsJsonAsync("api/chores/recurring", chore);
        public Task AddOneOffAsync(OneOffChore chore)
            => _http.PostAsJsonAsync("api/chores/oneoff", chore);

        public async Task DeleteRecurringAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/chores/recurring/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteOneOffAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/chores/oneoff/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateRecurringAsync(RecurringChore chore)
            => await _http.PutAsJsonAsync($"api/chores/recurring/{chore.Id}", chore);

        public async Task UpdateOneOffAsync(OneOffChore chore)
            => await _http.PutAsJsonAsync($"api/chores/oneoff/{chore.Id}", chore);


        private record TodayDto(IEnumerable<RecurringChore> Recurring, IEnumerable<OneOffChore> OneOff);
    }
}