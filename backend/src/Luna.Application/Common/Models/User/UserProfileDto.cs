namespace Luna.Application.Common.Models.User;

public record UserProfileDto
{
    public string? UserName { get; set; }
    public string? Bio { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? DepartmentId { get; set; }
    public string? MunicipalityId { get; set; }
}
