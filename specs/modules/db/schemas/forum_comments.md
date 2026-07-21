# `forum_comments`

Comentarios en posts del foro.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `post_id` | `uuid` | `NOT NULL` `FK -> forum_posts.id` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `parent_id` | `uuid` | `FK -> forum_comments.id` | Respuesta a comentario |
| `content` | `text` | `NOT NULL` | |
| `reactions` | `jsonb` | | Reacciones positivas |
| `is_edited` | `boolean` | `DEFAULT false` | |
| `is_flagged` | `boolean` | `DEFAULT false` | Reportado |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | Soft-delete |

## Índices

- `post_id`
- `user_id`
- `parent_id`

## Relaciones

`Ref: forum_comments.post_id > forum_posts.id [delete: cascade]`
`Ref: forum_comments.user_id > users.id [delete: cascade]`
`Ref: forum_comments.parent_id > forum_comments.id` (autorreferencial)
