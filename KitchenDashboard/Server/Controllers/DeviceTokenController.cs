using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/device/token")]
public class DeviceTokenController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(TokenStore.CurrentToken ?? "none");
}
