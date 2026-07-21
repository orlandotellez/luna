# `weight_logs`

Registro de peso durante el embarazo.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `NOT NULL` `FK -> pregnancies.id` | |
| `date` | `date` | `NOT NULL` | |
| `weight_kg` | `decimal(5,2)` | `NOT NULL` | Peso en kg |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `pregnancy_id`
- `date`
- `(user_id, pregnancy_id, date)` (unique)

## Relaciones

`Ref: weight_logs.user_id > users.id [delete: cascade]`
`Ref: weight_logs.pregnancy_id > pregnancies.id [delete: cascade]`
