# 10 · Reminders — Recordatorios

Endpoints para crear y gestionar recordatorios (anticonceptivos, citas, hidratarse, ejercicios, etc.) que disparan notificaciones push programadas.

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/reminders` | Sí | Listar recordatorios de la usuaria |
| POST | `/reminders` | Sí | Crear recordatorio |
| PUT | `/reminders/{id}` | Sí | Actualizar recordatorio |
| DELETE | `/reminders/{id}` | Sí | Eliminar (soft-delete) |
| PATCH | `/reminders/{id}/toggle` | Sí | Activar / desactivar (toggle) |

---

## Detalle de endpoints

### GET `/api/v1/reminders`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "type": "Pill",
      "title": "Tomar anticonceptivo",
      "frequency": "Daily",
      "time": "08:00",
      "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
      "enabled": true,
      "nextTriggerAt": "2026-07-21T08:00:00Z",
      "createdAt": "2026-01-10T08:00:00Z"
    }
  ]
}
```

---

### POST `/api/v1/reminders`

- **Auth**: Sí

**Request body**

```json
{
  "type": "Pill",
  "title": "Tomar anticonceptivo",
  "frequency": "Daily",
  "time": "08:00",
  "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
  "enabled": true
}
```

Valores de `type`: `Pill | Period | Appointment | Hydration | Exercise | Custom`.
Valores de `frequency`: `Daily | Weekly | Monthly | Once | Custom`.

**Response 201 Created** — apunta a `/api/v1/reminders/{id}`.

**Validaciones**

- `type`: requerido.
- `title`: requerido, max 100 chars.
- `frequency`: requerido.
- `time`: requerido si frequency ≠ Once.
- `daysOfWeek`: requerido si frequency = Weekly (`1..7`).

`nextTriggerAt` se calcula en backend.

---

### PUT `/api/v1/reminders/{id}`

- **Auth**: Sí

**Request body** — campos opcionales para actualización parcial.

**Response 200 OK** — `ReminderDto` actualizado (incluye `nextTriggerAt` recalculado).

---

### DELETE `/api/v1/reminders/{id}`

- **Auth**: Sí

**Response 204 No Content**.

**Side effects** — soft-delete; cancela próximas notificaciones programadas.

---

### PATCH `/api/v1/reminders/{id}/toggle`

- **Auth**: Sí

**Request body**: ninguno.

**Request header**: opcional `X-Want-State: enabled | disabled` — si no, hace toggle.

**Response 200 OK**

```json
{
  "id": "guid",
  "enabled": false,
  "nextTriggerAt": null
}
```
