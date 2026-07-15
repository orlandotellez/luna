using Luna.Api.Helpers;
using Luna.Application.Features.Auth;
using Luna.Application.Common.Interfaces;
using Luna.Infrastructure.Persistence.Repositories;

namespace Luna.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Helpers
        services.AddScoped<CookieHelper>();
        services.AddScoped<TokenHelper>();
        services.AddScoped<IPasswordService, PasswordHelper>();
        services.AddScoped<ITokenService, TokenHelperService>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
