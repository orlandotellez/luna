using Luna.Domain.Enums;

namespace Luna.Application.Common.Models;

public record UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public string? Phone { get; set; }
    public string? Image { get; set; }
    public UserRole Role { get; set; }
    public LifeStage? LifeStage { get; set; }
    public PregnancyDto? ActivePregnancy { get; set; }
    public HealthProfileDto? HealthProfile { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastSeenAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
    public string? DeletedByName { get; set; }

    public UserProfileDto? Profile { get; set; }
}
