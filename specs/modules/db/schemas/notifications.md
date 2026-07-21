# `notifications`

Notificaciones in-app.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `type` | `notification_type` | `NOT NULL` | |
| `title` | `varchar(255)` | `NOT NULL` | |
| `body` | `text` | `NOT NULL` | |
| `image_url` | `text` | | |
| `action_url` | `text` | | Deep link |
| `is_read` | `boolean` | `DEFAULT false` | |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `type`
- `is_read`
- `created_at`

## Relaciones

`Ref: notifications.user_id > users.id [delete: cascade]`

## Enums usados

- `notification_type`
