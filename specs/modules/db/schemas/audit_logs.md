# `audit_logs`

Log de auditoría para acceso a datos sensibles.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `FK -> users.id` | Quien accedió |
| `target_user_id` | `uuid` | | Usuario cuyos datos fueron accedidos |
| `action` | `varchar(255)` | `NOT NULL` | data_access, permission_change, account_delete |
| `entity_type` | `varchar(255)` | | |
| `entity_id` | `uuid` | | |
| `ip_address` | `varchar(45)` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `target_user_id`
- `action`
- `created_at`
