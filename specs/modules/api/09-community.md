# 09 · Community — Foro público e historias

Endpoints para la comunidad abierta: posts del foro, comentarios, reacciones, reportes de moderación e historias destacadas.

## Tabla de endpoints

### Forum Posts

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/forum/posts` | No | Listar posts (filtros + paginación) |
| GET | `/forum/posts/{id}` | No | Detalle de un post |
| POST | `/forum/posts` | Sí | Crear post |
| PUT | `/forum/posts/{id}` | Sí | Editar post propio |
| DELETE | `/forum/posts/{id}` | Sí | Eliminar post propio (o admin) |

### Forum Comments

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/forum/posts/{id}/comments` | No | Listar comentarios de un post |
| POST | `/forum/posts/{id}/comments` | Sí | Comentar en un post |
| POST | `/forum/comments/{id}/report` | Sí | Reportar comentario (moderación) |

### Reactions

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/forum/posts/{id}/react` | Sí | Reaccionar a un post |

### Stories

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/forum/stories` | No | Historias destacadas |
| POST | `/forum/stories` | Sí | Enviar historia para revisión |

---

## Detalle de endpoints

### GET `/api/v1/forum/posts`

- **Auth**: No

**Query params**

| Param | Type | Required | Description |
|-------|------|----------|-------------|
| `stage` | string | No | `Adolescent | ActiveCycle | Pregnancy | Menopause` |
| `search` | string | No | Búsqueda textual |
| `page` | int | No | Default `1` |
| `limit` | int | No | Default `20` |

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "title": "¿Cómo manejan los cólicos?",
      "excerpt": "Llevo 3 meses con cólicos muy fuertes...",
      "author": {
        "id": "guid",
        "displayName": "María G.",
        "isAnonymous": false
      },
      "stage": "ActiveCycle",
      "reactionsCount": 24,
      "commentsCount": 8,
      "isFeatured": false,
      "createdAt": "2026-06-01T10:00:00Z",
      "updatedAt": "2026-06-01T10:00:00Z"
    }
  ],
  "page": 1,
  "limit": 20,
  "total": 312
}
```

---

### GET `/api/v1/forum/posts/{id}`

- **Auth**: No

**Response 200 OK**

```json
{
  "id": "guid",
  "title": "¿Cómo manejan los cólicos?",
  "body": "<p>Llevo 3 meses con...</p>",
  "author": {
    "id": "guid",
    "displayName": "María G.",
    "isAnonymous": false
  },
  "stage": "ActiveCycle",
  "reactionsByType": {
    "heart": 18,
    "support": 4,
    "hug": 2
  },
  "commentsCount": 8,
  "isFeatured": false,
  "createdAt": "2026-06-01T10:00:00Z"
}
```

---

### POST `/api/v1/forum/posts`

- **Auth**: Sí

**Request body**

```json
{
  "title": "¿Cómo manejan los cólicos?",
  "body": "<p>Llevo 3 meses con...</p>",
  "stage": "ActiveCycle",
  "isAnonymous": false,
  "tags": ["dolor", "menstruación"]
}
```

**Response 201 Created** — apunta a `/api/v1/forum/posts/{id}` y devuelve `ForumPostDto` completo.

**Validaciones**

- `title`: requerido, max 200 chars.
- `body`: requerido, max 10000 chars (HTML sanitizado server-side).
- `stage`: requerido.

---

### PUT `/api/v1/forum/posts/{id}`

- **Auth**: Sí (autor o Admin)

**Request body** — cualquier campo modificable.

**Errores comunes**

- `403 Forbidden` — usuario no es autor del post.
- `400 Bad Request` — solo se permite editar dentro de las primeras 24 horas posteriores a la creación.

---

### DELETE `/api/v1/forum/posts/{id}`

- **Auth**: Sí (autor o Admin)

**Response 204 No Content**.

---

### GET `/api/v1/forum/posts/{id}/comments`

- **Auth**: No

**Query params**: `?page=1&limit=20`.

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "author": {
        "id": "guid",
        "displayName": "Lucía R.",
        "isAnonymous": true
      },
      "body": "A mí me funcionó el té de manzanilla con jengibre...",
      "reactionsCount": 5,
      "isModerated": false,
      "createdAt": "2026-06-02T08:30:00Z"
    }
  ]
}
```

---

### POST `/api/v1/forum/posts/{id}/comments`

- **Auth**: Sí

**Request body**

```json
{
  "body": "A mí me funcionó el té de manzanilla con jengibre...",
  "isAnonymous": false
}
```

**Response 201 Created** — `ForumCommentDto`.

**Validaciones** — `body`: requerido, max 2000 chars.

---

### POST `/api/v1/forum/comments/{id}/report`

- **Auth**: Sí

**Request body**

```json
{
  "reason": "spam | harassment | misinformation | inappropriate | other",
  "description": "Spam comercial evidente"
}
```

**Response 201 Created**

```json
{
  "message": "Comentario reportado. Nuestro equipo de moderación lo revisará."
}
```

**Side effects** — crea entry en `moderation_reports`; admin recibe notificación in-app.

---

### POST `/api/v1/forum/posts/{id}/react`

- **Auth**: Sí

**Request body**

```json
{
  "type": "heart"
}
```

Valores de `type`: `heart | support | hug | insightful | solidarity`.

**Response 200 OK**

```json
{
  "totalReactions": 25,
  "userReaction": "heart"
}
```

> Reacción idempotente: si ya existe, se quita (toggle).

---

### GET `/api/v1/forum/stories`

- **Auth**: No

**Response 200 OK**

```json
{
  "stories": [
    {
      "id": "guid",
      "slug": "mi-vientre-de-luz",
      "title": "Mi vientre de luz",
      "excerpt": "Una historia de parto respetado...",
      "coverImage": "https://cdn.luna.app/stories/uuid.jpg",
      "publishedAt": "2026-03-15T00:00:00Z"
    }
  ]
}
```

---

### POST `/api/v1/forum/stories`

- **Auth**: Sí

**Request body**

```json
{
  "title": "Mi vientre de luz",
  "body": "<p>Una historia de parto respetado...</p>",
  "coverImage": "https://cdn.luna.app/uploads/uuid.jpg",
  "consentToPublish": true
}
```

**Response 201 Created**

```json
{
  "message": "Historia enviada para revisión",
  "storyId": "guid",
  "status": "pending_review"
}
```

> Las historias NO se publican automáticamente — pasan por moderación admin (`PUT /admin/content/{id}/approve`).
