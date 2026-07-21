# `pregnancies`

Registro de embarazo.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `last_menstrual_period` | `date` | | FUM |
| `estimated_due_date` | `date` | `NOT NULL` | FPP calculada |
| `current_week` | `int` | `NOT NULL` | Semana gestacional actual |
| `is_first_pregnancy` | `boolean` | | Primer embarazo |
| `pregnancy_count` | `int` | `DEFAULT 1` | Número de embarazo (1, 2...) |
| `is_active` | `boolean` | `NOT NULL` `DEFAULT true` | Embarazo activo |
| `ended_at` | `timestamptz` | | Fecha de término (parto) |
| `notes` | `text` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `is_active`

## Relaciones

`Ref: pregnancies.user_id > users.id [delete: cascade]`

### Tablas hijas

- `appointments` (1:N)
- `fetal_movements` (1:N)
- `weight_logs` (1:N)
- `contractions` (1:N)
- `birth_plans` (1:1)
