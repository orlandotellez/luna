# 01 · Auth — Autenticación y sesión

Endpoints para registro, login, refresh de tokens, verificación de email, recuperación de contraseña y registro biométrico.

> ⚠️ **Importante**: la API **nunca devuelve tokens en el body**. Los tokens se transmiten exclusivamente como cookies `HttpOnly`, `Secure`, `SameSite=Strict`.

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/auth/register` | No | Registrar nueva usuaria y crear primer `Account` |
| POST | `/auth/login` | No | Iniciar sesión y obtener cookies de tokens |
| POST | `/auth/refresh` | No (requiere cookie `refreshToken`) | Renovar access token antes de su expiración |
| POST | `/auth/logout` | Sí | Cerrar sesión: revocar `Session` y limpiar cookies |
| POST | `/auth/verify-email` | No | Verificar email con token de un solo uso |
| POST | `/auth/resend-verification` | No | Reenviar email de verificación |
| POST | `/auth/forgot-password` | No | Solicitar reseteo de contraseña (envía email) |
| POST | `/auth/reset-password` | No | Resetear contraseña con token |
| POST | `/auth/biometric` | Sí | Registrar clave pública biométrica para login sin contraseña |

---

## Detalle de endpoints

### POST `/api/v1/auth/register`

- **Auth**: No
- **Content-Type**: `application/json`

**Request body**

```json
{
  "name": "María García",
  "email": "maria@email.com",
  "phone": "+525512345678",
  "password": "MiPassword123",
  "lifeStage": "ActiveCycle",
  "language": "es",
  "departmentId": "14",
  "municipalityId": "001"
}
```

**Response 201 Created**

```json
{
  "message": "Usuario registrado exitosamente",
  "user": {
    "id": "guid",
    "name": "María García",
    "email": "maria@email.com",
    "lifeStage": "ActiveCycle",
    "language": "es"
  }
}
```

**Validaciones**

- `name`: required, max 255 chars.
- `email`: required, formato válido, único en DB.
- `password`: required, min 8 chars, debe contener al menos una letra y un número.
- `lifeStage`: required, valores válidos: `Adolescent | ActiveCycle | Pregnancy | Menopause`.
- `language`: required, código válido: `ES | NAH | MAY | MIX | ZAP | TSO | OTO`.
- `departmentId` + `municipalityId`: requeridos para usuarias en México.

**Errores comunes**

- `400 Bad Request` — validación fallida.
- `409 Conflict` — email ya registrado.

**Side effects**

- Envía email de verificación (SendGrid).
- Setea cookies `accessToken` (15 min) + `refreshToken` (30 días).

---

### POST `/api/v1/auth/login`

- **Auth**: No

**Request body**

```json
{
  "email": "maria@email.com",
  "password": "MiPassword123"
}
```

**Response 200 OK**

```json
{
  "message": "Inicio de sesión exitoso",
  "user": {
    "id": "guid",
    "name": "María García",
    "email": "maria@email.com",
    "lifeStage": "ActiveCycle",
    "language": "es"
  }
}
```

**Errores comunes**

- `400 Bad Request` — credenciales vacías.
- `401 Unauthorized` — credenciales inválidas o cuenta inactiva.
- `403 Forbidden` — email aún no verificado.

**Side effects**

- Crea registro en `sessions` (token persistente hasta logout).
- Setea cookies `accessToken` + `refreshToken`.
- Registra evento en `login_logs` (auditoría).

---

### POST `/api/v1/auth/refresh`

- **Auth**: No (pero requiere cookie `refreshToken` válida y no expirada)

**Request body**: ninguno.

**Response 200 OK**

```json
{
  "message": "Tokens renovados"
}
```

**Errores comunes**

- `401 Unauthorized` — refresh token inválido, expirado o ya revocado.

**Side effects**

- Rota el `refreshToken` (revoca el anterior, crea uno nuevo).
- Renueva la cookie `accessToken`.

---

### POST `/api/v1/auth/logout`

- **Auth**: Sí

**Request body**: ninguno.

**Response 200 OK**

```json
{
  "message": "Sesión cerrada exitosamente"
}
```

**Side effects**

- Marca la `Session` actual como revocada en DB.
- Limpia cookies `accessToken` + `refreshToken` (Max-Age=0).

---

### POST `/api/v1/auth/verify-email`

- **Auth**: No

**Request body**

```json
{
  "token": "string-uuid-or-nanoid-enviado-por-email"
}
```

**Response 200 OK**

```json
{
  "message": "Email verificado exitosamente"
}
```

**Errores comunes**

- `400 Bad Request` — token inválido o expirado.
- `404 Not Found` — token no existe.

---

### POST `/api/v1/auth/resend-verification`

- **Auth**: No

**Request body**

```json
{
  "email": "maria@email.com"
}
```

**Response 202 Accepted**

```json
{
  "message": "Si el email existe, se envió un nuevo correo de verificación"
}
```

> Nota: la respuesta es intencionalmente genérica para evitar **enumeración de usuarios**.

---

### POST `/api/v1/auth/forgot-password`

- **Auth**: No

**Request body**

```json
{
  "email": "maria@email.com"
}
```

**Response 202 Accepted**

```json
{
  "message": "Si el email existe, se enviaron instrucciones de recuperación"
}
```

**Side effects**

- Crea registro en `verifications` con token de un solo uso (15 min de expiración).
- Envía email con link de recuperación (SendGrid).

---

### POST `/api/v1/auth/reset-password`

- **Auth**: No

**Request body**

```json
{
  "token": "string-recibido-por-email",
  "newPassword": "OtroPassword456"
}
```

**Response 200 OK**

```json
{
  "message": "Contraseña actualizada exitosamente"
}
```

**Validaciones**

- `newPassword`: min 8 chars, debe contener al menos una letra y un número.

**Errores comunes**

- `400 Bad Request` — token inválido, expirado o ya consumido; password inválido.

**Side effects**

- Actualiza el hash BCrypt en `accounts.password`.
- Invalida el token en `verifications`.

---

### POST `/api/v1/auth/biometric`

- **Auth**: Sí (registra la clave biométrica de la usuaria ya autenticada)

**Request body**

```json
{
  "publicKey": "string-base64-from-expo-local-authentication",
  "deviceId": "string-uuid-del-dispositivo"
}
```

**Response 201 Created**

```json
{
  "message": "Biométrica registrada exitosamente",
  "credentialId": "guid"
}
```

**Errores comunes**

- `400 Bad Request` — `publicKey` inválido o vacío.
- `409 Conflict` — ya existe una credencial biométrica para ese `deviceId`.
