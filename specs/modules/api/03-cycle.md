# 03 · Cycle — Ciclo menstrual

Endpoints para tracking diario del ciclo menstrual: estado actual, registro de período y síntomas, vista de calendario, historial, estadísticas y predicciones.

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/cycle/current` | Sí | Ciclo actual + predicciones mínimas según perfil de salud |
| POST | `/cycle/period` | Sí | Registrar inicio / fin de período menstrual |
| POST | `/cycle/symptoms` | Sí | Registrar síntomas del día (uno o varios) |
| GET | `/cycle/calendar` | Sí | Datos del calendario mensual (con fase y predicciones por día) |
| GET | `/cycle/history` | Sí | Historial de ciclos con métricas calculadas |
| GET | `/cycle/stats` | Sí | Estadísticas agregadas y tendencias |
| GET | `/cycle/predictions` | Sí | Predicciones actualizadas (día en curso + próximo período) |

---

## Detalle de endpoints

### GET `/api/v1/cycle/current`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "lifeStage": "ActiveCycle",
  "cycleLengthDays": 28,
  "periodLengthDays": 5,
  "hasRegularCycle": true,
  "activePregnancy": null,
  "predictions": {
    "currentPhase": "regular_cycle_configured",
    "dayOfCycle": null,
    "nextPeriodStart": null,
    "nextPeriodEnd": null,
    "fertileWindowStart": null,
    "fertileWindowEnd": null,
    "ovulationDate": null,
    "daysUntilNextPeriod": null
  }
}
```

> Si la usuaria tiene embarazo activo, `lifeStage` cambia a `"Pregnancy"` y `activePregnancy` se popula con `PregnancyDto`.

---

### POST `/api/v1/cycle/period`

- **Auth**: Sí

**Request body**

```json
{
  "startDate": "2026-06-10",
  "endDate": "2026-06-14",
  "flowIntensity": "Moderate",
  "notes": "Flujo normal"
}
```

Valores de `flowIntensity`: `Light | Moderate | Heavy`.

**Response 201 Created** — apunta a `/api/v1/cycle/period/{id}`

```json
{
  "id": "guid",
  "startDate": "2026-06-10",
  "endDate": "2026-06-14",
  "notes": "Flujo normal",
  "createdAt": "2026-06-10T07:30:00Z"
}
```

**Validaciones**

- `startDate`: requerido (`DateOnly`).
- `endDate`: opcional; si se envía, debe ser `>= startDate`.
- `flowIntensity`: opcional; default `Moderate`.

**Errores comunes**

- `400 Bad Request` — `endDate < startDate`.

---

### POST `/api/v1/cycle/symptoms`

- **Auth**: Sí

**Request body**

```json
{
  "date": "2026-06-12",
  "symptoms": [
    { "type": "Cramps", "intensity": 3 },
    { "type": "Headache", "intensity": 2 },
    { "type": "Bloating", "intensity": 1 }
  ],
  "mood": "Irritable",
  "sleepQuality": 3,
  "notes": "Dolor fuerte por la mañana"
}
```

Valores de `symptoms[].type`: `Cramps | Headache | Bloating | MoodSwings | Fatigue | Nausea | BreastTenderness | BackPain | Cravings | Insomnia | Acne | Spotting`.
Valores de `symptoms[].intensity`: `1..5`.
Valores de `mood`: `Happy | Normal | Sad | Anxious | Irritable | Calm | Energetic`.
`sleepQuality`: `1..5`.

**Response 201 Created** — apunta a `/api/v1/cycle/symptoms/{id}`

```json
{
  "id": "guid",
  "date": "2026-06-12",
  "symptom": "Cramps",
  "severity": 3,
  "notes": "Dolor fuerte por la mañana",
  "createdAt": "2026-06-12T09:15:00Z"
}
```

> Cada síntoma del array crea un `SymptomEntry` independiente.

---

