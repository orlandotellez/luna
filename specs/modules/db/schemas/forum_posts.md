# `forum_posts`

Posts del foro por etapa.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | Autora |
| `stage` | `life_stage` | `NOT NULL` | Etapa del foro |
| `title` | `varchar(255)` | `NOT NULL` | |
| `content` | `text` | `NOT NULL` | |
| `is_anonymous` | `boolean` | `DEFAULT false` | Publicar anónimamente |
| `is_published` | `boolean` | `DEFAULT true` | |
| `reactions` | `jsonb` | `DEFAULT '{}'` | Conteo de reacciones |
| `comments_count` | `int` | `DEFAULT 0` | |
| `reports_count` | `int` | `DEFAULT 0` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | Soft-delete |

## Índices

- `user_id`
- `stage`
- `is_published`
- `created_at`

## Relaciones

`Ref: forum_posts.user_id > users.id [delete: cascade]`

## Enums usados

- `life_stage`
