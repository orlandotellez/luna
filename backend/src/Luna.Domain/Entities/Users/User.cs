using Luna.Domain.Enums;
using Luna.Domain.Entities.Cycle;

namespace Luna.Domain.Entities.Users;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public string? Phone { get; set; }
    public string? Image { get; set; }
    public UserRole Role { get; set; }
    public LifeStage? LifeStage { get; set; }
    public HealthProfile? HealthProfile { get; set; }  // 1:1
    public ICollection<Pregnancy> Pregnancies { get; set; } = new List<Pregnancy>();  // 1:N
    public bool IsActive { get; set; }
    public DateTime? LastSeenAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; } // soft Delete
    public Guid? DeletedByUserId { get; set; } // Quien eliminó
    public string? DeletedByName { get; set; } // Nombre de quien eliminó 
    public int FailedLoginAttempts { get; set; } // Intentos fallidos 
    public DateTime? LockoutEnd { get; set; } // Fin del bloqueo 

    public UserProfile? Profile { get; set; }
}
