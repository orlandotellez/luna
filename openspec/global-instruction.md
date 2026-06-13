# LUNA — Acompañamiento Integral a Mujeres

Aplicación móvil integral para el acompañamiento de la salud femenina a lo largo de todas las etapas de la vida: **menstruación**, **embarazo** y **menopausia**. Piensa en Flo + Ovia + Clue con un enfoque en lenguas originarias y acompañamiento familiar.

---

## Project Overview

**LUNA** es una aplicación móvil de acompañamiento de salud femenina con:
- Interfaz cálida, femenina, accesible y multilingüe
- Full-stack implementation (React Native + ASP.NET Core 10)
- PostgreSQL persistence
- Contenido educativo multimedia en español y lenguas originarias
- Recordatorios inteligentes y notificaciones push
- Acompañamiento familiar integrado
- Directorio de servicios de salud
- Comunidad y foros con moderación
- Reportes de salud exportables

---

## Module Structure

The specification is organized into three main modules:

### 📁 [frontend/](modules/frontend/)
Frontend React Native implementation.

| File | Description |
|------|-------------|
| [01-stack](modules/frontend/01-stack.md) | Stack tecnológico |
| [02-design](modules/frontend/02-design.md) | Identidad visual, colores, tipografía, UI Kit |
| [03-architecture](modules/frontend/03-architecture.md) | Arquitectura, navegación, flujo de datos |
| [04-screens](modules/frontend/04-screens.md) | Pantallas detalladas por módulo |
| [05-quality](modules/frontend/05-quality.md) | Criterios de calidad |
| [06-push-notifications](modules/frontend/06-push-notifications.md) | Notificaciones push y recordatorios |

### 📁 [backend/](modules/backend/)
Backend ASP.NET Core 10 implementation.

| File | Description |
|------|-------------|
| [01-stack](modules/backend/01-stack.md) | Stack tecnológico |
| [02-architecture](modules/backend/02-architecture.md) | Clean Architecture, Service Layer, errores |
| [03-api](modules/backend/03-api.md) | Endpoints REST, JWT, autenticación |
| [04-security](modules/backend/04-security.md) | Medidas de seguridad |
| [05-testing](modules/backend/05-testing.md) | Estrategia de testing |
| [06-push-notifications](modules/backend/06-push-notifications.md) | Firebase Cloud Messaging + notificaciones |

### 📁 [db/](modules/db/)
Database PostgreSQL.

| File | Description |
|------|-------------|
| [01-schema](modules/db/01-schema.md) | Esquema SQL completo con tablas, enums y relaciones |
| [02-setup](modules/db/02-setup.md) | Setup local con Docker + migraciones |
| [03-use-cases](modules/db/03-use-cases.md) | Casos de uso con diagramas Mermaid |

---

## Quick Reference

### Stack
- **Frontend**: React Native 0.76+, TypeScript strict, Expo SDK 52+, Zustand, React Navigation, React Native Paper (opcional)
- **Backend**: ASP.NET Core 10, C# 13, Clean Architecture, Service Layer, FluentValidation
- **Database**: PostgreSQL 16, EF Core 10, full-text search (tsvector + GIN)
- **Infrastructure**: Docker, Firebase Cloud Messaging, SendGrid/Mailjet

### Design System
- Tema cálido: rosa empoderamiento (#D4A5B6), lavanda (#B8A9C9), blanco roto (#FFF5F5)
- Tono safe: coral (#E8A598), teal (#88BDB0) como secundarios
- Tipografía: Playfair Display (headings), Inter (body), Noto Sans (lenguas originarias)
- Border-radius: 16px cards, 12px inputs, 24px modales

### Auth
- JWT (15 min) + httpOnly refresh token (30 days)
- Role-based: USER, FAMILIAR, PROFESSIONAL, ADMIN
- Autenticación biométrica (huella / Face ID)
