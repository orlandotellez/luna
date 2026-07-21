# 7. Registro de Movimientos Fetales

**Descripción**: Una usuaria registra una sesión de conteo de patadas para monitorear la actividad fetal.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `fetal_movements`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Embarazo → "Contador de patadas"
    F->>U: Muestra botón grande "TOCA CADA VEZ QUE SIENTAS UNA PATADA"
    U->>F: Toca el botón cada vez que siente una patada
    F->>F: Contador local + temporizador

    Note over U: Sesión de 30 minutos

    U->>F: Finaliza sesión
    F->>B: POST /pregnancy/kicks { startTime, kicks[], totalKicks, durationMinutes }
    B->>DB: INSERT INTO fetal_movements (user_id, pregnancy_id, start_time, total_kicks, duration_minutes)
    B-->>F: 200

    alt Si total_kicks < 10 en 2 horas (baja actividad)
        B-->>F: 200 + alert: "Notaste poca actividad. ¿Quieres contactar a tu médico?"
        F->>U: Muestra alerta + botón "Contactar médico"
    end

    B->>B: Calcula promedio de patadas/hora
    B-->>F: 200 { averageKicksPerHour, trend }
    F->>U: Muestra historial: "Últimos 7 días"
```
