using Microsoft.AspNetCore.Mvc;
namespace KitchenDashboard.Server.Controllers
{
    [ApiController]
    [Route("api/alarms/push")]
    public class AlarmsPushController : ControllerBase
    {
        private readonly AlarmSender _sender;
        public AlarmsPushController(AlarmSender sender)
            => _sender = sender;

        public record AlarmDto(DateTime Due, string Message);

        [HttpPost]   // ← this is required so POST /api/alarms/push binds here
        public async Task<IActionResult> Post([FromBody] AlarmDto dto)
        {
            var token = TokenStore.CurrentToken;
            if (string.IsNullOrEmpty(token))
                return BadRequest("No device registered");

            await _sender.SendAlarmAsync(token, dto.Due, dto.Message);
            return Ok();
        }
    }
}
