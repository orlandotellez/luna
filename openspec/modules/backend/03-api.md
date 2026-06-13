# API Endpoints & Authentication

API REST de LUNA.

## Base URL

```
http://localhost:5000/api/v1
```

---

## Auth Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/auth/register` | No | Registrar nueva usuaria |
| POST | `/auth/login` | No | Iniciar sesión |
| POST | `/auth/refresh` | No (requiere refresh token) | Renovar tokens |
| POST | `/auth/logout` | Sí | Cerrar sesión |
| POST | `/auth/verify-email` | No | Verificar email con token |
| POST | `/auth/resend-verification` | No | Reenviar email de verificación |
| POST | `/auth/forgot-password` | No | Solicitar reseteo de contraseña |
| POST | `/auth/reset-password` | No | Resetear contraseña con token |
| POST | `/auth/biometric` | Sí | Registrar clave biométrica |

### Auth Responses

**Register / Login** — la API **NO devuelve tokens en el body** por seguridad:
```json
{
  "message": "Usuario registrado exitosamente",
  "user": {
    "id": "guid",
    "name": "María García",
    "email": "maria@email.com",
    "lifeStage": "ActiveCycle",
    "language": "es"
  }
}
```

Los tokens se envían únicamente como **HttpOnly cookies**:
- `accessToken` — 15 min de vida
- `refreshToken` — 30 días de vida

### Register

```json
POST /api/v1/auth/register
{
  "name": "María García",
  "email": "maria@email.com",
  "phone": "+525512345678",
  "password": "MiPassword123",
  "lifeStage": "ActiveCycle",
  "language": "es",
  "departmentId": "14",
  "municipalityId": "001"
}
```

Validation:
- Name: required, max 255 chars
- Email: required, valid format, unique
- Password: required, min 8 chars, must contain letter + number
- LifeStage: required, must be valid enum value
- Language: required, must be valid language code

### Login

```json
POST /api/v1/auth/login
{
  "email": "maria@email.com",
  "password": "MiPassword123"
}
```

### Auth Flow

```
1. POST /auth/register → crea usuario + account
2. → Envía email de verificación (SendGrid)
3. POST /auth/login → backend valida credenciales
4. → Crea session en DB
5. → Setea cookies HttpOnly: accessToken (15min) + refreshToken (30d)
6. → Response: { message, user } — SIN tokens en body
7. Frontend: cada request → cookie accessToken se envía automáticamente
8. 401 → frontend llama POST /auth/refresh → renueva cookies
9. POST /auth/logout → revoca session + limpia cookies
```

---

## User Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/users/me` | Sí | Perfil de la usuaria |
| PUT | `/users/me` | Sí | Actualizar perfil |
| PUT | `/users/me/life-stage` | Sí | Cambiar etapa de vida |
| PUT | `/users/me/avatar` | Sí | Subir foto de perfil |
| GET | `/users/me/health-profile` | Sí | Perfil de salud |
| PUT | `/users/me/health-profile` | Sí | Actualizar perfil de salud |
| DELETE | `/users/me/account` | Sí | Eliminar cuenta (soft-delete) |
| GET | `/users/me/reports` | Sí | Obtener reportes de salud |
| POST | `/users/me/export-data` | Sí | Solicitar exportación de datos |

### Life Stage Change

```json
PUT /api/v1/users/me/life-stage
{
  "lifeStage": "Pregnancy",
  "lastMenstrualPeriod": "2026-01-15",
  "estimatedDueDate": "2026-10-22"
}
```

---

## Cycle Endpoints

