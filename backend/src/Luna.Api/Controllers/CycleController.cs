using Luna.Api.Helpers;
using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Models.Cycle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luna.Api.Controllers;

[ApiController]
[Route("api/v1/cycle")]
[Authorize]
public class CycleController : ControllerBase
{
    private readonly IUserService _userService;

    public CycleController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("current")]
    public async Task<ActionResult<CycleCurrentDto>> GetCurrentCycle()
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var result = await _userService.GetCurrentCycleAsync(userId.Value);

        return Ok(result);
    }
}
