# `myths`

Mitos vs Realidad.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `myth_text` | `text` | `NOT NULL` | El mito |
| `reality_text` | `text` | `NOT NULL` | La realidad con respaldo científico |
| `category` | `article_category` | `NOT NULL` | Categoría |
| `scientific_ref` | `text` | | Referencia científica |
| `is_published` | `boolean` | `DEFAULT false` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `category`
- `is_published`
