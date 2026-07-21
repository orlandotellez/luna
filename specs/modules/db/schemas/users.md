# `users`

Tabla central de usuarias y acompañantes.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` `DEFAULT uuid_generate_v4()` | Identificador único |
| `name` | `text` | `NOT NULL` | Nombre completo |
| `email` | `text` | `NOT NULL` `UNIQUE` | Email de acceso |
| `email_verified` | `boolean` | `NOT NULL` `DEFAULT false` | Email verificado |
| `phone` | `text` | | Teléfono |
| `image` | `text` | | URL de avatar/foto |
| `role` | `user_role` | `NOT NULL` `DEFAULT 'USER'` | Rol del usuario |
| `life_stage` | `life_stage` | `NOT NULL` | Etapa de vida actual |
| `language` | `language` | `NOT NULL` `DEFAULT 'ES'` | Idioma preferido |
| `department_id` | `varchar(10)` | | Departamento de ubicación |
| `municipality_id` | `varchar(10)` | | Municipio de ubicación |
| `date_of_birth` | `date` | | Fecha de nacimiento |
| `is_active` | `boolean` | `NOT NULL` `DEFAULT true` | Cuenta activa/inactiva |
| `last_seen_at` | `timestamptz` | | Última vez online |
| `created_at` | `timestamptz` | `NOT NULL` `DEFAULT CURRENT_TIMESTAMP` | |
| `updated_at` | `timestamptz` | `NOT NULL` `DEFAULT CURRENT_TIMESTAMP` | |
| `deleted_at` | `timestamptz` | | Soft-delete |

## Índices

- `email` (unique)
- `life_stage`
- `department_id`
- `created_at`

## Relaciones

| Tabla | Tipo | |
|-------|------|---|
| `accounts` | 1:N | `users.id → accounts.user_id` |
| `sessions` | 1:N | `users.id → sessions.user_id` |
| `health_profiles` | 1:1 | `users.id → health_profiles.user_id` |
| `cycles` | 1:N | `users.id → cycles.user_id` |
| `pregnancies` | 1:N | `users.id → pregnancies.user_id` |
| `family_members` | 1:N | `users.id → family_members.user_id` |

## Enums usados

- `user_role`
- `life_stage`
- `language`
