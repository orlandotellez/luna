# `article_translations`

Traducciones de artículos a lenguas originarias.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `article_id` | `uuid` | `NOT NULL` `FK -> articles.id` | |
| `language` | `language` | `NOT NULL` | |
| `title` | `varchar(255)` | `NOT NULL` | |
| `summary` | `text` | | |
| `content` | `text` | `NOT NULL` | |
| `audio_url` | `text` | | Audio en esa lengua |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `article_id`
- `language`
- `(article_id, language)` (unique)

## Relaciones

`Ref: article_translations.article_id > articles.id [delete: cascade]`
