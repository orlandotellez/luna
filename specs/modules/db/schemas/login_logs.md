# `login_logs`

Auditoría de intentos de inicio de sesión.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `FK -> users.id` | Usuario (null si falló) |
| `email` | `text` | | Email intentado |
| `success` | `boolean` | `NOT NULL` | |
| `failure_reason` | `text` | | Motivo del fallo |
| `ip_address` | `text` | | |
| `user_agent` | `text` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `email`
- `success`
- `created_at`

## Relaciones

`Ref: login_logs.user_id > users.id [delete: set null]`
