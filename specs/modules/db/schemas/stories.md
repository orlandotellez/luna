# `stories`

Historias destacadas de la comunidad.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | Autora (anonimizada) |
| `title` | `varchar(255)` | `NOT NULL` | |
| `content` | `text` | `NOT NULL` | |
| `category` | `varchar(50)` | | superacion, descubrimiento, comunidad, consejos |
| `anonymous_name` | `varchar(100)` | | Nombre usado |
| `is_approved` | `boolean` | `DEFAULT false` | Aprobada por admin |
| `is_published` | `boolean` | `DEFAULT false` | |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `is_approved`
- `is_published`
- `category`

## Relaciones

`Ref: stories.user_id > users.id [delete: cascade]`
