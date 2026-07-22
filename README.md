# LUNA — Acompañamiento Integral de Salud Femenina

![React Native](https://img.shields.io/badge/React%20Native-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![Expo](https://img.shields.io/badge/Expo-1C1E24?style=for-the-badge&logo=expo&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![.NET 10](https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![pnpm](https://img.shields.io/badge/pnpm-F69220?style=for-the-badge&logo=pnpm&logoColor=white)

**LUNA** es una aplicación móvil diseñada para acompañar a las mujeres en todas las etapas de su vida: **menstruación, embarazo y menopausia**. Con contenido educativo en español y lenguas originarias de Latinoamérica, y un enfoque en la participación de la red de apoyo familiar.

---

## ¿Qué es LUNA?

LUNA nace de una necesidad real: las aplicaciones de salud femenina disponibles están diseñadas para contextos muy distintos al latinoamericano. Muchas están solo en inglés, no consideran la diversidad lingüística y cultural de la región, y están pensadas para un usuario individual en lugar de una red de apoyo familiar.

LUNA propone un modelo diferente:

- **Contenido en español y lenguas originarias** — náhuatl, maya, mixteco, zapoteco, tsotsil, otomí y más.
- **Acompañamiento familiar** — pareja, madre, hermana, hija pueden participar como acompañantes en el proceso.
- **Directorio de profesionales y centros de salud** — enfocado en comunidades latinoamericanas.
- **Seguimiento integral** — ciclo menstrual, embarazo y menopausia en una sola app.

---

## Índice

- [Tech Stack](#tech-stack)
- [Cómo Empezar](#cómo-empezar)
- [Arquitectura](#arquitectura)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Documentación](#documentación)
- [Contribuir](#contribuir)

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

---

## Cómo Empezar

### Prerrequisitos

- .NET 10 SDK
- Node.js 20+
- pnpm
- PostgreSQL 16+ (o Docker)

### Backend
Las migraciones se generan automaticamente, solo se necesita la conexión local de la base de dato en appsettings.json

```bash
cd backend/src/Luna.Api
dotnet run 
```

### Frontend

```bash
cd mobile-app
pnpm install
pnpm expo start -c 
```

``

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

