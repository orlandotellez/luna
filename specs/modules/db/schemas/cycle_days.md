# `cycle_days`

Registro diario de síntomas dentro de un ciclo.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `cycle_id` | `uuid` | `NOT NULL` `FK -> cycles.id` | |
| `date` | `date` | `NOT NULL` | |
| `day_of_cycle` | `int` | `NOT NULL` | Día del ciclo (1 = primer día de período) |
| `phase` | `cycle_phase` | | Fase del ciclo estimada |
| `flow_intensity` | `flow_intensity` | | Intensidad del flujo (si aplica) |
| `pain_level` | `int` | | Dolor 1-5 |
| `mood` | `mood_type` | | Estado de ánimo |
| `sleep_hours` | `decimal(3,1)` | | Horas de sueño |
| `sleep_quality` | `int` | | Calidad del sueño 1-5 |
| `notes` | `text` | | Notas personales |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `cycle_id`
- `date`
- `(user_id, date)` (unique)

## Relaciones

`Ref: cycle_days.user_id > users.id [delete: cascade]`
`Ref: cycle_days.cycle_id > cycles.id [delete: cascade]`

## Enums usados

- `cycle_phase`
- `flow_intensity`
- `mood_type`
