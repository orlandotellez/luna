# `cycles`

Ciclos menstruales registrados.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `cycle_number` | `int` | `NOT NULL` | Número de ciclo (1, 2, 3...) |
| `start_date` | `date` | `NOT NULL` | Fecha de inicio del período |
| `end_date` | `date` | | Fecha de fin del período |
| `period_length_days` | `int` | | Duración del período |
| `cycle_length_days` | `int` | | Duración total del ciclo |
| `flow_intensity` | `flow_intensity` | | Intensidad del flujo |
| `notes` | `text` | | Notas personales |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | Soft-delete |

## Índices

- `user_id`
- `start_date`
- `cycle_number`
- `(user_id, cycle_number)` (unique)

## Relaciones

`Ref: cycles.user_id > users.id [delete: cascade]`

## Enums usados

- `flow_intensity`
