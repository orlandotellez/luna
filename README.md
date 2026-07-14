# LUNA — Acompañamiento Integral de Salud Femenina

**LUNA** es una aplicación móvil diseñada para acompañar a las mujeres en todas las etapas de su vida: **menstruación, embarazo y menopausia**. Con contenido educativo en español y lenguas originarias de Latinoamérica, y un enfoque en la participación de la red de apoyo familiar.

> **Estado actual: Desarrollo activo — fase temprana.**  
> Este proyecto está en construcción. Las funcionalidades documentadas reflejan el destino final, no necesariamente el estado actual del código.

---

## ¿Qué es LUNA?

LUNA nace de una necesidad real: las aplicaciones de salud femenina disponibles están diseñadas para contextos muy distintos al latinoamericano. Muchas están solo en inglés, no consideran la diversidad lingüística y cultural de la región, y están pensadas para un usuario individual en lugar de una red de apoyo familiar.

LUNA propone un modelo diferente:

- **Contenido en español y lenguas originarias** — náhuatl, maya, mixteco, zapoteco, tsotsil, otomí y más.
- **Acompañamiento familiar** — pareja, madre, hermana, hija pueden participar como acompañantes en el proceso.
- **Directorio de profesionales y centros de salud** — enfocado en comunidades latinoamericanas.
- **Seguimiento integral** — ciclo menstrual, embarazo y menopausia en una sola app.

---

## Tech Stack

### Backend

| Tecnología | Versión | Propósito |
|---|---|---|
| **.NET / ASP.NET Core** | 10.0 | Web API |
| **C#** | 13 | Lenguaje |
| **Entity Framework Core** | 10.0 | ORM |
| **PostgreSQL** | 16+ | Base de datos |
| **BCrypt.Net-Next** | — | Hashing de contraseñas |
| **JWT Bearer** | 10.0 | Autenticación |
| **FluentValidation** | — | Validación de requests |

### Frontend

| Tecnología | Versión | Propósito |
|---|---|---|
| **React Native** | 0.81.5 | Framework mobile cross-platform |
| **Expo** | SDK 54 | Managed workflow, builds y OTA |
| **React** | 19.1.0 | UI |
| **TypeScript** | 5.9.2 | Tipado estricto |
| **Expo Router** | 6.0.23 | File-based routing |
| **pnpm** | — | Package manager |

### Infraestructura (planeada)

- Firebase Cloud Messaging — notificaciones push
- SendGrid — emails transaccionales
- AWS S3 — almacenamiento de archivos
- Docker — contenedores

---

## Arquitectura

### Backend: Clean Architecture

```
┌──────────────────────────────────────┐
│  Luna.Api        (Presentation)      │
│  - Controllers, Middleware, Config   │
├──────────────────────────────────────┤
│  Luna.Application (Use Cases)        │
│  - Interfaces, Services, DTOs, Maps  │
├──────────────────────────────────────┤
│  Luna.Domain      (Enterprise Logic) │
│  - Entities, Enums, Exceptions       │
│  - Sin dependencias externas         │
├──────────────────────────────────────┤
│  Luna.Infrastructure (Persistence)   │
│  - EF Core, Repositories, Configs    │
└──────────────────────────────────────┘
```

Decisiones de arquitectura:
- **Sin CQRS/MediatR** — inyección directa de servicios, sin sobrearquitecturar.
- **Sin AutoMapper** — extension methods manuales, explícitos y trazables.
- **JWT en HttpOnly cookies** — más seguro que localStorage.
- **Rate limiting** específico por ruta (10 req/min en auth).
- **Global Exception Middleware** con `AppException` → RFC 7807 ProblemDetails.

### Frontend: Feature-First + Atomic Design

- **Expo Router** con file-based routing.
- **Zustand** para estado global (planeado).
- **Componentes atómicos** reutilizables con sistema de diseño propio.
- **Paleta cálida**: rosa empoderamiento, lavanda, durazno, con colores específicos por etapa.

---

## Estructura del Proyecto

```
LUNA-REPO/
├── backend/                    # Backend .NET 10
│   ├── Luna.slnx              # Solution con 4 proyectos
│   └── src/
│       ├── Luna.Api/          # Capa de presentación
│       ├── Luna.Application/  # Casos de uso
│       ├── Luna.Domain/       # Entidades puras
│       └── Luna.Infrastructure/  # Persistencia
│
├── mobile-app/                # Frontend React Native / Expo
│   ├── app/                   # Expo Router (file-based routing)
│   ├── src/
│   │   └── shared/lib/       # Sistema de diseño (colores, tema)
│   └── assets/
│
├── specs/                     # Documentación técnica completa
│   ├── descripcion-proyecto.md
│   ├── global-instruction.md
│   └── modules/
│       ├── frontend/          # Stack, diseño, arquitectura, pantallas
│       ├── backend/           # Stack, API, seguridad, testing
│       └── db/                # Schema SQL (30+ tablas), setup Docker
│
└── experimento/               # Directorio experimental
```

---


## Cómo Empezar

### Prerrequisitos

- .NET 10 SDK
- Node.js 20+
- pnpm
- PostgreSQL 16+ (o Docker)

### Backend

```bash
cd backend/src/Luna.Api
dotnet restore
dotnet run
```

### Frontend

```bash
cd mobile-app
pnpm install
npx expo start
```

### Base de Datos (con Docker)

```bash
cd specs/modules/db
# Levantar PostgreSQL
docker compose up -d
# Luego aplicar migraciones desde el backend
```

---

## Documentación

La documentación completa del proyecto está en [`specs/`](./specs/):

| Sección | Archivos |
|---|---|
| Visión general | [`descripcion-proyecto.md`](./specs/descripcion-proyecto.md) |
| Stack y quick reference | [`global-instruction.md`](./specs/global-instruction.md) |
| Frontend | [`modules/frontend/`](./specs/modules/frontend/) |
| Backend | [`modules/backend/`](./specs/modules/backend/) |
| Base de datos | [`modules/db/`](./specs/modules/db/) |

---

## Contribuir

Por ahora el proyecto está en fase inicial de desarrollo. Si querés contribuir:

1. Hacé fork del repositorio.
2. Crea tu rama desde `develop`: `git checkout -b feat/lo-que-sea`
3. Hacé tus cambios con commits convencionales.
4. Abrí un Pull Request contra `develop`.

