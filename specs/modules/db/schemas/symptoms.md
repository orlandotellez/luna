# `symptoms`

Síntomas registrados por día.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `cycle_day_id` | `uuid` | `NOT NULL` `FK -> cycle_days.id` | |
| `type` | `symptom_type` | `NOT NULL` | Tipo de síntoma |
| `intensity` | `int` | `NOT NULL` | Intensidad 1-5 |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `cycle_day_id`
- `type`

## Relaciones

`Ref: symptoms.user_id > users.id [delete: cascade]`
`Ref: symptoms.cycle_day_id > cycle_days.id [delete: cascade]`

## Enums usados

- `symptom_type`
