# 05 · Menopause — Menopausia y perimenopausia

Endpoints para gestión de la etapa de menopausia: registro, síntomas, recomendaciones, salud ósea y terapia hormonal.

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/menopause/register` | Sí | Activar etapa Menopause |
| GET | `/menopause/current` | Sí | Resumen de la menopause actual |
| POST | `/menopause/symptoms` | Sí | Registrar lote de síntomas del día |
| GET | `/menopause/symptoms/history` | Sí | Historial de síntomas registrados |
| GET | `/menopause/symptoms/chart` | Sí | Datos agregados para gráficos de evolución |
| GET | `/menopause/recommendations` | Sí | Recomendaciones personalizadas (motor de contenido) |
| GET | `/menopause/bone-health` | Sí | Salud ósea (última densitometría) |
| PUT | `/menopause/bone-health` | Sí | Actualizar registro de salud ósea |
| POST | `/menopause/therapy` | Sí | Registrar terapia hormonal sustitutiva |

---

## Detalle de endpoints

### POST `/api/v1/menopause/register`

- **Auth**: Sí

**Request body**

```json
{
  "lastPeriodDate": "2025-09-15",
  "monthsWithoutPeriod": 9,
  "symptomStartAge": 47
}
```

**Response 201 Created**

```json
{
  "menopauseId": "guid",
  "stage": "perimenopause",
  "isInMenopause": false,
  "monthsWithoutPeriod": 9,
  "message": "Etapa de menopausia iniciada"
}
```

> `stage`: `perimenopause | menopause | postmenopause`. Se deriva de `monthsWithoutPeriod` (≥12 = menopause).

**Side effects** — setea `users.lifeStage = "Menopause"`.

---

### GET `/api/v1/menopause/current`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "menopauseId": "guid",
  "stage": "perimenopause",
  "lastPeriodDate": "2025-09-15",
  "monthsWithoutPeriod": 9,
  "isInMenopause": false,
  "symptomStartAge": 47
}
```

---

### POST `/api/v1/menopause/symptoms`

- **Auth**: Sí

**Request body**

```json
{
  "date": "2026-06-12",
  "symptoms": [
    { "type": "HotFlash", "intensity": 4, "frequency": 5, "notes": "Comenzaron después del almuerzo" },
    { "type": "NightSweats", "intensity": 3, "frequency": 2 },
    { "type": "Insomnia", "intensity": 2, "sleepHours": 5 }
  ]
}
```

Valores de `type`: `HotFlash | NightSweats | VaginalDryness | Insomnia | MoodSwings | WeightGain | JointPain | MemoryLoss | HairLoss | LibidoDecrease`.

**Response 201 Created**

```json
{
  "logged": [
    { "id": "guid", "type": "HotFlash", "intensity": 4, "frequency": 5 },
    { "id": "guid", "type": "NightSweats", "intensity": 3, "frequency": 2 },
    { "id": "guid", "type": "Insomnia", "intensity": 2, "sleepHours": 5 }
  ],
  "message": "3 síntomas registrados exitosamente"
}
```

---

### GET `/api/v1/menopause/symptoms/history`

- **Auth**: Sí

**Query params opcionales**: `?from=2026-01-01&to=2026-06-30&type=HotFlash&limit=50`.

**Response 200 OK**

```json
{
  "logs": [
    {
      "id": "guid",
      "date": "2026-06-12",
      "type": "HotFlash",
      "intensity": 4,
      "frequency": 5,
      "notes": "Comenzaron después del almuerzo"
    }
  ]
}
```

---

### GET `/api/v1/menopause/symptoms/chart`

- **Auth**: Sí

**Query params opcionales**: `?period=6m&type=HotFlash`.

**Response 200 OK**

```json
{
  "type": "HotFlash",
  "period": "6m",
  "dataPoints": [
    { "month": "2026-01", "averageIntensity": 3.2, "averageFrequency": 4.0, "count": 18 },
    { "month": "2026-02", "averageIntensity": 3.5, "averageFrequency": 4.5, "count": 22 }
  ]
}
```

---

### GET `/api/v1/menopause/recommendations`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "recommendations": [
    {
      "id": "guid",
      "title": "Ejercicios de peso para fortalecer huesos",
      "description": "30 min de caminata 3 veces por semana...",
      "articleSlug": "ejercicio-menopausia-huesos"
    }
  ]
}
```

> Las recomendaciones se generan según sintomatología + salud ósea + perfil de salud general vía `RecommendationEngine`. La forma final puede ampliarse cuando se implemente el endpoint.

---

### GET `/api/v1/menopause/bone-health`

- **Auth**: Sí

**Response 200 OK** (404 si aún no hay registro)

```json
{
  "lastDensitometryDate": "2026-04-15",
  "tScore": -1.2,
  "zScore": -0.5,
  "category": "osteopenia"
}
```

> `category`: `normal | osteopenia | osteoporosis`.

---

### PUT `/api/v1/menopause/bone-health`

- **Auth**: Sí

**Request body**

```json
{
  "densitometryDate": "2026-04-15",
  "tScore": -1.2,
  "zScore": -0.5,
  "location": "lumbar",
  "notes": "Primera densitometría postmenopausia"
}
```

**Response 200 OK** — mismo shape que GET.

---

### POST `/api/v1/menopause/therapy`

- **Auth**: Sí

**Request body**

```json
{
  "type": "HRT",
  "startDate": "2026-06-01",
  "dosage": "Estradiol 1mg diario",
  "doctor": "Dra. Hernández",
  "notes": "Seguimiento cada 3 meses"
}
```

Valores de `type`: `HRT | LocalEstrogen | NonHormonal | Herbal`.

**Response 201 Created**

```json
{
  "id": "guid",
  "type": "HRT",
  "startDate": "2026-06-01",
  "isActive": true,
  "createdAt": "2026-06-01T11:00:00Z"
}
```
