# `verifications`

Tokens de verificación (email, password reset).

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `identifier` | `text` | `NOT NULL` | Email o teléfono |
| `value` | `text` | `NOT NULL` | Token |
| `expires_at` | `timestamptz` | `NOT NULL` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `identifier`
- `value`
- `expires_at`
