# 8. Registro de Síntomas Menopáusicos

**Descripción**: Una usuaria en etapa de menopausia registra sus síntomas con intensidad y frecuencia.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `menopause_tracking`, `menopause_symptom_logs`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → "Registrar síntoma"
    F->>U: Muestra grid de síntomas menopáusicos
    U->>F: Selecciona "Sofoco" → Intensidad 4/5 → Frecuencia: 5/día
    U->>F: Selecciona "Sudores nocturnos" → Intensidad 3/5
    U->>F: Agrega nota: "Comenzaron después del almuerzo"
    F->>B: POST /menopause/symptoms { date, symptoms[] }
    B->>DB: INSERT INTO menopause_symptom_logs (user_id, menopause_id, date, type, intensity, frequency_per_day)
    B-->>F: 200

    B->>B: Actualiza gráfico de evolución
    alt Patrón detectado: sofocos más intensos en la tarde
        B-->>F: 200 + articleRecommendation: "Manejo de sofocos"
        F->>U: Muestra artículo recomendado
    end
```
