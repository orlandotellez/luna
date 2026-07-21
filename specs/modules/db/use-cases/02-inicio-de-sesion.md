# 2. Inicio de Sesión

**Descripción**: Una usuaria existente inicia sesión con email/password o mediante autenticación biométrica.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `users`, `accounts`, `sessions`, `login_logs`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend (RN)
    participant B as Backend (API)
    participant DB as PostgreSQL

    alt Acceso biométrico
        U->>F: Abre app, Face ID / Huella
        F->>F: Verifica biometría local (expo-local-authentication)
        F->>B: POST /auth/biometric { token }
        B->>DB: Verifica token biométrico
    else Email + Password
        U->>F: Ingresa email + password
        F->>B: POST /auth/login { email, password }
        B->>DB: Verifica credenciales
    end

    alt Credenciales inválidas
        B->>DB: INSERT INTO login_logs (email, success=false, failure_reason)
        B-->>F: 401 Error
        F-->>U: "Credenciales incorrectas"
    else Credenciales válidas
        B->>DB: INSERT INTO sessions (user_id, token, expires_at)
        B->>DB: INSERT INTO login_logs (user_id, success=true)
        B->>DB: UPDATE users SET last_seen_at = NOW()
        B-->>F: 200 + Set cookies
        F->>U: Redirige a Dashboard
    end
```
