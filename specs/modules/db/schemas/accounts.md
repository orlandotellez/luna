# `accounts`

Autenticación por password u OAuth.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | Usuario dueño |
| `provider_id` | `text` | `NOT NULL` | Proveedor (credentials, google, apple) |
| `account_id` | `text` | | ID de cuenta en proveedor |
| `password` | `text` | | Password hasheado (auth por credentials) |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `provider_id`
- `(provider_id, account_id)` (unique)

## Relaciones

`Ref: accounts.user_id > users.id [delete: cascade]`
