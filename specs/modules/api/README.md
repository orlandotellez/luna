# API Module

Documentación de la API REST de **LUNA** organizada por features. Cada archivo cubre un dominio funcional e incluye (1) la tabla de endpoints del feature y (2) el detalle de cada endpoint: método, path, auth, request body, response, validaciones y errores.

## Base URL

```
http://localhost:5000/api/v1
```

## Convenciones transversales

- **Auth**: `Sí` requiere cookie HttpOnly `accessToken` válida (15 min). `No` es público.
- **Content-Type**: `application/json` salvo indicación (multipart/form-data para subir archivos).
- **Errores**: se devuelven como `ProblemDetails` (RFC 7807) por el `ErrorHandlingMiddleware`.
- **Versionado**: prefijo `/api/v1` obligatorio en todos los endpoints.
- **Roles**: además de autenticado, algunos endpoints requieren rol específico (`Admin`, `Professional`, `Familiar`) via `[RequirePermission]`.

## Índice de features

| # | Feature | Descripción |
|---|---------|-------------|
| [01-auth.md](./01-auth.md) | Autenticación y sesión | Registro, login, refresh, logout, verificación de email, reset de contraseña, biométrica |
| [02-users.md](./02-users.md) | Perfil de usuaria | Datos básicos, perfil de salud, avatar, soft-delete, exportación |
| [03-cycle.md](./03-cycle.md) | Ciclo menstrual | Tracking diario, calendario mensual, historial, estadísticas, predicciones |
| [04-pregnancy.md](./04-pregnancy.md) | Embarazo | Semanas, patadas, peso, contracciones, citas, plan de parto |
| [05-menopause.md](./05-menopause.md) | Menopausia | Síntomas, gráficos, recomendaciones, salud ósea, terapia hormonal |
| [06-content.md](./06-content.md) | Contenido educativo | Artículos, mitos y glosario (con filtros por categoría, etapa e idioma) |
| [07-family.md](./07-family.md) | Acompañantes | Invitar familiares, mensajes privados, calendario compartido |
| [08-directory.md](./08-directory.md) | Directorio de salud | Profesionales, centros y líneas de emergencia |
| [09-community.md](./09-community.md) | Comunidad | Foro público, comentarios, reacciones e historias destacadas |
| [10-reminders.md](./10-reminders.md) | Recordatorios | Anticonceptivo, citas, custom — con frecuencia diaria/semanal/mensual |
| [11-notifications.md](./11-notifications.md) | Notificaciones | In-app y push (Firebase Cloud Messaging / FCM) |
| [12-reports.md](./12-reports.md) | Reportes de salud | Resumen mensual, tendencias, exportación PDF y CSV |
| [13-admin.md](./13-admin.md) | Panel administrativo | Gestión de usuarias, contenido, profesionales, moderación |

## Seguridad transversal

- JWT access token (15 min) + refresh token (30 días) transmitidos **únicamente** como cookies `HttpOnly`, `Secure`, `SameSite=Strict`.
- La API **nunca devuelve tokens en el body** de login/register/refresh.
- Refresh token revocado en DB al hacer logout (se elimina la `sessions`).
- Datos de salud: solo la usuaria puede acceder a sus propios datos de ciclo / embarazo / menopausia.
- Acompañantes (rol `Familiar`) solo ven datos explícitamente compartidos.

## Formato de cada archivo de feature

Todos los archivos de feature siguen esta misma estructura:

1. **Tabla resumen** — todos los endpoints del feature (método, path, auth, descripción).
2. **Detalle por endpoint** — para cada uno:
   - Auth requerida.
   - Request body (JSON) y/o query params.
   - Response (JSON) con código HTTP.
   - Validaciones (reglas de negocio).
   - Errores comunes.

## Convención de paginación

Todos los endpoints que devuelven listas siguen esta forma de respuesta (uniforme en toda la API):

```json
{
  "items": [/* ... */],
  "page": 1,
  "limit": 20,
  "total": 137,
  "hasMore": true
}
```

- `page` parte en `1`.
- `limit` por defecto `20`, máximo `100`.
- `total` siempre viene poblado (no se omite para «ahorrar query»).
- `hasMore` se calcula como `(page * limit) < total`.

Excepciones que no usan este shape: `GET /cycle/calendar` (devuelve `days[]` con fecha exacta sin paginar), `GET /cycle/predictions` (devuelve un solo objeto de forecast), descargas binarias (`/reports/export/pdf`, `/reports/export/csv`).
