# `appointments`

Citas médicas prenatales y ginecológicas.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `FK -> pregnancies.id` | Si aplica |
| `type` | `appointment_type` | `NOT NULL` | Tipo de cita |
| `title` | `varchar(255)` | `NOT NULL` | |
| `description` | `text` | | |
| `scheduled_at` | `timestamptz` | `NOT NULL` | Fecha y hora |
| `location` | `text` | | Ubicación |
| `professional_name` | `text` | | Nombre del profesional |
| `is_completed` | `boolean` | `DEFAULT false` | |
| `notes` | `text` | | Notas post-cita |
| `reminder_sent` | `boolean` | `DEFAULT false` | Recordatorio enviado |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | Soft-delete |

## Índices

- `user_id`
- `pregnancy_id`
- `scheduled_at`
- `type`

## Relaciones

`Ref: appointments.user_id > users.id [delete: cascade]`
`Ref: appointments.pregnancy_id > pregnancies.id [delete: cascade]`

## Enums usados

- `appointment_type`
