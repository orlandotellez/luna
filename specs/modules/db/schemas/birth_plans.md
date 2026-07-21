# `birth_plans`

Plan de parto de la usuaria.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `NOT NULL` `FK -> pregnancies.id` | |
| `birth_type` | `text` | | Preferencia de parto (vaginal, cesárea, natural) |
| `companion` | `text` | | Acompañante durante el parto |
| `pain_management` | `text` | | Manejo del dolor |
| `birth_position` | `text` | | Posición preferida |
| `skin_to_skin` | `boolean` | `DEFAULT true` | Contacto piel con piel |
| `delayed_cord_clamping` | `boolean` | `DEFAULT true` | Clampaje tardío del cordón |
| `immediate_breastfeeding` | `boolean` | `DEFAULT true` | Lactancia inmediata |
| `special_requests` | `text` | | Solicitudes especiales |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `pregnancy_id`
- `(user_id, pregnancy_id)` (unique)

## Relaciones

`Ref: birth_plans.user_id > users.id [delete: cascade]`
`Ref: birth_plans.pregnancy_id > pregnancies.id [delete: cascade]`
