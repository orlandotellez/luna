# 13. Exportación de Reporte

**Descripción**: Una usuaria exporta su reporte mensual de salud en PDF para compartir con su médico.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `health_reports`, `cycles`, `cycle_days`, `symptoms`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Perfil → "Reporte de Salud"
    U->>F: Selecciona período: "Junio 2026"
    F->>B: GET /reports/monthly?year=2026&month=6
    B->>DB: Obtiene datos del ciclo, síntomas, estado de ánimo del mes
    B-->>F: { summary, chartData, symptoms, trends }

    F->>U: Muestra resumen: ciclo, síntomas frecuentes, ánimo predominante
    F->>U: Muestra gráficos de tendencia

    U->>F: Toca "Exportar PDF"
    F->>B: GET /reports/export/pdf?type=monthly&month=6&year=2026
    B->>B: Genera PDF con datos del reporte
    B->>DB: INSERT INTO health_reports (user_id, type, period_start, period_end, pdf_url)
    B-->>F: { pdfUrl }

    F->>F: Descarga PDF (expo-file-system)
    F->>U: "PDF descargado"
    U->>F: Toca "Compartir" → Opciones nativas de compartir
```
