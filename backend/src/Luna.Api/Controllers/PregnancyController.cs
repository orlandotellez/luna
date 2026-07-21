using Luna.Api.Helpers;
using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Models.Pregnancy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luna.Api.Controllers;

[ApiController]
[Route("api/v1/pregnancy")]
[Authorize]
public class PregnancyController : ControllerBase
{
    private readonly IPregnancyService _pregnancyService;

    public PregnancyController(IPregnancyService pregnancyService)
    {
        _pregnancyService = pregnancyService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<PregnancyRegistrationResponseDto>> Register(
        [FromBody] RegisterPregnancyRequest request)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var result = await _pregnancyService.RegisterPregnancyAsync(userId.Value, request);

        return Created("/api/v1/pregnancy/current", result);
    }

    [HttpGet("current")]
    public async Task<ActionResult<CurrentPregnancyResponseDto>> GetCurrent()
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var result = await _pregnancyService.GetCurrentPregnancyAsync(userId.Value);

        return Ok(result);
    }
}
