# `family_members`

Acompañantes autorizados por la usuaria.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | Usuaria que invita |
| `family_user_id` | `uuid` | `FK -> users.id` | Usuario acompañante (si se registró) |
| `name` | `text` | `NOT NULL` | Nombre del acompañante |
| `email` | `text` | `NOT NULL` | Email del acompañante |
| `relationship` | `family_relationship` | `NOT NULL` | Parentesco |
| `invitation_token` | `text` | | Token de invitación |
| `status` | `varchar(20)` | `NOT NULL` `DEFAULT 'PENDING'` | PENDING, ACCEPTED, REJECTED, REVOKED |
| `can_view_calendar` | `boolean` | `DEFAULT true` | Permiso: ver calendario |
| `can_receive_alerts` | `boolean` | `DEFAULT true` | Permiso: recibir alertas |
| `can_send_messages` | `boolean` | `DEFAULT true` | Permiso: enviar mensajes |
| `invited_at` | `timestamptz` | `NOT NULL` | |
| `accepted_at` | `timestamptz` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `user_id`
- `family_user_id`
- `email`
- `status`

## Relaciones

`Ref: family_members.user_id > users.id [delete: cascade]`
`Ref: family_members.family_user_id > users.id [delete: set null]`

## Enums usados

- `family_relationship`
