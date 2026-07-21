# 07 · Family — Acompañantes familiares

Endpoints para gestión de acompañantes (pareja, madre, hermana, hija) y comunicación privada con ellos.

> Los acompañantes usan rol `Familiar`. Solo ven datos que la usuaria principal haya compartido explícitamente.

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/family/members` | Sí | Listar acompañantes invitados |
| POST | `/family/invite` | Sí | Invitar nuevo acompañante por email |
| DELETE | `/family/members/{id}` | Sí | Eliminar acompañante |
| PUT | `/family/members/{id}` | Sí | Actualizar permisos del acompañante |
| GET | `/family/shared-calendar` | Sí | Calendario compartido (lo que la usuaria decidió mostrar) |
| GET | `/family/messages` | Sí | Listar mensajes del foro familiar |
| POST | `/family/messages` | Sí | Enviar mensaje en el foro familiar |
| PUT | `/family/messages/{id}` | Sí | Editar mensaje propio |

---

## Detalle de endpoints

### GET `/api/v1/family/members`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "members": [
    {
      "id": "guid",
      "name": "Carlos García",
      "email": "carlos@email.com",
      "relationship": "Partner",
      "permissions": {
        "canSeeCycle": true,
        "canSeeAppointments": true,
        "canSeeSymptoms": false,
        "canReceiveAlerts": true
      },
      "invitedAt": "2026-05-01T10:00:00Z",
      "acceptedAt": "2026-05-02T14:30:00Z",
      "isActive": true
    }
  ]
}
```

---

### POST `/api/v1/family/invite`

- **Auth**: Sí

**Request body**

```json
{
  "name": "Carlos García",
  "email": "carlos@email.com",
  "relationship": "Partner",
  "permissions": {
    "canSeeCycle": true,
    "canSeeAppointments": true,
    "canReceiveAlerts": true
  }
}
```

Valores de `relationship`: `Partner | Mother | Sister | Daughter | Other`.

**Response 201 Created**

```json
{
  "invitationId": "guid",
  "message": "Invitación enviada a carlos@email.com"
}
```

**Side effects** — envía email con link de aceptación (SendGrid).

**Errores comunes**

- `409 Conflict` — email ya invitado o ya es miembro.

---

### DELETE `/api/v1/family/members/{id}`

- **Auth**: Sí

**Response 204 No Content**.

**Side effects** — soft-delete; el acompañante pierde acceso inmediato a datos compartidos.

---

### PUT `/api/v1/family/members/{id}`

- **Auth**: Sí

**Request body**

```json
{
  "permissions": {
    "canSeeCycle": true,
    "canSeeAppointments": true,
    "canSeeSymptoms": true,
    "canReceiveAlerts": true
  }
}
```

**Response 200 OK** — `FamilyMemberDto` actualizado.

---

### GET `/api/v1/family/shared-calendar`

- **Auth**: Sí (`User`) — o rol `Familiar` que la usuaria marcó como `canSeeAppointments = true`.

**Query params**: `?month=6&year=2026`.

**Response 200 OK**

```json
{
  "month": 6,
  "year": 2026,
  "sharedDays": [
    { "date": "2026-06-10", "sharedType": "period_start" },
    { "date": "2026-06-15", "sharedType": "appointment", "title": "Control prenatal" }
  ]
}
```

> Solo aparecen los días cuyo `sharedType` esté habilitado en los permisos del acompañante.

---

### GET `/api/v1/family/messages`

- **Auth**: Sí

**Query params**: `?cursor=...&limit=20`.

**Response 200 OK**

```json
{
  "messages": [
    {
      "id": "guid",
      "authorId": "guid",
      "authorName": "Carlos García",
      "authorRole": "Familiar",
      "body": "¿Cómo te sientes hoy?",
      "sentAt": "2026-06-12T08:00:00Z",
      "readByUser": true
    }
  ],
  "nextCursor": null
}
```

---

### POST `/api/v1/family/messages`

- **Auth**: Sí

**Request body**

```json
{
  "body": "Hoy fue un buen día. Dormí mejor."
}
```

**Response 201 Created** — apunta a `/api/v1/family/messages/{id}`.

**Validaciones** — `body`: requerido, max 1000 chars.

---

### PUT `/api/v1/family/messages/{id}`

- **Auth**: Sí

**Request body**

```json
{
  "body": "Texto editado"
}
```

**Response 200 OK** — `FamilyMessageDto` actualizado (incluye `editedAt`).

**Errores comunes**

- `403 Forbidden` — el mensaje no pertenece a la usuaria autenticada.
- `400 Bad Request` — solo editable dentro de los primeros 15 minutos desde el envío.
