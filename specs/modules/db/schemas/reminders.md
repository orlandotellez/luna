# `reminders`

Recordatorios personalizados.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `type` | `varchar(50)` | `NOT NULL` | pill, period, appointment, exam, vaccination, affirmation |
| `title` | `varchar(255)` | `NOT NULL` | |
| `description` | `text` | | |
| `frequency` | `reminder_frequency` | `NOT NULL` | |
| `time` | `time` | | Hora del recordatorio |
| `days_of_week` | `int[]` | | Días de la semana (1=lunes .. 7=domingo) |
| `day_of_month` | `int` | | Día del mes |
| `start_date` | `date` | | Fecha de inicio |
| `end_date` | `date` | | Fecha de fin (opcional) |
| `is_enabled` | `boolean` | `DEFAULT true` | |
| `last_triggered_at` | `timestamptz` | | Última vez que se disparó |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `type`
- `is_enabled`

## Relaciones

`Ref: reminders.user_id > users.id [delete: cascade]`

## Enums usados

- `reminder_frequency`