### Daily Tracking

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/cycle/current` | Sí | Ciclo actual + predicciones |
| POST | `/cycle/period` | Sí | Registrar inicio/fin de período |
| POST | `/cycle/symptoms` | Sí | Registrar síntoma del día |
| GET | `/cycle/calendar?month=6&year=2026` | Sí | Datos del calendario mensual |
| GET | `/cycle/history` | Sí | Historial de ciclos |
| GET | `/cycle/stats` | Sí | Estadísticas y tendencias |
| GET | `/cycle/predictions` | Sí | Predicciones actualizadas |

### Register Period

```json
POST /api/v1/cycle/period
{
  "startDate": "2026-06-10",
  "endDate": "2026-06-14",
  "flowIntensity": "Moderate"
}
```

### Register Symptom

```json
POST /api/v1/cycle/symptoms
{
  "date": "2026-06-12",
  "symptoms": [
    { "type": "Cramps", "intensity": 3 },
    { "type": "Headache", "intensity": 2 },
    { "type": "Bloating", "intensity": 1 }
  ],
  "mood": "Irritable",
  "sleepQuality": 3,
  "notes": "Dolor fuerte por la mañana"
}
```

---

## Pregnancy Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/pregnancy/register` | Sí | Registrar embarazo |
| GET | `/pregnancy/current` | Sí | Datos actuales del embarazo |
| GET | `/pregnancy/week/{weekNumber}` | Sí | Información de semana específica |
| GET | `/pregnancy/weeks` | Sí | Todas las semanas (progreso) |
| POST | `/pregnancy/kicks` | Sí | Registrar patada/ movimiento fetal |
| GET | `/pregnancy/kicks/history` | Sí | Historial de patadas |
| POST | `/pregnancy/weight` | Sí | Registrar peso |
| GET | `/pregnancy/weight/history` | Sí | Historial de peso |
| POST | `/pregnancy/contractions` | Sí | Registrar contracción |
| GET | `/pregnancy/contractions/session` | Sí | Sesión actual de contracciones |
| GET/PUT | `/pregnancy/birth-plan` | Sí | Obtener/actualizar plan de parto |
| GET/POST | `/pregnancy/appointments` | Sí | Listar/crear citas prenatales |
| PUT | `/pregnancy/appointments/{id}` | Sí | Actualizar cita |
| DELETE | `/pregnancy/appointments/{id}` | Sí | Eliminar cita |

### Register Pregnancy

```json
POST /api/v1/pregnancy/register
{
  "lastMenstrualPeriod": "2026-01-15",
  "isFirstPregnancy": true
}
```

Response:
```json
{
  "pregnancyId": "guid",
  "currentWeek": 21,
  "estimatedDueDate": "2026-10-22",
  "trimester": 2,
  "weeksRemaining": 19
}
```

### Register Kick

```json
POST /api/v1/pregnancy/kicks
{
  "startTime": "2026-06-12T09:00:00Z",
  "kicks": [
    { "timestamp": "2026-06-12T09:05:00Z" },
    { "timestamp": "2026-06-12T09:08:00Z" },
    { "timestamp": "2026-06-12T09:12:00Z" }
  ],
  "totalKicks": 12,
  "durationMinutes": 30
}
```

---

## Menopause Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/menopause/register` | Sí | Registrar inicio de etapa menopausia |
| GET | `/menopause/current` | Sí | Datos actuales de menopausia |
| POST | `/menopause/symptoms` | Sí | Registrar lote de síntomas |
| GET | `/menopause/symptoms/history` | Sí | Historial de síntomas |
| GET | `/menopause/symptoms/chart` | Sí | Datos para gráficos de evolución |
| GET | `/menopause/recommendations` | Sí | Recomendaciones personalizadas |
| GET/PUT | `/menopause/bone-health` | Sí | Salud ósea |
| POST | `/menopause/therapy` | Sí | Registrar terapia hormonal |

### Register Menopause Symptoms

```json
POST /api/v1/menopause/symptoms
{
  "date": "2026-06-12",
  "symptoms": [
    { "type": "HotFlash", "intensity": 4, "frequency": 5, "notes": "Comenzaron después del almuerzo" },
    { "type": "NightSweats", "intensity": 3, "frequency": 2 },
    { "type": "Insomnia", "intensity": 2, "sleepHours": 5 }
  ]
}
```

---

## Content Endpoints

