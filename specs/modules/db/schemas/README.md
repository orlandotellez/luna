# Schemas — LUNA

Esquemas de tablas de la base de datos PostgreSQL.

Cada archivo documenta una tabla con su esquema, columnas, constraints, índices y relaciones.

## Listado de tablas

### 1. Users & Auth

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `users` | [users.md](./users.md) | Tabla central de usuarias y acompañantes |
| `accounts` | [accounts.md](./accounts.md) | Autenticación por password u OAuth |
| `sessions` | [sessions.md](./sessions.md) | Sesiones activas (refresh tokens) |
| `verifications` | [verifications.md](./verifications.md) | Tokens de verificación |
| `login_logs` | [login_logs.md](./login_logs.md) | Auditoría de intentos de login |

### 2. Health Profile

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `health_profiles` | [health_profiles.md](./health_profiles.md) | Perfil de salud completo |

### 3. Cycle Tracking

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `cycles` | [cycles.md](./cycles.md) | Ciclos menstruales registrados |
| `cycle_days` | [cycle_days.md](./cycle_days.md) | Registro diario de síntomas |
| `symptoms` | [symptoms.md](./symptoms.md) | Síntomas registrados por día |

### 4. Pregnancy

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `pregnancies` | [pregnancies.md](./pregnancies.md) | Registro de embarazo |
| `appointments` | [appointments.md](./appointments.md) | Citas médicas |
| `fetal_movements` | [fetal_movements.md](./fetal_movements.md) | Movimientos fetales |
| `weight_logs` | [weight_logs.md](./weight_logs.md) | Registro de peso |
| `contractions` | [contractions.md](./contractions.md) | Contracciones |
| `birth_plans` | [birth_plans.md](./birth_plans.md) | Plan de parto |

### 5. Menopause

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `menopause_tracking` | [menopause_tracking.md](./menopause_tracking.md) | Seguimiento de menopausia |
| `menopause_symptom_logs` | [menopause_symptom_logs.md](./menopause_symptom_logs.md) | Síntomas menopáusicos |

### 6. Content Library

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `articles` | [articles.md](./articles.md) | Contenido educativo |
| `article_translations` | [article_translations.md](./article_translations.md) | Traducciones a lenguas originarias |
| `myths` | [myths.md](./myths.md) | Mitos vs Realidad |
| `glossary_terms` | [glossary_terms.md](./glossary_terms.md) | Glosario de términos médicos |

### 7. Family

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `family_members` | [family_members.md](./family_members.md) | Acompañantes autorizados |
| `family_messages` | [family_messages.md](./family_messages.md) | Mensajes del foro familiar |

### 8. Directory

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `professionals` | [professionals.md](./professionals.md) | Profesionales de la salud |
| `health_centers` | [health_centers.md](./health_centers.md) | Centros de salud |

### 9. Community

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `forum_posts` | [forum_posts.md](./forum_posts.md) | Posts del foro |
| `forum_comments` | [forum_comments.md](./forum_comments.md) | Comentarios del foro |
| `stories` | [stories.md](./stories.md) | Historias destacadas |

### 10. Reminders

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `reminders` | [reminders.md](./reminders.md) | Recordatorios personalizados |

### 11. Notifications

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `notifications` | [notifications.md](./notifications.md) | Notificaciones in-app |
| `push_devices` | [push_devices.md](./push_devices.md) | Dispositivos para push |

### 12. Reports & Audit

| Tabla | Archivo | Descripción |
|-------|---------|-------------|
| `health_reports` | [health_reports.md](./health_reports.md) | Reportes de salud |
| `audit_logs` | [audit_logs.md](./audit_logs.md) | Log de auditoría |

---

> **Vista completa**: [full-schema.md](./full-schema.md) contiene todas las tablas en un solo archivo.
