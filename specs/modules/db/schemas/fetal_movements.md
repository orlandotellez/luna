# `fetal_movements`

Registro de movimientos fetales (patadas).

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `NOT NULL` `FK -> pregnancies.id` | |
| `start_time` | `timestamptz` | `NOT NULL` | Inicio de la sesión de conteo |
| `total_kicks` | `int` | `NOT NULL` | Total de movimientos |
| `duration_minutes` | `int` | `NOT NULL` | Duración de la sesión |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `pregnancy_id`
- `start_time`

## Relaciones

`Ref: fetal_movements.user_id > users.id [delete: cascade]`
`Ref: fetal_movements.pregnancy_id > pregnancies.id [delete: cascade]`
