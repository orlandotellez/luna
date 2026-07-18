using Luna.Domain.Enums;

namespace Luna.Application.Common.Models.User;

public record CreateUserRequest(
    string Name,
    string Email,
    string Password,
    UserRole Role,
    string? Phone = null
);

public record UpdateUserRequest(
    string? Name,
    string? Email,
    UserRole? Role,
    string? Phone,
    string? Bio,
    string? UserName,
    bool? IsActive
);

public record UserFilter(
    string? Search,
    UserRole? Role,
    bool? IsActive,
    bool? IncludeDeleted
);

public record UpdateUserProfileRequest(
    string? Name,
    string? Phone,
    string? Bio,
    string? UserName
);

public record UpdateLifeStageRequest(
    LifeStage LifeStage,
    DateOnly? LastMenstrualPeriod,
    DateOnly? EstimatedDueDate
);

public record UpdateHealthProfileRequest(
    bool? HasRegularCycle,
    int? CycleLengthDays,
    int? PeriodLengthDays,
    bool? HasEndometriosis,
    bool? HasPcos,
    bool? HasThyroidIssues,
    bool? HasGestationalDiabetes,
    bool? HasFibroids,
    bool? HasHypertension,
    string? Allergies,
    List<String>? Medications,
    int? PreviousPregnancies,
    string? Surgeries,
    List<string>? Vaccinations
);
