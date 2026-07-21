using Luna.Api.Helpers;
using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Models.Cycle;
using Luna.Application.Common.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luna.Api.Controllers;

[ApiController]
[Route("api/v1/cycle")]
[Authorize]
public class CycleController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICycleService _cycleService;

    public CycleController(
        IUserService userService,
        ICycleService cycleService
        )
    {
        _userService = userService;
        _cycleService = cycleService;
    }

    [HttpGet("current")]
    public async Task<ActionResult<CycleCurrentDto>> GetCurrentCycle()
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var result = await _cycleService.GetCurrentCycleAsync(userId.Value);

        return Ok(result);
    }

    [HttpPost("period")]
    public async Task<ActionResult<PeriodEntryDto>> RegisterPeriod([FromBody] RegisterPeriodRequest request)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var result = await _cycleService.RegisterPeriodAsync(userId.Value, request);

        return Created($"/api/v1/cycle/period/{result.Id}", result);
    }

    [HttpPost("symptoms")]
    public async Task<ActionResult<SymptomEntryDto>> RegisterSymptom([FromBody] RegisterSymptomRequest request)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var result = await _userService.RegisterSymptomAsync(userId.Value, request);

        return Created($"/api/v1/cycle/symptoms/{result.Id}", result);
    }

    [HttpGet("calendar")]
    public async Task<ActionResult<CycleCalendarDto>> GetCalendar(
    [FromQuery] int month,
    [FromQuery] int year)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var result = await _cycleService.GetCalendarAsync(userId.Value, month, year);

        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<ActionResult<CycleHistoryDto>> GetHistory(
           [FromQuery] int limit = 12)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var result = await _cycleService.GetHistoryAsync(userId.Value, limit);

        return Ok(result);
    }

    [HttpGet("stats")]
    public async Task<ActionResult<CycleStatsDto>> GetStats()
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var result = await _cycleService.GetStatsAsync(userId.Value);

        return Ok(result);
    }
}
