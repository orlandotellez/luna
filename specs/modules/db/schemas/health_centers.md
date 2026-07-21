# `health_centers`

Centros de salud, clínicas y hospitales.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `name` | `text` | `NOT NULL` | |
| `type` | `varchar(50)` | `NOT NULL` | clinic, hospital, community_center |
| `phone` | `text` | | |
| `address` | `text` | | |
| `latitude` | `decimal(10,7)` | | |
| `longitude` | `decimal(10,7)` | | |
| `department_id` | `varchar(10)` | | |
| `municipality_id` | `varchar(10)` | | |
| `services` | `text[]` | | Servicios ofrecidos |
| `schedule` | `text` | | Horario |
| `is_public` | `boolean` | `DEFAULT true` | Público o privado |
| `is_active` | `boolean` | `DEFAULT true` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `type`
- `department_id`
- `is_public`
