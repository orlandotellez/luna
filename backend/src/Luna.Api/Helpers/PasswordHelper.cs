using Luna.Application.Common.Interfaces;

namespace Luna.Api.Helpers;

public class PasswordHelper : IPasswordService
{
    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string hash)
        => BCrypt.Net.BCrypt.Verify(password, hash);
}
