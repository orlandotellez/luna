# 1. Registro y Onboarding

**Descripción**: Una nueva usuaria se registra, selecciona su etapa de vida y configura su perfil inicial de salud.

**Actores**: Usuaria no autenticada, Sistema

**Tablas involucradas**: `users`, `accounts`, `sessions`, `health_profiles`, `verifications`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend (RN)
    participant B as Backend (API)
    participant DB as PostgreSQL
    participant M as Mail Service

    U->>F: Abre app por primera vez
    F->>U: Slider de bienvenida (3 etapas)
    U->>F: Selecciona etapa de vida
    U->>F: Completa formulario de registro
    F->>B: POST /auth/register
    B->>DB: Verifica email no existe
    alt Email ya registrado
        B-->>F: 409 Email en uso
        F-->>U: Muestra error
    else Email disponible
        B->>DB: INSERT INTO users (name, email, life_stage, language)
        B->>DB: INSERT INTO accounts (user_id, provider_id='credentials', password=hash)
        B->>DB: INSERT INTO health_profiles (user_id, ...initial_data)
        B->>B: Genera token de verificación
        B->>DB: INSERT INTO verifications (identifier=email, value=token, expires_at)
        B->>M: Envía email de verificación
        B-->>F: 201 + Set cookies (accessToken, refreshToken)
        F->>U: Redirige a Dashboard personalizado
    end
```
