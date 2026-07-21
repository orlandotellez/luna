# Casos de Uso — LUNA

Diagramas de flujo funcionales con Mermaid para cada caso de uso principal de la plataforma.

## Listado

| # | Caso de Uso | Tablas involucradas |
|---|-------------|---------------------|
| 1 | [Registro y Onboarding](./01-registro-y-onboarding.md) | users, accounts, sessions, health_profiles, verifications |
| 2 | [Inicio de Sesión](./02-inicio-de-sesion.md) | users, accounts, sessions, login_logs |
| 3 | [Registro de Ciclo Menstrual](./03-registro-ciclo-menstrual.md) | cycles, cycle_days, symptoms |
| 4 | [Registro de Síntomas Diarios](./04-registro-sintomas-diarios.md) | cycle_days, symptoms |
| 5 | [Predicción de Ciclo](./05-prediccion-ciclo.md) | cycles, cycle_days |
| 6 | [Inicio de Embarazo](./06-inicio-embarazo.md) | pregnancies, appointments, users |
| 7 | [Registro de Movimientos Fetales](./07-registro-movimientos-fetales.md) | fetal_movements |
| 8 | [Registro de Síntomas Menopáusicos](./08-registro-sintomas-menopausicos.md) | menopause_tracking, menopause_symptom_logs |
| 9 | [Lectura de Contenido Educativo](./09-lectura-contenido-educativo.md) | articles, article_translations |
| 10 | [Invitación de Acompañante](./10-invitacion-acompanante.md) | family_members, users, family_messages |
| 11 | [Búsqueda de Profesional](./11-busqueda-profesional.md) | professionals |
| 12 | [Participación en Foro](./12-participacion-foro.md) | forum_posts, forum_comments |
| 13 | [Exportación de Reporte](./13-exportacion-reporte.md) | health_reports, cycles, cycle_days, symptoms |
| 14 | [Notificaciones y Recordatorios](./14-notificaciones-recordatorios.md) | reminders, notifications, push_devices |
| 15 | [Cambio de Etapa de Vida](./15-cambio-etapa-vida.md) | users, health_profiles |
