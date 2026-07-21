# `professionals`

Profesionales de la salud.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `name` | `text` | `NOT NULL` | |
| `specialty` | `varchar(100)` | `NOT NULL` | Especialidad |
| `photo_url` | `text` | | |
| `experience_years` | `int` | | |
| `description` | `text` | | |
| `phone` | `text` | | |
| `email` | `text` | | |
| `website` | `text` | | |
| `address` | `text` | | |
| `latitude` | `decimal(10,7)` | | |
| `longitude` | `decimal(10,7)` | | |
| `department_id` | `varchar(10)` | | |
| `municipality_id` | `varchar(10)` | | |
| `languages` | `language[]` | | Idiomas que habla |
| `consultation_fee` | `decimal(10,2)` | | Costo aproximado |
| `accepts_insurance` | `boolean` | | Acepta seguro |
| `offers_teleconsult` | `boolean` | | Ofrece teleconsulta |
| `rating` | `decimal(3,2)` | | Calificación promedio |
| `rating_count` | `int` | `DEFAULT 0` | |
| `is_verified` | `boolean` | `DEFAULT false` | Perfil verificado |
| `is_active` | `boolean` | `DEFAULT true` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | Soft-delete |

## Índices

- `specialty`
- `department_id`
- `is_verified`
- `is_active`
