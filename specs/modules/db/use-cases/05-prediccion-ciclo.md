# 5. Predicción de Ciclo

**Descripción**: El sistema calcula automáticamente la próxima menstruación, ventana fértil y ovulación basado en el historial.

**Actores**: Sistema (automático), Usuaria (consulta)

**Tablas involucradas**: `cycles`, `cycle_days`

```mermaid
sequenceDiagram
    participant S as CyclePredictor
    participant DB as PostgreSQL
    participant B as Backend (API)
    participant F as Frontend

    Note over S: Trigger: nuevo ciclo registrado
    S->>DB: SELECT * FROM cycles WHERE user_id = ? ORDER BY cycle_number DESC LIMIT 6
    S->>S: Calcula promedio de duración de ciclo
    S->>S: Calcula promedio de duración de período
    S->>S: Estima ventana fértil (método del calendario)
    S->>S: Calcula nivel de confianza según regularidad
    S->>DB: Guarda predicciones (en cache o como datos calculados)
    S-->>B: Predicción actualizada

    Note over B,F: Usuaria consulta
    U as Usuaria
    U->>F: Abre calendario / dashboard
    F->>B: GET /cycle/predictions
    B-->>F: { nextPeriodDate, fertileWindow, ovulationDate, confidence }

    Note over F: Renderiza calendario con colores
    F->>U: 🔴 Período esperado | 🟢 Ventana fértil | 💧 Ovulación
```
