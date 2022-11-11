using Microsoft.AspNetCore.Mvc;

namespace Play.Underwriting.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UnderwritingController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
}
