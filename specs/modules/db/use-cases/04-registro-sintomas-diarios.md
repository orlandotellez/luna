# 4. Registro de Síntomas Diarios

**Descripción**: Una usuaria registra síntomas, estado de ánimo y calidad del sueño en un día específico.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `cycle_days`, `symptoms`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → "Registrar síntoma"
    F->>U: Muestra selector de síntomas con sliders
    U->>F: Selecciona síntomas e intensidades
    U->>F: Selecciona estado de ánimo
    U->>F: Registra calidad del sueño
    U->>F: Escribe nota personal (opcional)
    F->>B: POST /cycle/symptoms { date, symptoms[], mood, sleepQuality, notes }
    B->>DB: UPSERT cycle_days (user_id, date)
    B->>DB: DELETE + INSERT symptoms para este día (reemplaza anteriores)
    B-->>F: 200
    F->>U: "Síntomas guardados"

    note over B: Sistema detecta patrón de síntomas
    B->>B: RecommendationEngine evalúa síntomas vs histórico
    alt Patrón detectado
        B-->>F: 200 + recommendedArticle (opcional)
        F->>U: Muestra recomendación de artículo
    end
```
