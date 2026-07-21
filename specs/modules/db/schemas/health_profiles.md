# `health_profiles`

Perfil de salud completo de la usuaria.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `UNIQUE` `FK -> users.id` | |
| `has_regular_cycle` | `boolean` | | Ciclo regular |
| `cycle_length_days` | `int` | | Duración del ciclo en días |
| `period_length_days` | `int` | | Duración del período en días |
| `has_endometriosis` | `boolean` | `DEFAULT false` | |
| `has_pcos` | `boolean` | `DEFAULT false` | Síndrome de ovario poliquístico |
| `has_thyroid_issues` | `boolean` | `DEFAULT false` | Problemas de tiroides |
| `has_gestational_diabetes` | `boolean` | `DEFAULT false` | Diabetes gestacional |
| `has_fibroids` | `boolean` | `DEFAULT false` | Miomas |
| `has_hypertension` | `boolean` | `DEFAULT false` | Hipertensión |
| `allergies` | `text` | | Alergias conocidas |
| `medications` | `text[]` | | Medicamentos actuales |
| `previous_pregnancies` | `int` | `DEFAULT 0` | Número de embarazos previos |
| `surgeries` | `text` | | Cirugías previas |
| `vaccinations` | `text[]` | | Vacunas recibidas |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`

## Relaciones

`Ref: health_profiles.user_id > users.id [delete: cascade]`
