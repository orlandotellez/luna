# `contractions`

Registro de contracciones (preparto).

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `NOT NULL` `FK -> pregnancies.id` | |
| `session_start` | `timestamptz` | `NOT NULL` | Inicio de la sesión |
| `start_time` | `timestamptz` | `NOT NULL` | Inicio de la contracción |
| `end_time` | `timestamptz` | `NOT NULL` | Fin de la contracción |
| `duration_seconds` | `int` | `NOT NULL` | Duración |
| `interval_seconds` | `int` | | Intervalo desde la anterior |
| `intensity` | `int` | | Intensidad 1-5 |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `pregnancy_id`
- `session_start`

## Relaciones

`Ref: contractions.user_id > users.id [delete: cascade]`
`Ref: contractions.pregnancy_id > pregnancies.id [delete: cascade]`