### GET `/api/v1/cycle/calendar`

- **Auth**: Sí

**Query params**

| Param | Type | Required | Range |
|-------|------|----------|-------|
| `month` | `int` | Sí | `1..12` |
| `year` | `int` | Sí | `1900..2100` |

**Ejemplo**: `GET /api/v1/cycle/calendar?month=6&year=2026`

**Response 200 OK**

```json
{
  "userId": "guid",
  "month": 6,
  "year": 2026,
  "startDate": "2026-06-01",
  "endDate": "2026-06-30",
  "lifeStage": "ActiveCycle",
  "days": [
    {
      "date": "2026-06-10",
      "dayOfCycle": 1,
      "phase": "Menstrual",
      "isPeriodDay": true,
      "isPredictedPeriod": false,
      "isFertileWindow": false,
      "isOvulationDay": false,
      "symptoms": []
    }
  ]
}
```

**Validaciones**

- `month` y `year` validados; fuera de rango → `400`.

**Algoritmos**

- **Fase** basada en `dayOfCycle` y `periodLength`:
  - `dayOfCycle ≤ periodLength` → `Menstrual`.
  - `≤ cycleLength - 16` → `Follicular`.
  - `≤ cycleLength - 13` → `Ovulatory`.
  - resto → `Luteal`.
- **Ventana fértil** = `cycleLength - 14 ± 5 días`.
- `cycleLength` por defecto `28` (o el de `HealthProfile.CycleLengthDays`).

---

### GET `/api/v1/cycle/history`

- **Auth**: Sí

**Query params**

| Param | Type | Required | Default | Range |
|-------|------|----------|---------|-------|
| `limit` | `int` | No | `12` | `1..100` |

**Response 200 OK**

```json
{
  "userId": "guid",
  "lifeStage": "ActiveCycle",
  "totalCycles": 4,
  "averageCycleLengthDays": 29.0,
  "averagePeriodLengthDays": 5.0,
  "cycles": [
    {
      "id": "guid",
      "cycleNumber": 1,
      "startDate": "2025-09-10",
      "endDate": "2025-09-14",
      "periodLengthDays": 5,
      "cycleLengthDays": 28,
      "notes": null,
      "createdAt": "2025-09-10T08:00:00Z"
    }
  ]
}
```

**Algoritmos**

- `cycleNumber` = posición ascendente desde el más antiguo.
- `cycleLengthDays` = `current.startDate - previous.startDate` (para ciclos a partir del segundo).
- Orden en `cycles[]` = descendente (más reciente primero).
- `lifeStage = "Pregnancy"` si hay embarazo activo.

**Errores comunes**

- `400 Bad Request` — `limit` fuera de `1..100`.

---

