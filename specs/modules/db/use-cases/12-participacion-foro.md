# 12. Participación en Foro

**Descripción**: Una usuaria participa en el foro de su etapa, crea un post o comenta.

**Actores**: Usuaria, Sistema, Moderación

**Tablas involucradas**: `forum_posts`, `forum_comments`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Comunidad
    F->>B: GET /forum/posts?stage=PREGNANCY&page=1
    B->>DB: SELECT * FROM forum_posts WHERE stage = 'PREGNANCY' AND is_published = true ORDER BY created_at DESC
    B-->>F: Lista de posts
    F->>U: Muestra posts con reacciones y comentarios

    U->>F: Toca "Nuevo post"
    U->>F: Escribe título + contenido
    U->>F: Activa "Publicar anónimamente" (opcional)
    F->>B: POST /forum/posts { stage, title, content, isAnonymous }
    B->>B: Filtro automático de contenido inapropiado
    alt Contenido inapropiado detectado
        B->>DB: INSERT INTO forum_posts (is_published=false, reports_count=1)
        B-->>F: 201 "Tu post será revisado por moderación"
        F->>U: "Tu post está pendiente de revisión"
    else Contenido apropiado
        B->>DB: INSERT INTO forum_posts (stage, title, content, is_anonymous, is_published=true)
        B-->>F: 201
        F->>U: Post publicado
    end

    note over U,B: Otro usuario comenta
    Otro as OtraUsuaria
    Otro->>F: Abre post → Escribe comentario
    F->>B: POST /forum/posts/{id}/comments { content }
    B->>DB: INSERT INTO forum_comments (post_id, user_id, content)
    B->>DB: UPDATE forum_posts SET comments_count = comments_count + 1
    B-->>F: 201
```
