# 04 · Pregnancy — Embarazo

Endpoints para gestión completa del embarazo: semanas, patadas fetales, peso, contracciones, plan de parto y citas médicas.

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/pregnancy/register` | Sí | Activar etapa Pregnancy y crear registro de embarazo |
| GET | `/pregnancy/current` | Sí | Resumen del embarazo activo |
| GET | `/pregnancy/week/{weekNumber}` | Sí | Info detallada de una semana específica |
| GET | `/pregnancy/weeks` | Sí | Listado de todas las semanas (progreso) |
| POST | `/pregnancy/kicks` | Sí | Registrar sesión de movimientos fetales |
| GET | `/pregnancy/kicks/history` | Sí | Historial de sesiones de patadas |
| POST | `/pregnancy/weight` | Sí | Registrar peso |
| GET | `/pregnancy/weight/history` | Sí | Historial de peso |
| POST | `/pregnancy/contractions` | Sí | Registrar contracción individual |
| GET | `/pregnancy/contractions/session` | Sí | Sesión actual de contracciones (para UI de preparto) |
| GET | `/pregnancy/birth-plan` | Sí | Obtener plan de parto |
| PUT | `/pregnancy/birth-plan` | Sí | Crear / actualizar plan de parto (upsert) |
| GET | `/pregnancy/appointments` | Sí | Listar citas prenatales |
| POST | `/pregnancy/appointments` | Sí | Crear cita prenatal |
| PUT | `/pregnancy/appointments/{id}` | Sí | Actualizar cita |
| DELETE | `/pregnancy/appointments/{id}` | Sí | Eliminar cita (soft-delete) |

---

## Detalle de endpoints

### POST `/api/v1/pregnancy/register`

- **Auth**: Sí

**Request body**

```json
{
  "lastMenstrualPeriod": "2026-01-15",
  "isFirstPregnancy": true
}
```

**Response 201 Created**

```json
{
  "pregnancyId": "guid",
  "currentWeek": 26,
  "estimatedDueDate": "2026-10-22",
  "trimester": 2,
  "weeksRemaining": 14
}
```

**Side effects**

- Setea `users.lifeStage = "Pregnancy"`.
- Crea registro en `pregnancies` con `isActive = true`.

**Errores comunes**

- `409 Conflict` — ya existe embarazo activo.

---

### GET `/api/v1/pregnancy/current`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "pregnancyId": "guid",
  "lastMenstrualPeriod": "2026-01-15",
  "estimatedDueDate": "2026-10-22",
  "currentWeek": 26,
  "trimester": 2,
  "weeksRemaining": 14,
  "isFirstPregnancy": true,
  "isActive": true,
  "notes": null
}
```

---

### GET `/api/v1/pregnancy/week/{weekNumber}`

- **Auth**: Sí

**Path params**: `weekNumber` ∈ `1..42`.

**Response 200 OK**

```json
{
  "weekNumber": 26,
  "trimester": 2,
  "title": "Semana 26 — El bebé responde a sonidos",
  "babySizeCm": 35.6,
  "babyWeightG": 760,
  "highlights": ["Desarrollo auditivo", "Reflejos de sobresalto"],
  "tipsForMom": ["Hablarle al bebé", "Dormir sobre el lado izquierdo"],
  "alerts": []
}
```

**Errores comunes**

- `404 Not Found` — semana fuera de rango o sin contenido publicado.

---

### GET `/api/v1/pregnancy/weeks`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "weeks": [
    { "weekNumber": 25, "trimester": 2, "title": "Semana 25 — ...", "isCompleted": true, "isCurrent": false },
    { "weekNumber": 26, "trimester": 2, "title": "Semana 26 — ...", "isCompleted": false, "isCurrent": true }
  ]
}
```

---

### POST `/api/v1/pregnancy/kicks`

- **Auth**: Sí

**Request body**

```json
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

**Response 201 Created**

```json
{
  "id": "guid",
  "startTime": "2026-06-12T09:00:00Z",
  "totalKicks": 12,
  "durationMinutes": 30
}
```

> `alertLevel`: `low` | `normal` | `high`. Backend evalúa umbrales clínicos; `high` puede disparar notificación push al acompañante.

---

### GET `/api/v1/pregnancy/kicks/history`

- **Auth**: Sí

**Query params opcionales**: `?from=2026-01-01&to=2026-06-30&limit=50`.

