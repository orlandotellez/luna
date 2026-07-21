# `family_messages`

Mensajes en el foro familiar.

## Esquema

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `family_group_id` | `uuid` | `NOT NULL` | Grupo familiar (user_id del que invita) |
| `sender_id` | `uuid` | `NOT NULL` `FK -> users.id` | Quien envía |
| `message` | `text` | `NOT NULL` | |
| `is_support_message` | `boolean` | `DEFAULT false` | Mensaje predefinido de apoyo |
| `created_at` | `timestamptz` | `NOT NULL` | |

## Índices

- `family_group_id`
- `sender_id`

## Relaciones

`Ref: family_messages.sender_id > users.id [delete: cascade]`
