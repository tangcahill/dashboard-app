using KitchenDashboard.Shared.Models;
using KitchenDashboard.Server.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KitchenDashboard.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChoresController : ControllerBase
    {
        private readonly IChoreRepository _repo;
        public ChoresController(IChoreRepository repo) => _repo = repo;

        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            // use local today
            var today = DateTime.Now.Date;
            var rec = await _repo.GetTodaysRecurringAsync(today.DayOfWeek);
            var one = await _repo.GetTodaysOneOffAsync(today);
            return Ok(new { Recurring = rec, OneOff = one });
        }

        [HttpPost("recurring")]
        public async Task<IActionResult> AddRecurring([FromBody] RecurringChore chore)
        {
            await _repo.AddRecurringChoreAsync(chore);
            return Created("", null);
        }

        [HttpPost("oneoff")]
        public async Task<IActionResult> AddOneOff([FromBody] OneOffChore chore)
        {
            await _repo.AddOneOffChoreAsync(chore);
            return Created("", null);
        }

        [HttpPost("recurring/{id}/complete")]
        public async Task<IActionResult> CompleteRecurring(Guid id)
        {
            // also use local date here
            await _repo.MarkRecurringCompletedAsync(id, DateTime.Now.Date);
            return NoContent();
        }

        [HttpPost("oneoff/{id}/complete")]
        public async Task<IActionResult> CompleteOneOff(Guid id)
        {
            await _repo.MarkOneOffCompletedAsync(id);
            return NoContent();
        }
    }
}
