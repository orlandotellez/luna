# 15. Cambio de Etapa de Vida

**Descripción**: Una usuaria actualiza su etapa de vida (ej: de ciclo activo a embarazo, o a menopausia).

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `users`, `health_profiles`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Perfil → "Mi etapa" → "Cambiar etapa"
    F->>B: GET /users/me
    B-->>F: Etapa actual: ActiveCycle

    F->>U: Muestra selector de etapas disponibles
    U->>F: Selecciona "Embarazo"
    F->>U: Pregunta: Fecha de última menstruación (FUM)
    U->>F: Ingresa FUM
    F->>B: PUT /users/me/life-stage { lifeStage: "PREGNANCY", lastMenstrualPeriod: "2026-01-15" }
    B->>DB: UPDATE users SET life_stage = 'PREGNANCY'
    B->>B: Calcula FPP y semana gestacional
    B->>DB: INSERT INTO pregnancies (user_id, last_menstrual_period, estimated_due_date, current_week)
    B-->>F: 200 { newStage: "PREGNANCY", currentWeek: 21, estimatedDueDate: "2026-10-22" }

    F->>U: Dashboard transformado
    F->>U: Muestra información de semana actual
    F->>U: Sugiere invitar acompañante
    F->>U: Muestra checklist de primer trimestre

    Note over B: Los datos anteriores de ciclo menstrual se preservan
    Note over B: La usuaria puede volver a cambiar si es necesario
```
