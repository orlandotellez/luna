# `menopause_tracking`

Seguimiento de menopausia.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `last_period_date` | `date` | | Fecha del último período |
| `months_without_period` | `int` | | Meses sin período |
| `is_in_menopause` | `boolean` | | Confirmación de menopausia |
| `symptom_start_age` | `int` | | Edad de inicio de síntomas |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`

## Relaciones

`Ref: menopause_tracking.user_id > users.id [delete: cascade]`
