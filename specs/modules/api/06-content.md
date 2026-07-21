# 06 · Content — Artículos, mitos y glosario

Endpoints públicos para navegar contenido educativo. Los artículos recomendados sí requieren autenticación (para personalizar según perfil).

## Tabla de endpoints

### Articles

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/articles` | No | Listar artículos con filtros y paginación |
| GET | `/articles/{slug}` | No | Detalle de artículo |
| GET | `/articles/featured` | No | Artículos destacados |
| GET | `/articles/recommended` | Sí | Recomendados según perfil de la usuaria |
| GET | `/articles/by-stage/{stage}` | No | Filtrar por etapa de vida |
| GET | `/articles/search` | No | Búsqueda textual + idioma |

### Myths

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/myths` | No | Listar mitos (paginados) |
| GET | `/myths/{id}` | No | Detalle de mito |
| GET | `/myths/categories` | No | Categorías de mitos |

### Glossary

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/glossary` | No | Listar tévariantes |
| GET | `/glossary/{id}` | No | Detalle de término |
| GET | `/glossary/search` | No | Búsqueda de términos |

---

## Detalle de endpoints

### Articles

#### GET `/api/v1/articles`

- **Auth**: No

**Query params**

| Param | Type | Required | Description |
|-------|------|----------|-------------|
| `category` | string | No | `Cycle | Pregnancy | Menopause | General | Myths | Nutrition | Exercise | MentalHealth` |
| `stage` | string | No | `Adolescent | ActiveCycle | Pregnancy | Menopause` |
| `language` | string | No | `es | nah | may | mix | zap | tso | oto` |
| `search` | string | No | Texto libre (full-text search) |
| `page` | int | No | Default `1` |
| `limit` | int | No | Default `20`, max `100` |

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "slug": "que-es-la-menopausia",
      "title": "¿Qué es la menopausia?",
      "excerpt": "Una guía introductoria...",
      "category": "Menopause",
      "language": "es",
      "coverImage": "https://cdn.luna.app/articles/uuid.jpg",
      "readTimeMinutes": 5,
      "publishedAt": "2026-01-15T00:00:00Z",
      "authorName": "Dra. María Pérez"
    }
  ],
  "page": 1,
  "limit": 20,
  "total": 47,
  "hasMore": true
}
```

---

#### GET `/api/v1/articles/{slug}`

- **Auth**: No

**Response 200 OK**

```json
{
  "id": "guid",
  "slug": "que-es-la-menopausia",
  "title": "¿Qué es la menopausia?",
  "content": "<p>HTML con el contenido completo</p>",
  "category": "Menopause",
  "language": "es",
  "tags": ["salud", "mujeres", "50+"],
  "readTimeMinutes": 5,
  "publishedAt": "2026-01-15T00:00:00Z",
  "author": {
    "name": "Dra. María Pérez",
    "credentials": "Ginecóloga, UNAM"
  },
  "relatedArticles": ["slug-2", "slug-3"]
}
```

**Errores comunes**

- `404 Not Found` — slug inexistente.

---

#### GET `/api/v1/articles/featured`

- **Auth**: No

**Response 200 OK**

```json
{
  "items": [/* ArticleSummaryDto[] — máximo 6 elementos */]
}
```

---

#### GET `/api/v1/articles/recommended`

- **Auth**: Sí

**Response 200 OK** — máximo 6 artículos personalizados según el `lifeStage`, síntomas recientes e idioma preferido del usuario.

```json
{
  "items": [/* ArticleSummaryDto[] */]
}
```

---

#### GET `/api/v1/articles/by-stage/{stage}`

- **Auth**: No

**Path params**: `stage` ∈ `Adolescent | ActiveCycle | Pregnancy | Menopause`.

**Response 200 OK** — mismo shape que `GET /articles` paginado.

---

#### GET `/api/v1/articles/search`

- **Auth**: No

**Query params**

| Param | Type | Required |
|-------|------|----------|
| `q` | string | Sí |
| `language` | string | No |

**Response 200 OK** — mismo shape que `GET /articles`.

---

### Myths

#### GET `/api/v1/myths`

- **Auth**: No

**Query params**: `?category=...&page=1&limit=20`.

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "category": "Cycle",
      "question": "¿Es malo bañarse durante el período?",
      "answer": "Falso. No existe riesgo médico...",
      "language": "es"
    }
  ],
  "page": 1,
  "limit": 20,
  "total": 32
}
```

---

#### GET `/api/v1/myths/{id}`

- **Auth**: No

**Response 200 OK** — `MythDto` completo (mismos campos que el listado más `references[]`).

---

#### GET `/api/v1/myths/categories`

- **Auth**: No

**Response 200 OK**

```json
{
  "categories": ["Cycle", "Pregnancy", "Menopause", "Nutrition", "STI", "MentalHealth"]
}
```

---

### Glossary

#### GET `/api/v1/glossary`

- **Auth**: No

**Query params**: `?letter=A&language=es&page=1&limit=50`.

**Response 200 OK**

```json
{
  "items": [
    {
      "id": "guid",
      "term": "Amenorrea",
      "definition": "Ausencia del período menstrual...",
      "language": "es",
      "relatedTerms": ["Menstruación", "Oligomenorrea"]
    }
  ]
}
```

---

#### GET `/api/v1/glossary/{id}`

- **Auth**: No

**Response 200 OK** — `GlossaryTermDto` completo.

---

#### GET `/api/v1/glossary/search`

- **Auth**: No

**Query params**: `?q=ovulacion`.

**Response 200 OK** — mismo shape que `GET /glossary`.
