# Backend Stack

Stack tecnológico del backend — LUNA API.

## Core Technologies

- **.NET 10 SDK** (net10.0)
- **C# 13**
- **ASP.NET Core 10** Web API
- **PostgreSQL 16+**
- **Entity Framework Core 10**

## Architecture

- **Clean Architecture** (Domain / Application / Infrastructure / API layers)
- **Service Layer pattern** in Application layer (no CQRS / MediatR)
- **Repository Pattern** with EF Core
- **Global Exception Middleware** with ProblemDetails (RFC 7807)

## Dependencies

### Api.csproj
```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.8" />
<PackageReference Include="Serilog.AspNetCore" Version="10.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.0.0" />
<PackageReference Include="FirebaseAdmin" Version="3.0.0" />
```

### Infrastructure.csproj
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.0.0" />
<PackageReference Include="SendGrid" Version="9.29.0" />       <!-- Email service -->
<PackageReference Include="AWSSDK.S3" Version="3.7.0" />       <!-- File storage (media) -->
```

### Application.csproj
```xml
<ProjectReference Include="../Domain/Domain.csproj" />
```

### Domain.csproj
- No external dependencies — pure C# POCOs and enums.

## Key Patterns

| Pattern | Implementation |
|---------|---------------|
| **Service Layer** | `IAuthService` / `AuthService`, `ICycleService` / `CycleService` |
| **Repository** | `ICycleRepository` / `CycleRepository`, etc. |
| **Mapping** | Manual extension methods (`MappingCycle.cs`, `MappingUser.cs`) |
| **Validation** | FluentValidation (validación de requests) |
| **Error handling** | `AppException` → `ErrorHandlingMiddleware` → ProblemDetails |
| **Auth** | JWT in HttpOnly cookies, RBAC with PermissionHandler |

## Infrastructure Services

- **PostgreSQL** — primary database via EF Core
- **BCrypt** — password hashing
- **JWT** — authentication tokens (stored in HttpOnly cookies)
- **Serilog** — structured logging to console + file
- **Firebase Admin SDK** — push notifications
- **SendGrid** — transactional emails (verification, password reset, reminders)
- **AWS S3 / Local Storage** — file storage for media (avatars, article images, PDFs)
- **OpenAPI** — `/swagger` in development mode

## Notable Absences (vs. typical .NET template)

| Feature | Status |
|---------|--------|
| **CQRS / MediatR** | Not used — direct service injection instead |
| **AutoMapper** | Not used — manual extension methods |
| **Hangfire** | Not installed |
| **Redis** | Not installed |
| **SignalR** | Not installed |
| **Testing** | No test projects yet |
