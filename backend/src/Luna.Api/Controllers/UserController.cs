using Luna.Api.Helpers;
using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luna.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IFileStorageService _fileStorageService;

    public UsersController(
        IUserService userService,
IFileStorageService fileStorageService
        )
    {
        _userService = userService;
        _fileStorageService = fileStorageService;
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

    [HttpPut("me/avatar")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB máximo
    public async Task<ActionResult<UserDto>> UpdateAvatar(IFormFile file, CancellationToken ct)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file provided or file is empty." });

        // Validate extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
    };

        if (!allowedExtensions.Contains(extension))
            return BadRequest(new { error = $"File type '{extension}' is not supported. Allowed: jpg, jpeg, png, gif, webp." });

        if (file.Length > 10 * 1024 * 1024)
            return BadRequest(new { error = "File exceeds maximum size of 10 MB." });

        await using var stream = file.OpenReadStream();
        var imageUrl = await _fileStorageService.UploadImageAsync(stream, file.FileName, ct);

        var result = await _userService.UpdateAvatarAsync(userId.Value, imageUrl);

        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<ActionResult<UserDto>> UpdateMyProfil([FromBody] UpdateUserProfileRequest request)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null) return Unauthorized(new { error = "Invalid token" });

        var profile = await _userService.UpdateUserProfileAsync(userId.Value, request);

        return Ok(profile);
    }

    [HttpPut("me/life-stage")]
    public async Task<ActionResult<UserDto>> UpdateLifeStage([FromBody] UpdateLifeStageRequest request)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null) return Unauthorized(new { error = "Invalid token" });

        var result = await _userService.UpdateLifeStageAsync(userId.Value, request);

        return Ok(result);
    }

    [HttpGet("me/health-profile")]
    public async Task<ActionResult<HealthProfileDto>> GetHealthProfile()
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var healthProfile = await _userService.GetHealthProfileAsync(userId.Value);

        if (healthProfile is null)
            return Ok(new HealthProfileDto()); // Devuelve un DTO vacío en lugar de null

        return Ok(healthProfile);
    }

    [HttpPut("me/health-profile")]
    public async Task<ActionResult<HealthProfileDto>> UpdateHealthProfile([FromBody] UpdateHealthProfileRequest request)
    {
        var userId = HttpContext.GetCurrentUserId();

        if (userId is null)
            return Unauthorized(new { error = "Invalid token" });

        var healthProfile = await _userService.UpdateHealthProfileAsync(userId.Value, request);

        return Ok(healthProfile);
    }
}
