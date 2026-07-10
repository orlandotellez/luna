namespace Luna.Application.Common.Models;

public record UserFilter(string? Searh, string? Role, bool? IsActive, bool? IncludeDeleted);