**Response 200 OK**

```json
{
  "sessions": [
    {
      "id": "guid",
      "startTime": "2026-06-12T09:00:00Z",
      "totalKicks": 12,
      "durationMinutes": 30,
      "alertLevel": "normal"
    }
  ]
}
```

---

### POST `/api/v1/pregnancy/weight`

- **Auth**: Sí

**Request body**

```json
{
  "date": "2026-06-12",
  "weightKg": 67.4
}
```

**Response 201 Created**

```json
{
  "id": "guid",
  "date": "2026-06-12",
  "weightKg": 67.4
}
```

---

### GET `/api/v1/pregnancy/weight/history`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "logs": [
    { "id": "guid", "date": "2026-06-01", "weightKg": 67.0 },
    { "id": "guid", "date": "2026-06-12", "weightKg": 67.4 }
  ],
  "totalGainKg": 8.4,
  "recommendedGainKg": [11.5, 16.0]
}
```

---

### POST `/api/v1/pregnancy/contractions`

- **Auth**: Sí

**Request body**

```json
{
  "sessionStart": "2026-07-15T20:00:00Z",
  "startTime": "2026-07-15T20:05:00Z",
  "endTime": "2026-07-15T20:06:00Z",
  "intensity": 4
}
```

**Response 201 Created**

```json
{
  "id": "guid",
  "durationSeconds": 60,
  "intervalSeconds": 300,
  "intensity": 4
}
```

> El cálculo de `intervalSeconds` requiere que la contracción venga después de otra registrada en la misma sesión; si es la primera, el campo queda `null`.

---

### GET `/api/v1/pregnancy/contractions/session`

- **Auth**: Sí

**Query params**: `?sessionStart=2026-07-15T20:00:00Z`.

**Response 200 OK**

```json
{
  "sessionStart": "2026-07-15T20:00:00Z",
  "contractions": [
    {
      "id": "guid",
      "startTime": "2026-07-15T20:05:00Z",
      "endTime": "2026-07-15T20:06:00Z",
      "durationSeconds": 60,
      "intervalSeconds": null,
      "intensity": 4
    }
  ]
}
```

---

### GET `/api/v1/pregnancy/birth-plan`

- **Auth**: Sí

**Response 200 OK** (404 si no existe)

```json
{
  "birthType": "vaginal",
  "companion": "Carlos García",
  "painManagement": "epidural",
  "birthPosition": "sentada",
  "skinToSkin": true,
  "delayedCordClamping": true,
  "immediateBreastfeeding": true,
  "specialRequests": "Sin óxido nitroso"
}
```

---

### PUT `/api/v1/pregnancy/birth-plan`

- **Auth**: Sí

**Request body** (mismos campos que GET). Upsert: crea si no existe, actualiza si existe.

**Response 200 OK** — mismo shape que GET.

---

### GET `/api/v1/pregnancy/appointments`

- **Auth**: Sí

**Query params opcionales**: `?from=2026-01-01&to=2026-12-31&type=Prenatal`.

**Response 200 OK**

```json
{
  "appointments": [
    {
      "id": "guid",
      "type": "Prenatal",
      "title": "Control prenatal — semana 28",
      "scheduledAt": "2026-07-10T10:00:00Z",
      "location": "Centro de Salud #14",
      "professionalName": "Dra. López",
      "isCompleted": false,
      "reminderSent": false,
      "notes": null
    }
  ]
}
```

---

### POST `/api/v1/pregnancy/appointments`

- **Auth**: Sí

**Request body**

```json
{
  "type": "Ultrasound",
  "title": "Ultrasonido morfológico",
  "scheduledAt": "2026-07-10T10:00:00Z",
  "location": "Centro de Salud #14",
  "professionalName": "Dra. López",
  "description": "Ultrasonido de detalle morfológico"
}
```

**Response 201 Created** — apunta a `/api/v1/pregnancy/appointments/{id}` y devuelve `AppointmentDto` completo.

---

### PUT `/api/v1/pregnancy/appointments/{id}`

- **Auth**: Sí

**Request body** — campos opcionales para actualización parcial.

**Response 200 OK** — `AppointmentDto` actualizado.

---

### DELETE `/api/v1/pregnancy/appointments/{id}`

- **Auth**: Sí

**Response 204 No Content**.

**Side effects** — soft-delete (`appointments.deleted_at`); no se elimina físicamente.
