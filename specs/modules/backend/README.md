# Backend Module

Backend de **LUNA** — ASP.NET Core 10 Web API.

## Contents

1. [01-stack](./01-stack.md) — Stack tecnológico
2. [02-architecture](./02-architecture.md) — Estructura, capas, errores
3. [03-api](./03-api.md) — Endpoints y autenticación
4. [04-security](./04-security.md) — Medidas de seguridad
5. [05-testing](./05-testing.md) — Estrategia de testing
6. [06-push-notifications](./06-push-notifications.md) — Firebase Cloud Messaging

## Quick Start

```bash
# Desde backend/
dotnet restore
dotnet run --project src/Api/Api.csproj
```

Requiere PostgreSQL corriendo en `localhost:5432` con la connection string configurada en `appsettings.json`.

Seed automático crea datos demo al iniciar (admin, categorías, contenido educativo inicial).
