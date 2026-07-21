# 13 · Admin — Panel administrativo y moderación

Endpoints exclusivos del rol `Admin` para backoffice: gestión de usuarias, aprobación de contenido, profesionales/centros y resolución de reportes.

> Todas las rutas requieren `RequirePermission(...)` específica. La autenticación base es `[Authorize]` a nivel de controlador.

## Tabla de endpoints

### Users

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/admin/users` | Admin | Listar usuarias con filtros |
| PUT | `/admin/users/{id}/status` | Admin | Activar / desactivar cuenta |

### Content Moderation

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/admin/content/pending` | Admin | Contenido pendiente de revisión |
| PUT | `/admin/content/{id}/approve` | Admin | Aprobar contenido (story) |
| PUT | `/admin/content/{id}/reject` | Admin | Rechazar contenido |

### Articles

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/admin/articles` | Admin | Crear artículo |
| PUT | `/admin/articles/{id}` | Admin | Actualizar artículo |
| DELETE | `/admin/articles/{id}` | Admin | Eliminar artículo |

### Content (myths, glossary, professionals, centers)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/admin/myths` | Admin | Crear mito |
| POST | `/admin/glossary` | Admin | Crear término de glosario |
| POST | `/admin/professionals` | Admin | Registrar profesional (idéntico a `POST /directory/professionals`) |
| POST | `/admin/health-centers` | Admin | Registrar centro de salud |

### Moderation Reports

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/admin/moderation/reports` | Admin | Reportes abiertos |
| PUT | `/admin/moderation/reports/{id}` | Admin | Resolver reporte |

### Analytics

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/admin/reports/analytics` | Admin | Dashboard analítico |

---

## Detalle de endpoints

### GET `/api/v1/admin/users`

- **Auth**: Admin

**Query params**: `?role=USER&lifeStage=ActiveCycle&isActive=true&search=maria&page=1&limit=20`.

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "name": "María García",
      "email": "maria@email.com",
      "role": "USER",
      "lifeStage": "ActiveCycle",
      "isActive": true,
      "lastSeenAt": "2026-07-20T18:45:00Z",
      "createdAt": "2026-01-10T08:00:00Z"
    }
  ],
  "page": 1,
  "limit": 20,
  "total": 4711
}
```

---

### PUT `/api/v1/admin/users/{id}/status`

- **Auth**: Admin

**Request body**

```json
{
  "isActive": false,
  "reason": "Cuenta reportada por spam"
}
```

**Response 200 OK** — `UserDto` actualizado.

**Side effects** — si `isActive = false`, revoca todas las `sessions` activas de la usuaria.

---

### GET `/api/v1/admin/content/pending`

- **Auth**: Admin

**Query params**: `?type=story | article | myth | glossary`.

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "type": "story",
      "title": "Mi vientre de luz",
      "submittedBy": "guid",
      "submittedByName": "Lucía R.",
      "submittedAt": "2026-07-10T08:00:00Z"
    }
  ]
}
```

---

### PUT `/api/v1/admin/content/{id}/approve`

- **Auth**: Admin

**Request body** (opcional)

```json
{
  "publishAt": "2026-07-21T09:00:00Z",
  "editorNotes": "Buen contenido, aprobada sin cambios"
}
```

**Response 200 OK**

```json
{
  "id": "guid",
  "status": "approved",
  "publishedAt": "2026-07-21T09:00:00Z"
}
```

---

### PUT `/api/v1/admin/content/{id}/reject`

- **Auth**: Admin

**Request body**

```json
{
  "reason": "duplicate | misinformation | inappropriate | off-topic | other",
  "feedback": "El contenido se envía a una sección que no aplica"
}
```

**Response 200 OK**

```json
{
  "id": "guid",
  "status": "rejected",
  "rejectedAt": "2026-07-20T12:00:00Z"
}
```

**Side effects** — envía email al autor notificando el rechazo y el feedback.

---

### Artículos / Mitos / Glosario (POST/PUT/DELETE)

Cuerpo del request parecido a las versiones públicas pero con campos administrativos extra:

```json
// POST /admin/articles
{
  "slug": "que-es-la-menopausia",
  "title": "¿Qué es la menopausia?",
  "excerpt": "...",
  "body": "<p>HTML completo</p>",
  "category": "Menopause",
  "language": "es",
  "coverImage": "https://cdn.luna.app/articles/uuid.jpg",
  "tags": ["salud", "50+"],
  "authorName": "Dra. María Pérez",
  "authorCredentials": "Ginecóloga, UNAM",
  "status": "draft | published | archived",
  "isFeatured": false,
  "publishAt": "2026-07-21T09:00:00Z"
}
```

**Response 201 Created** (o `200 OK` para PUT) — DTO completo del recurso.

---

### POST `/api/v1/admin/health-centers`

- **Auth**: Admin

**Request body**

```json
{
  "name": "Centro de Salud #14",
  "type": "HealthCenter",
  "address": "Av. Reforma 100",
  "departmentId": "14",
  "municipalityId": "001",
  "phone": "+523312345678",
  "hasGynecology": true,
  "hasObstetrics": true,
  "hasEmergency": false,
  "languagesSupported": ["es"]
}
```

**Response 201 Created** — `HealthCenterDto`.

---

### GET `/api/v1/admin/moderation/reports`

- **Auth**: Admin

**Query params**: `?status=open | resolved | dismissed&type=comment | post`.

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "contentType": "comment",
      "contentId": "guid",
      "reporterId": "guid",
      "reason": "spam",
      "description": "Promoción comercial explícita",
      "status": "open",
      "reportedAt": "2026-07-18T07:00:00Z",
      "preview": "<snippet del contenido reportado>"
    }
  ]
}
```

---

### PUT `/api/v1/admin/moderation/reports/{id}`

- **Auth**: Admin

**Request body**

```json
{
  "resolution": "remove_content | keep_content | warn_author | ban_author",
  "notes": "Se eliminó el comentario por spam"
}
```

**Response 200 OK**

```json
{
  "id": "guid",
  "status": "resolved",
  "resolvedAt": "2026-07-20T12:00:00Z",
  "resolution": "remove_content"
}
```

**Side effects**

- `remove_content` → soft-delete del comment/post original.
- `warn_author` / `ban_author` → acción sobre el usuario autor (cambia `users.warnings_count` o `is_active`).

---

### GET `/api/v1/admin/reports/analytics`

- **Auth**: Admin

**Query params opcionales**: `?from=2026-01-01&to=2026-06-30`.

**Response 200 OK**

```json
{
  "totalUsers": 4711,
  "activeUsersLast30Days": 2890,
  "usersByStage": {
    "Adolescent": 412,
    "ActiveCycle": 3100,
    "Pregnancy": 805,
    "Menopause": 394
  },
  "usersByLanguage": {
    "es": 4450,
    "nah": 110,
    "may": 87,
    "mix": 64
  },
  "totalCyclesRegistered": 12340,
  "totalPregnanciesActive": 805,
  "totalMenopauseTracking": 394,
  "contentCounts": {
    "articles": 312,
    "myths": 56,
    "glossaryTerms": 240,
    "forumPosts": 4123
  },
  "moderationPending": 12
}
```
