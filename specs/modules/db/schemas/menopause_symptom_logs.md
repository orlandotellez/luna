# `menopause_symptom_logs`

Registro diario/semanal de síntomas menopáusicos.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `menopause_id` | `uuid` | `NOT NULL` `FK -> menopause_tracking.id` | |
| `date` | `date` | `NOT NULL` | |
| `type` | `menopause_symptom_type` | `NOT NULL` | Tipo de síntoma |
| `intensity` | `int` | `NOT NULL` | Intensidad 1-5 |
| `frequency_per_day` | `int` | | Frecuencia diaria |
| `notes` | `text` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `menopause_id`
- `date`
- `type`

## Relaciones

`Ref: menopause_symptom_logs.user_id > users.id [delete: cascade]`
`Ref: menopause_symptom_logs.menopause_id > menopause_tracking.id [delete: cascade]`
