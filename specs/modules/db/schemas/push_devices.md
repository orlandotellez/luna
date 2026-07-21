# `push_devices`

Registro de dispositivos para FCM.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `token` | `text` | `NOT NULL` `UNIQUE` | FCM token |
| `platform` | `varchar(10)` | `NOT NULL` | ios, android |
| `is_active` | `boolean` | `DEFAULT true` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `token`
- `is_active`

## Relaciones

`Ref: push_devices.user_id > users.id [delete: cascade]`
