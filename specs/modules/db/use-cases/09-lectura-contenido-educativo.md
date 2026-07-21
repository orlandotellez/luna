# 9. Lectura de Contenido Educativo

**Descripción**: Una usuaria lee un artículo de la biblioteca, posiblemente en una lengua originaria.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `articles`, `article_translations`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Biblioteca
    F->>B: GET /articles?stage=PREGNANCY&language=ES
    B->>DB: SELECT * FROM articles WHERE stage = 'PREGNANCY' AND is_published = true
    B-->>F: Lista de artículos
    F->>U: Muestra grid de artículos por categoría

    U->>F: Busca "Embarazo saludable" → Filtra idioma → "Maya"
    F->>B: GET /articles/search?q=embarazo&language=MAY
    B->>DB: Full-text search con filtro de idioma
    B-->>F: Artículo encontrado + traducción en maya

    F->>B: GET /articles/{slug}
    B->>DB: Obtiene artículo + traducción al maya
    B-->>F: ArticleDetailDto con contenido en maya, audio_url en maya

    F->>U: Renderiza contenido en maya
    U->>F: Toca "Escuchar" → Reproduce audio en maya
    U->>F: Toca "Descargar" → Guarda para offline
    U->>F: Toca "Compartir" → Envía por WhatsApp a familiar
```
