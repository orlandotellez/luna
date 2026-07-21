# 3. Registro de Ciclo Menstrual

**Descripción**: Una usuaria registra el inicio de su período menstrual con intensidad de flujo y síntomas.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `cycles`, `cycle_days`, `symptoms`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Dashboard → "Registrar período"
    F->>B: GET /cycle/current
    B->>DB: Obtiene último ciclo registrado
    B-->>F: Último ciclo + predicciones

    U->>F: Selecciona fecha de inicio
    U->>F: Selecciona intensidad de flujo
    U->>F: Registra síntomas asociados
    U->>F: Agrega nota opcional
    F->>B: POST /cycle/period { startDate, flowIntensity, symptoms, notes }
    B->>DB: INSERT INTO cycles (user_id, start_date, flow_intensity)

    alt Si ya existe cycle_day para esta fecha
        B->>DB: UPDATE cycle_days SET flow_intensity = ?
    else No existe
        B->>DB: INSERT INTO cycle_days (user_id, cycle_id, date, day_of_cycle, flow_intensity)
    end

    B->>DB: INSERT INTO symptoms (user_id, cycle_day_id, type, intensity) para cada síntoma
    B->>B: Recalcula predicción del próximo ciclo
    B-->>F: 200 { cycle, predictions }
    F->>U: Calendario actualizado + predicción actualizada

    note over B: Si dolor intenso 3 ciclos seguidos
    B->>F: Sugerencia: "Consulta a tu ginecóloga"
```
