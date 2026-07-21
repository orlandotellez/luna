# Local Setup

Guía de configuración local para la base de datos PostgreSQL de LUNA.

> **Stack**: PostgreSQL 16 + EF Core 10 + Docker

---

## Prerrequisitos

| Herramienta | Versión | Propósito |
|-------------|---------|-----------|
| Docker | 24+ | Contenedor PostgreSQL |
| Docker Compose | v2+ | Orquestación de servicios |
| PostgreSQL CLI (`psql`) | 16+ | Consultas manuales (opcional) |
| .NET SDK | 10 | Migraciones con EF Core |
| pgAdmin / DBeaver | — | GUI para base de datos (opcional) |

---

## 1. Levantar PostgreSQL con Docker

```bash
# Desde la raíz del proyecto
docker compose up postgres -d

# Verificar que el contenedor esté corriendo
docker compose ps

# Ver logs
docker compose logs postgres
```

El `docker-compose.yml` expone PostgreSQL en:

| Variable | Valor |
|----------|-------|
| Host | `localhost` |
| Puerto | `5432` |
| Base de datos | `luna_dev` |
| Usuario | `luna` |
| Contraseña | (definida en `.env`) |

> Si no existe `docker-compose.yml`, crear uno mínimo:

```yaml
version: '3.8'
services:
  postgres:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: luna_dev
      POSTGRES_USER: luna
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U luna"]
      interval: 5s
      timeout: 5s
      retries: 5

volumes:
  pgdata:
```

---

## 2. Configurar Variables de Entorno

```bash
# En la raíz del proyecto
cp .env.example .env
```

Editar `.env` con los valores correspondientes:

```env
# PostgreSQL
POSTGRES_HOST=localhost
POSTGRES_PORT=5432
POSTGRES_DB=luna_dev
POSTGRES_USER=luna
POSTGRES_PASSWORD=tu_contraseña_segura

# Connection String (EF Core)
DATABASE_CONNECTION_STRING=Host=localhost;Port=5432;Database=luna_dev;Username=luna;Password=tu_contraseña_segura
```

---

## 3. Ejecutar Migraciones

```bash
# Aplicar migraciones desde el backend
cd src/Api
dotnet ef database update

# Verificar migraciones aplicadas
dotnet ef migrations list
```

Si no existe la carpeta `Migrations`, crearla:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 4. Seed Data (Opcional)

El proyecto incluye un seeder para datos de desarrollo:

```bash
# Desde src/Api
dotnet run -- --seed
```

Esto crea:
- Usuaria admin por defecto
- Categorías de artículos y contenido educativo inicial
- Mitos vs Realidad (contenido semilla)
- Profesionales demo
- Centros de salud de ejemplo
- Términos del glosario

---

## 5. Verificar la Conexión

```bash
# Con psql
psql -h localhost -U luna -d luna_dev

# Ver tablas
\dt

# Ver enums
\dt

# Verificar que uuid-ossp está habilitado
SELECT * FROM pg_extension WHERE extname = 'uuid-ossp';
```

> **Nota**: La extensión `uuid-ossp` es necesaria para `uuid_generate_v4()`. Si no está instalada:
```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
```

---

## 6. Comandos Útiles

```bash
# Resetear base de datos (cuidado: borra todo)
dotnet ef database drop
dotnet ef database update

# Crear una migración nueva
dotnet ef migrations add NombreDeLaMigracion

# Generar script SQL de migración
dotnet ef migrations script -o upgrade.sql

# Ver SQL que generará una migración
dotnet ef migrations script --idempotent
```

---

## Troubleshooting

### `uuid_generate_v4()` no existe
```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
```

### Puerto 5432 ocupado
Cambiar el puerto en `docker-compose.yml` y en `.env`.

### Conexión rechazada
```bash
# Verificar que el contenedor está corriendo
docker ps | grep postgres

# Ver logs
docker compose logs postgres
```