### Articles

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/articles` | No | Listar artículos (con filtros) |
| GET | `/articles/{slug}` | No | Detalle de artículo |
| GET | `/articles/featured` | No | Artículos destacados |
| GET | `/articles/recommended` | Sí | Artículos recomendados (según perfil) |
| GET | `/articles/by-stage/{stage}` | No | Artículos por etapa de vida |
| GET | `/articles/search?q=&language=` | No | Buscar artículos |

Query params para `GET /articles`:
- `category` — filtrar por categoría
- `stage` — filtrar por etapa de vida
- `language` — filtrar por idioma
- `search` — búsqueda textual
- `page`, `limit` — paginación

### Myths

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/myths` | No | Listar mitos (paginados) |
| GET | `/myths/{id}` | No | Detalle de mito |
| GET | `/myths/categories` | No | Categorías de mitos |

### Glossary

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/glossary` | No | Listar términos |
| GET | `/glossary/{id}` | No | Detalle de término |
| GET | `/glossary/search?q=` | No | Buscar términos |

---

## Family Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/family/members` | Sí | Listar acompañantes |
| POST | `/family/invite` | Sí | Invitar acompañante |
| DELETE | `/family/members/{id}` | Sí | Eliminar acompañante |
| PUT | `/family/members/{id}` | Sí | Actualizar permiso de acompañante |
| GET | `/family/shared-calendar` | Sí | Calendario compartido |
| GET/POST | `/family/messages` | Sí | Mensajes del foro familiar |
| PUT | `/family/messages/{id}` | Sí | Editar mensaje |

### Invite Family Member

```json
POST /api/v1/family/invite
{
  "name": "Carlos García",
  "email": "carlos@email.com",
  "relationship": "Partner"
}
```

---

## Directory Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/directory/professionals` | No | Buscar profesionales |
| GET | `/directory/professionals/{id}` | No | Perfil de profesional |
| GET | `/directory/centers` | No | Centros de salud |
| GET | `/directory/emergency` | No | Líneas de emergencia |
| POST | `/directory/professionals` | Admin | Registrar profesional |
| PUT | `/directory/professionals/{id}` | Admin | Actualizar profesional |

Query params para `GET /professionals`:
- `specialty` — filtrar por especialidad
- `departmentId` — filtrar por departamento
- `municipalityId` — filtrar por municipio
- `language` — filtrar por idioma
- `latitude`, `longitude`, `radius` — búsqueda por cercanía
- `page`, `limit` — paginación

---

## Community Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/forum/posts` | No | Listar posts (por etapa) |
| GET | `/forum/posts/{id}` | No | Detalle de post |
| POST | `/forum/posts` | Sí | Crear post |
| PUT | `/forum/posts/{id}` | Sí | Editar post (propio) |
| DELETE | `/forum/posts/{id}` | Sí | Eliminar post (propio/admin) |
| GET | `/forum/posts/{id}/comments` | No | Comentarios de un post |
| POST | `/forum/posts/{id}/comments` | Sí | Comentar en post |
| POST | `/forum/comments/{id}/report` | Sí | Reportar comentario |
| POST | `/forum/posts/{id}/react` | Sí | Reaccionar a post |
| GET | `/forum/stories` | No | Historias destacadas |
| POST | `/forum/stories` | Sí | Enviar historia (para revisión) |

Query params para `GET /forum/posts`:
- `stage` — filtrar por etapa
- `search` — búsqueda textual
- `page`, `limit` — paginación

---

## Reminders Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/reminders` | Sí | Listar recordatorios |
| POST | `/reminders` | Sí | Crear recordatorio |
| PUT | `/reminders/{id}` | Sí | Actualizar recordatorio |
| DELETE | `/reminders/{id}` | Sí | Eliminar recordatorio |
| PATCH | `/reminders/{id}/toggle` | Sí | Activar/desactivar |

### Create Reminder

```json
POST /api/v1/reminders
{
  "type": "Pill",
  "title": "Tomar anticonceptivo",
  "frequency": "Daily",
  "time": "08:00",
  "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
  "enabled": true
}
```

