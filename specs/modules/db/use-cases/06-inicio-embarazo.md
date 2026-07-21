# 6. Inicio de Embarazo

**Descripción**: Una usuaria cambia su etapa a embarazo, ingresa su FUM y el sistema calcula la FPP y semana gestacional.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `pregnancies`, `appointments`, `users`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Perfil → "Cambiar etapa" → "Embarazo"
    F->>B: GET /users/me
    B-->>F: Etapa actual: ActiveCycle

    U->>F: Ingresa fecha de última menstruación (FUM)
    F->>B: POST /pregnancy/register { lastMenstrualPeriod, isFirstPregnancy }
    B->>B: Calcula FPP (regla de Naegele: FUM + 280 días)
    B->>B: Calcula semana gestacional actual
    B->>DB: INSERT INTO pregnancies (user_id, last_menstrual_period, estimated_due_date, current_week)
    B->>DB: UPDATE users SET life_stage = 'PREGNANCY'
    B-->>F: 201 { pregnancyId, currentWeek, estimatedDueDate }

    F->>U: Dashboard transformado a vista de embarazo
    F->>U: Muestra "Semana X" con información del desarrollo
    F->>U: Checklist primer trimestre: "Agenda tu primera ecografía"
    F->>U: Sugerencia: "Invita a tu pareja como acompañante"
```
