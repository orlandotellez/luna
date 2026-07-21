# `health_reports`

Reportes de salud generados.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `type` | `varchar(20)` | `NOT NULL` | monthly, trend, export |
| `period_start` | `date` | `NOT NULL` | |
| `period_end` | `date` | `NOT NULL` | |
| `pdf_url` | `text` | | URL del PDF generado |
| `data` | `jsonb` | | Datos del reporte (cache) |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `type`
- `period_start`

## Relaciones

`Ref: health_reports.user_id > users.id [delete: cascade]`