---

## Notifications Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/notifications` | Sí | Listar notificaciones |
| GET | `/notifications/unread-count` | Sí | Contador de no leídas |
| PATCH | `/notifications/{id}/read` | Sí | Marcar como leída |
| PATCH | `/notifications/read-all` | Sí | Marcar todas como leídas |
| POST | `/notifications/register-device` | Sí | Registrar token FCM |
| DELETE | `/notifications/unregister-device` | Sí | Eliminar token FCM |

---

## Reports Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/reports/monthly?year=2026&month=6` | Sí | Resumen mensual |
| GET | `/reports/trends?period=6m` | Sí | Tendencias |
| GET | `/reports/export/pdf?type=monthly&month=6&year=2026` | Sí | Exportar PDF |
| GET | `/reports/export/csv?type=cycle&from=2026-01-01&to=2026-06-30` | Sí | Exportar CSV |

---

## Admin Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/admin/users` | Admin | Listar usuarias |
| PUT | `/admin/users/{id}/status` | Admin | Activar/desactivar usuaria |
| GET | `/admin/content/pending` | Admin | Contenido pendiente de revisión |
| PUT | `/admin/content/{id}/approve` | Admin | Aprobar contenido (historias) |
| PUT | `/admin/content/{id}/reject` | Admin | Rechazar contenido |
| GET | `/admin/reports/analytics` | Admin | Dashboard analítico |
| POST | `/admin/articles` | Admin | Crear artículo |
| PUT | `/admin/articles/{id}` | Admin | Actualizar artículo |
| DELETE | `/admin/articles/{id}` | Admin | Eliminar artículo |
| POST | `/admin/myths` | Admin | Crear mito |
| POST | `/admin/glossary` | Admin | Crear término de glosario |
| POST | `/admin/professionals` | Admin | Registrar profesional |
| POST | `/admin/health-centers` | Admin | Registrar centro de salud |
| GET | `/admin/moderation/reports` | Admin | Reportes de moderación |
| PUT | `/admin/moderation/reports/{id}` | Admin | Resolver reporte |

---

## RBAC Permissions

| Permission | Admin | Professional | User | Familiar |
|------------|-------|--------------|------|----------|
| `users:read` | ✅ | ❌ | ❌ | ❌ |
| `users:update` | ✅ | ❌ | ❌ | ❌ |
| `users:delete` | ✅ | ❌ | ❌ | ❌ |
| `content:create` | ✅ | ❌ | ❌ | ❌ |
| `content:update` | ✅ | ❌ | ❌ | ❌ |
| `content:delete` | ✅ | ❌ | ❌ | ❌ |
| `content:approve` | ✅ | ❌ | ❌ | ❌ |
| `directory:manage` | ✅ | ❌ | ❌ | ❌ |
| `moderation:resolve` | ✅ | ❌ | ❌ | ❌ |
| `admin:panel` | ✅ | ❌ | ❌ | ❌ |

## Roles

```csharp
public enum UserRole { User, Familiar, Professional, Admin }
```

Uso en controllers:
```csharp
[RequirePermission(Permissions.ContentCreate)]
public async Task<ActionResult> CreateArticle(...)
```

## JWT Configuration

```json
{
  "Jwt:Secret": "your-super-secret-jwt-key-min-32-chars-long",
  "AccessTokenExpiryMinutes": 15,
  "RefreshTokenExpiryDays": 30
}
```

## Security Notes

- La API **nunca devuelve tokens en el body** de login/register/refresh
- Los tokens se transmiten exclusivamente por cookies HttpOnly, Secure, SameSite=Strict
- El refresh token se revoca en DB al hacer logout (se elimina la session)
- Todas las rutas protegidas requieren el permiso específico via `[RequirePermission]`
- Datos de salud: solo la usuaria puede acceder a sus propios datos de ciclo/embarazo/menopausia
- Acompañantes solo ven datos explícitamente compartidos
