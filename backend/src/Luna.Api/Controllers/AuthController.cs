using Luna.Application.Common.Interfaces;
using Luna.Application.Common.Models;
using Luna.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Luna.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly CookieHelper _cookieHelper;

    public AuthController(
        IAuthService authService,
        CookieHelper cookieHelper
        )
    {
        _authService = authService;
        _cookieHelper = cookieHelper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        var currentUserId = HttpContext.GetCurrentUserId();

        if (currentUserId.HasValue)
        {
            return Conflict("Already loggend in. Please logout before creating a new account.");
        }

        var result = await _authService.RegisterAsync(request);

        _cookieHelper.SetAuthCookies(result.AccessToken, result.RefreshToken);

        return CreatedAtAction(null, new
        {
            message = result.Message,
            user = result.User
        });
    }
}
