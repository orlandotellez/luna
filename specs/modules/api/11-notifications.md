# 11 · Notifications — Notificaciones in-app y push

Endpoints para gestionar notificaciones: listado, marcar como leídas, registrar tokens FCM (Firebase Cloud Messaging) para push.

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/notifications` | Sí | Listar notificaciones in-app |
| GET | `/notifications/unread-count` | Sí | Contador de no leídas (badge) |
| PATCH | `/notifications/{id}/read` | Sí | Marcar una como leída |
| PATCH | `/notifications/read-all` | Sí | Marcar todas como leídas |
| POST | `/notifications/register-device` | Sí | Registrar token FCM del dispositivo |
| DELETE | `/notifications/unregister-device` | Sí | Eliminar token FCM |

---

## Detalle de endpoints

### GET `/api/v1/notifications`

- **Auth**: Sí

**Query params opcionales**: `?type=PeriodReminder | unreadOnly=true | page=1&limit=20`.

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "type": "PeriodReminder",
      "title": "Tu período está por comenzar",
      "body": "Según tu historial, debería iniciar mañana.",
      "data": { "deepLink": "/calendar" },
      "readAt": null,
      "createdAt": "2026-07-15T07:00:00Z"
    }
  ],
  "unreadCount": 5,
  "page": 1,
  "limit": 20
}
```

Valores de `type`: `PeriodReminder | PillReminder | Appointment | Content | FamilyMessage | Support | Alert | Affirmation`.

---

### GET `/api/v1/notifications/unread-count`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "unreadCount": 5
}
```

> Endpoint ligero, pensado para el badge de la app.

---

### PATCH `/api/v1/notifications/{id}/read`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "id": "guid",
  "readAt": "2026-07-16T09:30:00Z"
}
```

**Side effects** — setea `readAt`; decrementa contador global.

---

### PATCH `/api/v1/notifications/read-all`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "markedRead": 5
}
```

---

### POST `/api/v1/notifications/register-device`

- **Auth**: Sí

**Request body**

```json
{
  "fcmToken": "string-from-Firebase-Messaging",
  "platform": "ios | android",
  "appVersion": "1.0.5",
  "deviceId": "string-uuid"
}
```

**Response 201 Created**

```json
{
  "message": "Device registered successfully",
  "deviceId": "guid"
}
```

**Side effects** — inserta/actualiza en `push_devices`. Backend podrá mandar push vía Firebase Admin SDK.

---

### DELETE `/api/v1/notifications/unregister-device`

- **Auth**: Sí

**Request body**

```json
{
  "fcmToken": "string-from-Firebase-Messaging"
}
```

**Response 204 No Content**.
