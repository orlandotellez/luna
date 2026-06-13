# Security

Medidas de seguridad implementadas en el backend de LUNA. Dado que la aplicación maneja **datos sensibles de salud**, las medidas de seguridad son críticas.

---

## Implemented

### Authentication
- **JWT tokens** stored exclusively in **HttpOnly cookies** (inaccesibles desde JavaScript)
- **SameSite=Strict** en todas las cookies
- **Secure** flag en producción (requiere HTTPS)
- **Refresh token rotation**: cada refresh genera un nuevo par de tokens y revoca el anterior
- **Session revocation**: al hacer logout, se elimina la session de la base de datos
- **Clock skew = Zero**: los tokens expiran exactamente cuando corresponde
- **Biometric auth**: soporte para autenticación biométrica (huella / Face ID)

### Cookie Configuration
```csharp
// HttpOnly → inaccesible desde JS
// SameSite.Strict → solo se envía en solicitudes del mismo sitio
// Secure → solo por HTTPS (en producción)
// Expires → tiempo de vida limitado (15 min / 30 días)
```

### Authorization (RBAC)
- **Permission-based** authorization via `[RequirePermission]` attribute
- Centralized `Permissions` catalog and `RolePermissions` mapping
- Custom `PermissionHandler` + `PermissionRequirement` implementando `IAuthorizationHandler`

### Input Validation
- **FluentValidation** configured for all request DTOs
- Validation errors return structured ProblemDetails responses
- Sanitización de HTML en contenido de foros (XSS prevention)

### Error Handling
- **Global exception middleware** catches all unhandled exceptions
- Returns RFC 7807 **ProblemDetails** responses
- No stack traces leaked to production
- Custom `AppException` with typed status codes (400, 401, 403, 404, 409, 422)

### Health Data Privacy (GDPR-style)
- Datos de ciclo/embarazo/menopausia son **propiedad de la usuaria**
- Acceso de acompañantes es **explícitamente autorizado** y **revocable**
- Los acompañantes solo ven datos compartidos (citas, calendario), NUNCA datos médicos detallados
- Auditoría de acceso a datos sensibles
- Derecho al olvido: eliminación completa de datos al cerrar cuenta

### Database
- **No raw SQL**: all queries via EF Core parameterized LINQ (SQL injection prevention)
- **Soft-delete** pattern: `DeletedAt` timestamp instead of physical DELETE
- **Snake_case** column naming consistent
- UUID primary keys (no sequential IDs que expongan cantidad de usuarios)

### CORS
```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:8081")  // React Native dev
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
```

### Rate Limiting
```csharp
// Protección de endpoints auth contra brute force
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Auth", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("Api", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
    });
});
```

---

## Not Yet Implemented (Roadmap)

| Feature | Priority | Descripción |
|---------|----------|-------------|
| **Email verification** | High | Obligatorio para acceder a funcionalidades de salud |
| **Password recovery** | High | Forgot/reset password flow |
| **Audit logs** | Medium | Registrar acceso a datos de salud |
| **Security headers** | Medium | X-Content-Type-Options, X-Frame-Options, HSTS |
| **CSRF protection** | Low | Mitigado parcialmente por cookies HttpOnly |
| **Encryption at rest** | Medium | Encriptar datos de salud sensibles en DB |
| **2FA** | Medium | Autenticación de dos factores |
