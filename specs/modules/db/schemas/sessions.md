# `sessions`

Sesiones activas (refresh tokens).

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `token` | `text` | `NOT NULL` `UNIQUE` | Token de sesión |
| `expires_at` | `timestamptz` | `NOT NULL` | |
| `ip_address` | `text` | | |
| `user_agent` | `text` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `token` (unique)
- `user_id`
- `expires_at`

## Relaciones

`Ref: sessions.user_id > users.id [delete: cascade]`
