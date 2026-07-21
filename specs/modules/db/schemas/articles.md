# `articles`

Contenido educativo.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `title` | `varchar(255)` | `NOT NULL` | Título |
| `slug` | `varchar(255)` | `NOT NULL` `UNIQUE` | |
| `category` | `article_category` | `NOT NULL` | Categoría |
| `stage` | `life_stage` | | Etapa de vida asociada |
| `summary` | `text` | | Resumen breve |
| `content` | `text` | `NOT NULL` | Contenido en markdown |
| `image_url` | `text` | | URL de imagen de portada |
| `audio_url` | `text` | | URL de audio (narración) |
| `video_url` | `text` | | URL de video |
| `read_time_minutes` | `int` | | Tiempo de lectura |
| `is_published` | `boolean` | `DEFAULT false` | |
| `is_featured` | `boolean` | `DEFAULT false` | |
| `author` | `text` | | Autor/a del artículo |
| `scientific_refs` | `text[]` | | Referencias científicas |
| `search_vector` | `tsvector` | | Full-text search |
| `published_at` | `timestamptz` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | Soft-delete |

## Índices

- `slug` (unique)
- `category`
- `stage`
- `is_published`
- `is_featured`
- `search_vector` (GIN index)

## Full-Text Search

Índice GIN sobre `search_vector` (type `tsvector`):

```sql
CREATE INDEX idx_articles_search ON articles USING GIN(search_vector);
```

## Enums usados

- `article_category`
- `life_stage`
