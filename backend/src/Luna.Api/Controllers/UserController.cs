using Luna.Api.Helpers;
using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luna.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMyProfile()
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var profile = await _userService.GetMyProfileAsync(userId.Value);

        return Ok(profile);
    }

    [HttpPut("me")]
    public async Task<ActionResult<UserDto>> UpdateMyProfil([FromBody] UpdateUserProfileRequest request)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null) return Unauthorized(new { error = "Invalid token" });

        var profile = await _userService.UpdateUserProfileAsync(userId.Value, request);

        return Ok(profile);
    }
}
