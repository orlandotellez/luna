# 12 · Reports — Reportes de salud

Endpoints para generar, visualizar y exportar reportes individuales de salud (resumen mensual, tendencias y exportación).

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/reports/monthly` | Sí | Resumen del mes (ciclo + síntomas + citas) |
| GET | `/reports/trends` | Sí | Tendencias agregadas (último período) |
| GET | `/reports/export/pdf` | Sí | Exportar reporte en PDF (descarga directa) |
| GET | `/reports/export/csv` | Sí | Exportar datos crudos en CSV (descarga directa) |

---

## Detalle de endpoints

### GET `/api/v1/reports/monthly`

- **Auth**: Sí

**Query params**

| Param | Type | Required | Default | Range |
|-------|------|----------|---------|-------|
| `year` | `int` | Sí | — | `2020..2100` |
| `month` | `int` | Sí | — | `1..12` |

**Response 200 OK**

```json
{
  "year": 2026,
  "month": 6,
  "lifeStage": "ActiveCycle",
  "cycleSummary": {
    "periodDays": 5,
    "cycleDayAtEndOfMonth": 23,
    "phasesCount": {
      "menstrual": 5,
      "follicular": 9,
      "ovulatory": 2,
      "luteal": 14
    }
  },
  "symptomsSummary": {
    "totalLogs": 18,
    "mostFrequent": [
      { "symptom": "Cramps", "count": 6, "averageIntensity": 2.8 }
    ]
  },
  "appointments": [
    {
      "title": "Ultrasonido morfológico",
      "scheduledAt": "2026-06-15T10:00:00Z",
      "attended": true
    }
  ],
  "moodAverage": "Anxious"
}
```

---

### GET `/api/v1/reports/trends`

- **Auth**: Sí

**Query params opcionales**: `?period=3m|6m|12m` (default `6m`).

**Response 200 OK**

```json
{
  "periodFrom": "2026-01-01",
  "periodTo": "2026-06-30",
  "cycleLengthTrend": [
    { "month": "2026-01", "average": 28.0 },
    { "month": "2026-02", "average": 28.5 },
    { "month": "2026-03", "average": 29.1 }
  ],
  "periodLengthTrend": [
    { "month": "2026-01", "average": 5.0 }
  ],
  "symptomFrequencyTrend": [
    { "month": "2026-01", "Cramps": 6, "Headache": 2 },
    { "month": "2026-02", "Cramps": 4, "Headache": 3 }
  ],
  "moodDistribution": {
    "Happy": 0.20,
    "Normal": 0.45,
    "Sad": 0.10,
    "Anxious": 0.20,
    "Irritable": 0.05
  }
}
```

---

### GET `/api/v1/reports/export/pdf`

- **Auth**: Sí

**Query params**: `?type=monthly&month=6&year=2026`.

**Response** — `Content-Type: application/pdf`, `Content-Disposition: attachment; filename="luna-report-2026-06.pdf"`.

> El backend genera el PDF en línea (o lo toma del cache/CDN si ya fue generado) y lo retorna como stream binario.

---

### GET `/api/v1/reports/export/csv`

- **Auth**: Sí

**Query params**: `?type=cycle&from=2026-01-01&to=2026-06-30`.

Valores de `type`: `cycle | symptoms | appointments | weight | all`.

**Response** — `Content-Type: text/csv`, `Content-Disposition: attachment; filename="luna-cycle-2026-01-01_to_2026-06-30.csv"`.

> Retorna CSV con una fila por registro. La primera línea es el header con los nombres de columnas.
