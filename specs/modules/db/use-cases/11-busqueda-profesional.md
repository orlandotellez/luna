# 11. Búsqueda de Profesional

**Descripción**: Una usuaria busca un profesional de la salud cerca de su ubicación, con filtros por especialidad e idioma.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `professionals`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Directorio → "Buscar profesional"
    F->>B: GET /directory/professionals?departmentId=14&page=1
    B->>DB: SELECT * FROM professionals WHERE department_id = '14' AND is_active = true
    B-->>F: Lista de profesionales en el departamento

    U->>F: Filtra: Ginecología + Idioma náhuatl
    F->>B: GET /directory/professionals?specialty=Gynecology&language=NAH&departmentId=14
    B->>DB: SELECT * FROM professionals WHERE specialty = 'Gynecology' AND 'NAH' = ANY(languages) AND department_id = '14'
    B-->>F: Resultados filtrados

    U->>F: Ve perfil de una ginecóloga
    F->>B: GET /directory/professionals/{id}
    B-->>F: Detalle: foto, experiencia, costo, rating, mapa
    F->>U: Muestra perfil con opciones
    U->>F: Toca "Llamar" → Abre teléfono
    U->>F: Toca "Agendar cita" → Link externo o teleconsulta
```