### GET `/api/v1/cycle/stats`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "userId": "guid",
  "lifeStage": "ActiveCycle",
  "totalCyclesTracked": 4,
  "cyclesAnalyzed": 3,
  "insufficientData": false,
  "regularity": {
    "isRegular": true,
    "regularityScore": 85,
    "averageCycleLengthDays": 29.0,
    "medianCycleLengthDays": 29,
    "minCycleLengthDays": 27,
    "maxCycleLengthDays": 31,
    "stdDeviationDays": 1.6
  },
  "periodLength": {
    "averagePeriodLengthDays": 5.0,
    "minPeriodLengthDays": 4,
    "maxPeriodLengthDays": 6
  },
  "trends": {
    "cycleLengthTrend": "stable",
    "periodLengthTrend": "stable",
    "cycleLengthChangeLastCycles": 0.5,
    "periodLengthChangeLastCycles": -0.3
  },
  "lastCycle": {
    "id": "guid",
    "startDate": "2026-06-10",
    "endDate": "2026-06-14",
    "daysSinceStart": 12,
    "isInProgress": false
  },
  "topSymptoms": [
    { "symptom": "Cramps", "occurrences": 8, "averageSeverity": 2.5 }
  ],
  "predictionBaseline": {
    "nextPeriodEstimatedStart": "2026-07-08",
    "nextPeriodEstimatedEnd": "2026-07-12",
    "fertilityWindowStart": "2026-06-24",
    "fertilityWindowEnd": "2026-06-30",
    "ovulationDate": "2026-06-29",
    "confidence": "high"
  }
}
```

**Algoritmos**

- `regularityScore`: `% de ciclos dentro de ±3 días del promedio`.
- `isRegular`: `cyclesAnalyzed ≥ 3 && score ≥ 80`.
- `trend`: comparar avg de primera mitad vs segunda (≥4 ciclos para calcular).
- `predictionBaseline.confidence`: `high ≥ 6`, `medium ≥ 3`, `low < 3`.
- Si `lifeStage = Pregnancy` → todo en null con `insufficientData = true`.

---

### GET `/api/v1/cycle/predictions`

- **Auth**: Sí

**Response 200 OK**

```json
{
  "userId": "guid",
  "lifeStage": "ActiveCycle",
  "lifeStageState": "active_cycle",
  "isPredictionApplicable": true,
  "prediction": {
    "currentPhase": "ovulatory",
    "dayOfCycle": 15,
    "nextPeriodStart": "2026-07-08",
    "nextPeriodEnd": "2026-07-12",
    "fertileWindowStart": "2026-06-24",
    "fertileWindowEnd": "2026-06-30",
    "ovulationDate": "2026-06-29",
    "daysUntilNextPeriod": 18
  },
  "metadata": {
    "confidence": "high",
    "source": "config",
    "assumedCycleLength": 28,
    "assumedPeriodLength": 5,
    "cyclesAnalyzed": 0,
    "isAnomaly": false
  }
}
```

**Algoritmos**

**Estrategia de fuente de verdad** (en orden de prioridad):
1. **HealthProfile config** — si `HasRegularCycle = true && CycleLengthDays` configurado → `source = "config"`, `confidence = "high"`.
2. **Histórica (mediana)** — si no hay config pero ≥3 ciclos finalizados → mediana de los últimos 6 ciclos, `source = "historical_median"`.
3. **Default** — fallback a 28 / 5, `source = "default"`, `confidence = "low"`.

**`CurrentPhase` valores**

| Valor | Condición |
|-------|-----------|
| `unknown` | sin períodos registrados |
| `menstrual` | `dayOfCycle ≤ periodLength` |
| `follicular` | `≤ cycleLength - 16` |
| `ovulatory` | `≤ cycleLength - 13` |
| `luteal` | `≤ cycleLength` |
| `late` | `> cycleLength` |

> ⚠️ **Importante:** `GET /cycle/current` usa otros valores para `currentPhase`: solo emite `"regular_cycle_configured"` (cuando la usuaria tiene `HealthProfile` con `HasRegularCycle = true`) o `"unknown"` (cuando no). Las fases reales (`menstrual | follicular | ovulatory | luteal | late`) SOLO las devuelve `GET /cycle/predictions`.

**`daysUntilNextPeriod`**
- Positivo: días faltantes.
- **Negativo**: días de atraso (ej. `-5` = atrasada 5 días).
- `null` si no hay ningún período registrado.

**`isAnomaly = true`**
- Si `dayOfCycle > cycleLength + 14` (heurística: atraso significativo → frontend puede sugerir "¿olvidaste registrar?").

**`lifeStage` especiales**

Cuando `lifeStage = Pregnancy` o `Menopause` → `isPredictionApplicable = false`, `prediction = null`, `metadata = null`, pero el endpoint retorna `200 OK` con información contextual:

```json
{
  "userId": "guid",
  "lifeStage": "Pregnancy",
  "lifeStageState": "pregnancy",
  "isPredictionApplicable": false,
  "prediction": null,
  "metadata": null
}
```
