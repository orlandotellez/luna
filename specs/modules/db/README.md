# Database Module

Esquema y configuración de la base de datos PostgreSQL de LUNA.

## Estructura

```
db/
├── README.md
├── setup.md                          # Guía de setup local (Docker, migraciones, .env)
├── enums/
│   ├── README.md                     # Índice de todos los enums
│   ├── user_role.md
│   ├── life_stage.md
│   ├── cycle_phase.md
│   ├── flow_intensity.md
│   ├── symptom_type.md
│   ├── mood_type.md
│   ├── menopause_symptom_type.md
│   ├── article_category.md
│   ├── language.md
│   ├── family_relationship.md
│   ├── reminder_frequency.md
│   ├── appointment_type.md
│   └── notification_type.md
├── schemas/
│   ├── README.md                     # Índice de todas las tablas
│   ├── full-schema.md                # Todas las tablas en un solo archivo
│   ├── users.md                      # Tabla central
│   ├── accounts.md                   # Autenticación
│   ├── sessions.md                   # Sesiones activas
│   ├── verifications.md              # Tokens de verificación
│   ├── login_logs.md                 # Auditoría de login
│   ├── health_profiles.md            # Perfil de salud
│   ├── cycles.md                     # Ciclos menstruales
│   ├── cycle_days.md                 # Registro diario
│   ├── symptoms.md                   # Síntomas
│   ├── pregnancies.md               # Embarazos
│   ├── appointments.md              # Citas médicas
│   ├── fetal_movements.md           # Movimientos fetales
│   ├── weight_logs.md               # Registro de peso
│   ├── contractions.md              # Contracciones
│   ├── birth_plans.md               # Plan de parto
│   ├── menopause_tracking.md        # Seguimiento menopausia
│   ├── menopause_symptom_logs.md    # Síntomas menopáusicos
│   ├── articles.md                  # Contenido educativo
│   ├── article_translations.md      # Traducciones
│   ├── myths.md                     # Mitos vs Realidad
│   ├── glossary_terms.md            # Glosario médico
│   ├── family_members.md            # Acompañantes
│   ├── family_messages.md           # Mensajes familiares
│   ├── professionals.md             # Profesionales de la salud
│   ├── health_centers.md            # Centros de salud
│   ├── forum_posts.md               # Posts del foro
│   ├── forum_comments.md            # Comentarios del foro
│   ├── stories.md                   # Historias destacadas
│   ├── reminders.md                 # Recordatorios
│   ├── notifications.md             # Notificaciones in-app
│   ├── push_devices.md              # Dispositivos FCM
│   ├── health_reports.md            # Reportes de salud
│   └── audit_logs.md                # Log de auditoría
└── use-cases/
    ├── README.md                     # Índice de casos de uso
    ├── 01-registro-y-onboarding.md
    ├── 02-inicio-de-sesion.md
    ├── 03-registro-ciclo-menstrual.md
    ├── 04-registro-sintomas-diarios.md
    ├── 05-prediccion-ciclo.md
    ├── 06-inicio-embarazo.md
    ├── 07-registro-movimientos-fetales.md
    ├── 08-registro-sintomas-menopausicos.md
    ├── 09-lectura-contenido-educativo.md
    ├── 10-invitacion-acompanante.md
    ├── 11-busqueda-profesional.md
    ├── 12-participacion-foro.md
    ├── 13-exportacion-reporte.md
    ├── 14-notificaciones-recordatorios.md
    └── 15-cambio-etapa-vida.md
```

## Stack

| Componente | Tecnología |
|------------|------------|
| Motor | PostgreSQL 16 |
| ORM | Entity Framework Core 10 |
| Extensiones | `uuid-ossp` (UUIDs), full-text search (GIN + tsvector) |
| Migraciones | EF Core migrations |
| Infraestructura | Docker + Docker Compose |
