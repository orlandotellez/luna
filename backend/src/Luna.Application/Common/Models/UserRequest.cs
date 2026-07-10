namespace Luna.Application.Common.Models;

public record CreateUserRequest(string Name, string Email, string Password, string Role, string? Phone = null);
public record UpdateUserRequest(string? Name, string? Email, string? Role, string? Phone, string? Bio, string? UserName, bool? IsActive);
public record UserFilter(string? Searh, string? Role, bool? IsActive, bool? IncludeDeleted);
