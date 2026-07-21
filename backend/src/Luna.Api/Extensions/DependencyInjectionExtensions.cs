using Luna.Api.Helpers;
using Luna.Application.Features.Auth;
using Luna.Application.Features.Users;
using Luna.Application.Features.Cycle;
using Luna.Application.Common.Interfaces.Repositories.Auth;
using Luna.Application.Common.Interfaces.Repositories.Users;
using Luna.Application.Common.Interfaces.Repositories.Cycle;
using Luna.Application.Common.Interfaces.Services;
using Luna.Infrastructure.Persistence.Repositories.Users;
using Luna.Infrastructure.Persistence.Repositories.Auth;
using Luna.Infrastructure.Persistence.Repositories.Cycle;
using Luna.Infrastructure.Services;
using Luna.Infrastructure.Adapters.Cloud;

namespace Luna.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Helpers
        services.AddScoped<CookieHelper>();

        // Services Application
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IPregnancyRepository, PregnancyRepository>();
        services.AddScoped<IHealthProfileRepository, HealthProfileRepository>();
        services.AddScoped<IPeriodEntryRepository, PeriodEntryRepository>();
        services.AddScoped<ISymptomEntryRepository, SymptomEntryRepository>();
        services.AddScoped<ICycleService, CycleService>();

        // Services Infrastructure
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.Configure<R2Options>(configuration.GetSection(R2Options.SectionName));
        services.AddScoped<IFileStorageService, R2StorageService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
