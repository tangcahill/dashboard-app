using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/device/register")]
public class DeviceController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] string token)
    {
        TokenStore.CurrentToken = token;
        return Ok();
    }
}
