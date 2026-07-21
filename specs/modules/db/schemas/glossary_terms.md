# `glossary_terms`

Glosario de términos médicos.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `term` | `varchar(255)` | `NOT NULL` `UNIQUE` | Término |
| `definition` | `text` | `NOT NULL` | Definición simple |
| `category` | `varchar(100)` | | Categoría |
| `audio_url` | `text` | | Pronunciación (audio) |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `term`
