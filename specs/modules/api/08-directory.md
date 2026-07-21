# 08 · Directory — Directorio de servicios de salud

Endpoints para buscar profesionales de la salud, centros y líneas de emergencia. La mayoría son públicos para facilitar el acceso.

## Tabla de endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/directory/professionals` | No | Buscar profesionales con filtros |
| GET | `/directory/professionals/{id}` | No | Perfil público de un profesional |
| GET | `/directory/centers` | No | Centros de salud |
| GET | `/directory/emergency` | No | Líneas de emergencia |
| POST | `/directory/professionals` | Admin | Registrar nuevo profesional |
| PUT | `/directory/professionals/{id}` | Admin | Actualizar profesional existente |

---

## Detalle de endpoints

### GET `/api/v1/directory/professionals`

- **Auth**: No

**Query params**

| Param | Type | Required | Description |
|-------|------|----------|-------------|
| `specialty` | string | No | `Ginecología | Obstetricia | Pediatría | Endocrinología | Psicología | Nutrición` |
| `departmentId` | string | No | Código departamento |
| `municipalityId` | string | No | Código municipio |
| `language` | string | No | Idioma que habla el profesional |
| `latitude` | decimal | No | Coordenada para búsqueda por cercanía |
| `longitude` | decimal | No | Coordenada para búsqueda por cercanía |
| `radius` | int | No | Radio en km (default 10) |
| `page` | int | No | Default `1` |
| `limit` | int | No | Default `20` |

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "fullName": "Dra. María Pérez",
      "specialty": "Ginecología",
      "credentials": "Ginecóloga Obstetra UNAM",
      "languages": ["es", "nah"],
      "department": "Jalisco",
      "municipality": "Guadalajara",
      "address": "Av. Federalismo 1234",
      "phone": "+523312345678",
      "email": "contacto@draperez.example",
      "acceptsInsurance": true,
      "rating": 4.8,
      "distanceKm": 2.3
    }
  ],
  "page": 1,
  "limit": 20,
  "total": 145
}
```

---

### GET `/api/v1/directory/professionals/{id}`

- **Auth**: No

**Response 200 OK**

```json
{
  "id": "guid",
  "fullName": "Dra. María Pérez",
  "specialty": "Ginecología",
  "credentials": "...",
  "biography": "20 años de experiencia en...",
  "languages": ["es", "nah"],
  "address": "Av. Federalismo 1234",
  "phone": "+523312345678",
  "email": "...",
  "schedule": [
    { "dayOfWeek": 1, "from": "09:00", "to": "14:00" },
    { "dayOfWeek": 3, "from": "09:00", "to": "14:00" }
  ],
  "acceptsInsurance": true,
  "rating": 4.8,
  "reviewsCount": 87
}
```

---

### GET `/api/v1/directory/centers`

- **Auth**: No

**Query params**: `?departmentId=14&municipalityId=001&type=Hospital|HealthCenter|Clinic&page=1&limit=20`.

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "name": "Centro de Salud #14",
      "type": "HealthCenter",
      "address": "Av. Reforma 100",
      "department": "Jalisco",
      "municipality": "Guadalajara",
      "phone": "+523312345678",
      "hasGynecology": true,
      "hasObstetrics": true,
      "hasEmergency": false,
      "languagesSupported": ["es"]
    }
  ]
}
```

---

### GET `/api/v1/directory/emergency`

- **Auth**: No

**Response 200 OK**

```json
{
  "lines": [
    {
      "id": "guid",
      "name": "Línea de la Vida",
      "phone": "800-911-2000",
      "description": "Orientación sobre violencia de género, 24/7",
      "languagesSupported": ["es"],
      "coverage": "Nacional"
    },
    {
      "id": "guid",
      "name": "SAPTEL — Crisis emocional",
      "phone": "555-525-4012",
      "description": "Apoyo psicológico en crisis, 24/7",
      "languagesSupported": ["es"]
    }
  ]
}
```

---

### POST `/api/v1/directory/professionals`

- **Auth**: Admin (`RequirePermission(Permissions.DirectoryManage)`)

**Request body**

```json
{
  "fullName": "Dra. María Pérez",
  "specialty": "Ginecología",
  "credentials": "Ginecóloga Obstetra UNAM",
  "languages": ["es", "nah"],
  "phone": "+523312345678",
  "email": "contacto@draperez.example",
  "departmentId": "14",
  "municipalityId": "001",
  "address": "Av. Federalismo 1234",
  "biography": "20 años de experiencia..."
}
```

**Response 201 Created** — `ProfessionalDto` completo con `id` generado.

---

### PUT `/api/v1/directory/professionals/{id}`

- **Auth**: Admin

**Request body** — campos opcionales para actualización parcial.

**Response 200 OK** — `ProfessionalDto` actualizado.
