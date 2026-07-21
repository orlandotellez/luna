# 02 · Users — Perfil y salud de la usuaria

Endpoints para gestionar el perfil de la usuaria autenticada: datos básicos, etapa de vida, perfil de salud, avatar, eliminación de cuenta y exportación de datos.

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/users/me` | Sí | Obtener perfil de la usuaria autenticada |
| PUT | `/users/me` | Sí | Actualizar datos básicos del perfil |
| PUT | `/users/me/life-stage` | Sí | Cambiar etapa de vida (con efecto en módulos downstream) |
| PUT | `/users/me/avatar` | Sí | Subir foto de perfil (multipart) |
| GET | `/users/me/health-profile` | Sí | Obtener perfil de salud completo |
| PUT | `/users/me/health-profile` | Sí | Actualizar perfil de salud |
| DELETE | `/users/me/account` | Sí | Eliminar cuenta (soft-delete vía `deleted_at`) |
| GET | `/users/me/reports` | Sí | Listar reportes mensuales generados |
| POST | `/users/me/export-data` | Sí | Solicitar exportación completa de datos (async) |

---

## Detalle de endpoints

### GET `/api/v1/users/me`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "id": "guid",
  "name": "María García",
  "email": "maria@email.com",
  "phone": "+525512345678",
  "image": "https://cdn.luna.app/avatars/uuid.jpg",
  "role": "USER",
  "lifeStage": "ActiveCycle",
  "language": "es",
  "departmentId": "14",
  "municipalityId": "001",
  "dateOfBirth": "1995-04-12",
  "isActive": true,
  "lastSeenAt": "2026-07-20T18:45:00Z",
  "createdAt": "2026-01-10T08:00:00Z"
}
```

---

### PUT `/api/v1/users/me`

- **Auth**: Sí

**Request body** (todos los campos opcionales, solo se actualizan los enviados)

```json
{
  "name": "María García López",
  "phone": "+525512345678",
  "image": "https://cdn.luna.app/avatars/uuid.jpg",
  "language": "es",
  "departmentId": "14",
  "municipalityId": "001",
  "dateOfBirth": "1995-04-12"
}
```

**Response 200 OK** — perfil actualizado (mismo shape que `GET /users/me`).

---

### PUT `/api/v1/users/me/life-stage`

- **Auth**: Sí

**Request body**

```json
{
  "lifeStage": "Pregnancy",
  "lastMenstrualPeriod": "2026-01-15",
  "estimatedDueDate": "2026-10-22"
}
```

**Response 200 OK**

```json
{
  "lifeStage": "Pregnancy",
  "pregnancyId": "guid",
  "currentWeek": 26,
  "estimatedDueDate": "2026-10-22"
}
```

> ⚠️ **Side effect**: al pasar a `Pregnancy` se crea automáticamente un registro en `pregnancies` y desactiva temporalmente el módulo de ciclo menstrual en el frontend.

**Validaciones**

- `lifeStage`: requerido, valores válidos: `Adolescent | ActiveCycle | Pregnancy | Menopause`.
- `lastMenstrualPeriod` y `estimatedDueDate`: requeridos solo si `lifeStage === "Pregnancy"`.

---

### PUT `/api/v1/users/me/avatar`

- **Auth**: Sí
- **Content-Type**: `multipart/form-data`

**Request (form-data)**

| Field | Type | Description |
|-------|------|-------------|
| `file` | `binary` | Imagen JPG/PNG, max 5 MB |

**Response 200 OK**

```json
{
  "image": "https://cdn.luna.app/avatars/uuid.jpg"
}
```

**Side effects**

- Sube a R2 (Cloudflare) via `IFileStorageService`.

---

### GET `/api/v1/users/me/health-profile`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "id": "guid",
  "hasRegularCycle": true,
  "cycleLengthDays": 28,
  "periodLengthDays": 5,
  "hasEndometriosis": false,
  "hasPcos": false,
  "hasThyroidIssues": false,
  "hasGestationalDiabetes": false,
  "hasFibroids": false,
  "hasHypertension": false,
  "allergies": "Penicilina",
  "medications": ["Levotiroxina 50mcg"],
  "previousPregnancies": 0,
  "surgeries": null,
  "vaccinations": ["COVID-19", "Influenza"]
}
```

> Si la usuaria todavía no creó perfil de salud → `404 Not Found`.

---

### PUT `/api/v1/users/me/health-profile`

- **Auth**: Sí
- **Content-Type**: `application/json`

**Request body** (idénticos campos que el GET, todos opcionales; upsert)

```json
{
  "hasRegularCycle": true,
  "cycleLengthDays": 28,
  "periodLengthDays": 5,  "hasEndometriosis": false,
  "hasPcos": false,
  "allergies": "Penicilina",
  "medications": ["Levotiroxina 50mcg"] 
}
```

**Response 200 OK** — perfil de salud completo (mismo shape que GET).

---

### DELETE `/api/v1/users/me/account`

- **Auth**: Sí

**Request body**

```json
{
  "password": "MiPassword123",
  "confirmation": "ELIMINAR"
}
```

**Response 204 No Content** — sin body.

**Side effects**

- Soft-delete: setea `users.deleted_at` y `is_active = false`.
- Revoca todas las `sessions` activas.
- Pasa a cola de anonimización (después de 30 días se eliminan los datos personales).

**Errores comunes**

- `400 Bad Request` — password incorrecto o `confirmation` ≠ `"ELIMINAR"`.

---

### GET `/api/v1/users/me/reports`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "reports": [
    {
      "id": "guid",
      "type": "monthly",
      "year": 2026,
      "month": 6,
      "generatedAt": "2026-07-01T00:00:00Z",
      "downloadUrl": "https://cdn.luna.app/reports/uuid.pdf"
    }
  ]
}
```

> Query params opcionales: `?year=2026&month=6` para filtrar.

---

### POST `/api/v1/users/me/export-data`

- **Auth**: Sí

**Request body**

```json
{
  "format": "json"
}
```

Valores permitidos: `json | csv | pdf`.

**Response 202 Accepted**

```json
{
  "message": "Solicitud de exportación recibida. Te avisaremos por email cuando esté lista.",
  "requestId": "guid"
}
```

**Side effects**

- Encola un job que genera un archivo con todos los datos de la usuaria (ciclo, embarazo, perfil, posts propios).
- Al terminar, envía email con link de descarga (válido 7 días).
