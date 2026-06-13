# Casos de Uso — LUNA

Diagramas de flujo funcionales con Mermaid para cada caso de uso principal.

---

## Índice

1. [Registro y Onboarding](#1-registro-y-onboarding)
2. [Inicio de Sesión y Autenticación Biométrica](#2-inicio-de-sesión)
3. [Registro de Ciclo Menstrual](#3-registro-de-ciclo-menstrual)
4. [Registro de Síntomas Diarios](#4-registro-de-síntomas-diarios)
5. [Predicción de Ciclo](#5-predicción-de-ciclo)
6. [Inicio de Embarazo](#6-inicio-de-embarazo)
7. [Registro de Movimientos Fetales](#7-registro-de-movimientos-fetales)
8. [Registro de Síntomas Menopáusicos](#8-registro-de-síntomas-menopáusicos)
9. [Lectura de Contenido Educativo](#9-lectura-de-contenido-educativo)
10. [Invitación de Acompañante Familiar](#10-invitación-de-acompañante)
11. [Búsqueda de Profesional](#11-búsqueda-de-profesional)
12. [Participación en Foro](#12-participación-en-foro)
13. [Exportación de Reporte de Salud](#13-exportación-de-reporte)
14. [Notificaciones y Recordatorios](#14-notificaciones-y-recordatorios)
15. [Cambio de Etapa de Vida](#15-cambio-de-etapa-de-vida)

---

## 1. Registro y Onboarding

**Descripción**: Una nueva usuaria se registra, selecciona su etapa de vida y configura su perfil inicial de salud.

**Actores**: Usuaria no autenticada, Sistema

**Tablas involucradas**: `users`, `accounts`, `sessions`, `health_profiles`, `verifications`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend (RN)
    participant B as Backend (API)
    participant DB as PostgreSQL
    participant M as Mail Service

    U->>F: Abre app por primera vez
    F->>U: Slider de bienvenida (3 etapas)
    U->>F: Selecciona etapa de vida
    U->>F: Completa formulario de registro
    F->>B: POST /auth/register
    B->>DB: Verifica email no existe
    alt Email ya registrado
        B-->>F: 409 Email en uso
        F-->>U: Muestra error
    else Email disponible
        B->>DB: INSERT INTO users (name, email, life_stage, language)
        B->>DB: INSERT INTO accounts (user_id, provider_id='credentials', password=hash)
        B->>DB: INSERT INTO health_profiles (user_id, ...initial_data)
        B->>B: Genera token de verificación
        B->>DB: INSERT INTO verifications (identifier=email, value=token, expires_at)
        B->>M: Envía email de verificación
        B-->>F: 201 + Set cookies (accessToken, refreshToken)
        F->>U: Redirige a Dashboard personalizado
    end
```

---

## 2. Inicio de Sesión

**Descripción**: Una usuaria existente inicia sesión con email/password o mediante autenticación biométrica.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `users`, `accounts`, `sessions`, `login_logs`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend (RN)
    participant B as Backend (API)
    participant DB as PostgreSQL

    alt Acceso biométrico
        U->>F: Abre app, Face ID / Huella
        F->>F: Verifica biometría local (expo-local-authentication)
        F->>B: POST /auth/biometric { token }
        B->>DB: Verifica token biométrico
    else Email + Password
        U->>F: Ingresa email + password
        F->>B: POST /auth/login { email, password }
        B->>DB: Verifica credenciales
    end

    alt Credenciales inválidas
        B->>DB: INSERT INTO login_logs (email, success=false, failure_reason)
        B-->>F: 401 Error
        F-->>U: "Credenciales incorrectas"
    else Credenciales válidas
        B->>DB: INSERT INTO sessions (user_id, token, expires_at)
        B->>DB: INSERT INTO login_logs (user_id, success=true)
        B->>DB: UPDATE users SET last_seen_at = NOW()
        B-->>F: 200 + Set cookies
        F->>U: Redirige a Dashboard
    end
```

---

## 3. Registro de Ciclo Menstrual

**Descripción**: Una usuaria registra el inicio de su período menstrual con intensidad de flujo y síntomas.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `cycles`, `cycle_days`, `symptoms`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Dashboard → "Registrar período"
    F->>B: GET /cycle/current
    B->>DB: Obtiene último ciclo registrado
    B-->>F: Último ciclo + predicciones

    U->>F: Selecciona fecha de inicio
    U->>F: Selecciona intensidad de flujo
    U->>F: Registra síntomas asociados
    U->>F: Agrega nota opcional
    F->>B: POST /cycle/period { startDate, flowIntensity, symptoms, notes }
    B->>DB: INSERT INTO cycles (user_id, start_date, flow_intensity)

    alt Si ya existe cycle_day para esta fecha
        B->>DB: UPDATE cycle_days SET flow_intensity = ?
    else No existe
        B->>DB: INSERT INTO cycle_days (user_id, cycle_id, date, day_of_cycle, flow_intensity)
    end

    B->>DB: INSERT INTO symptoms (user_id, cycle_day_id, type, intensity) para cada síntoma
    B->>B: Recalcula predicción del próximo ciclo
    B-->>F: 200 { cycle, predictions }
    F->>U: Calendario actualizado + predicción actualizada

    note over B: Si dolor intenso 3 ciclos seguidos
    B->>F: Sugerencia: "Consulta a tu ginecóloga"
```

---

## 4. Registro de Síntomas Diarios

**Descripción**: Una usuaria registra síntomas, estado de ánimo y calidad del sueño en un día específico.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `cycle_days`, `symptoms`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → "Registrar síntoma"
    F->>U: Muestra selector de síntomas con sliders
    U->>F: Selecciona síntomas e intensidades
    U->>F: Selecciona estado de ánimo
    U->>F: Registra calidad del sueño
    U->>F: Escribe nota personal (opcional)
    F->>B: POST /cycle/symptoms { date, symptoms[], mood, sleepQuality, notes }
    B->>DB: UPSERT cycle_days (user_id, date)
    B->>DB: DELETE + INSERT symptoms para este día (reemplaza anteriores)
    B-->>F: 200
    F->>U: "Síntomas guardados"

    note over B: Sistema detecta patrón de síntomas
    B->>B: RecommendationEngine evalúa síntomas vs histórico
    alt Patrón detectado
        B-->>F: 200 + recommendedArticle (opcional)
        F->>U: Muestra recomendación de artículo
    end
```

---

## 5. Predicción de Ciclo

**Descripción**: El sistema calcula automáticamente la próxima menstruación, ventana fértil y ovulación basado en el historial.

**Actores**: Sistema (automático), Usuaria (consulta)

**Tablas involucradas**: `cycles`, `cycle_days`

```mermaid
sequenceDiagram
    participant S as CyclePredictor
    participant DB as PostgreSQL
    participant B as Backend (API)
    participant F as Frontend

    Note over S: Trigger: nuevo ciclo registrado
    S->>DB: SELECT * FROM cycles WHERE user_id = ? ORDER BY cycle_number DESC LIMIT 6
    S->>S: Calcula promedio de duración de ciclo
    S->>S: Calcula promedio de duración de período
    S->>S: Estima ventana fértil (método del calendario)
    S->>S: Calcula nivel de confianza según regularidad
    S->>DB: Guarda predicciones (en cache o como datos calculados)
    S-->>B: Predicción actualizada

    Note over B,F: Usuaria consulta
    U as Usuaria
    U->>F: Abre calendario / dashboard
    F->>B: GET /cycle/predictions
    B-->>F: { nextPeriodDate, fertileWindow, ovulationDate, confidence }

    Note over F: Renderiza calendario con colores
    F->>U: 🔴 Período esperado | 🟢 Ventana fértil | 💧 Ovulación
```

---

## 6. Inicio de Embarazo

**Descripción**: Una usuaria cambia su etapa a embarazo, ingresa su FUM y el sistema calcula la FPP y semana gestacional.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `pregnancies`, `appointments`, `users`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Perfil → "Cambiar etapa" → "Embarazo"
    F->>B: GET /users/me
    B-->>F: Etapa actual: ActiveCycle

    U->>F: Ingresa fecha de última menstruación (FUM)
    F->>B: POST /pregnancy/register { lastMenstrualPeriod, isFirstPregnancy }
    B->>B: Calcula FPP (regla de Naegele: FUM + 280 días)
    B->>B: Calcula semana gestacional actual
    B->>DB: INSERT INTO pregnancies (user_id, last_menstrual_period, estimated_due_date, current_week)
    B->>DB: UPDATE users SET life_stage = 'PREGNANCY'
    B-->>F: 201 { pregnancyId, currentWeek, estimatedDueDate }

    F->>U: Dashboard transformado a vista de embarazo
    F->>U: Muestra "Semana X" con información del desarrollo
    F->>U: Checklist primer trimestre: "Agenda tu primera ecografía"
    F->>U: Sugerencia: "Invita a tu pareja como acompañante"
```

---

## 7. Registro de Movimientos Fetales

**Descripción**: Una usuaria registra una sesión de conteo de patadas para monitorear la actividad fetal.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `fetal_movements`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Embarazo → "Contador de patadas"
    F->>U: Muestra botón grande "TOCA CADA VEZ QUE SIENTAS UNA PATADA"
    U->>F: Toca el botón cada vez que siente una patada
    F->>F: Contador local + temporizador

    Note over U: Sesión de 30 minutos

    U->>F: Finaliza sesión
    F->>B: POST /pregnancy/kicks { startTime, kicks[], totalKicks, durationMinutes }
    B->>DB: INSERT INTO fetal_movements (user_id, pregnancy_id, start_time, total_kicks, duration_minutes)
    B-->>F: 200

    alt Si total_kicks < 10 en 2 horas (baja actividad)
        B-->>F: 200 + alert: "Notaste poca actividad. ¿Quieres contactar a tu médico?"
        F->>U: Muestra alerta + botón "Contactar médico"
    end

    B->>B: Calcula promedio de patadas/hora
    B-->>F: 200 { averageKicksPerHour, trend }
    F->>U: Muestra historial: "Últimos 7 días"
```

---

## 8. Registro de Síntomas Menopáusicos

**Descripción**: Una usuaria en etapa de menopausia registra sus síntomas con intensidad y frecuencia.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `menopause_tracking`, `menopause_symptom_logs`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → "Registrar síntoma"
    F->>U: Muestra grid de síntomas menopáusicos
    U->>F: Selecciona "Sofoco" → Intensidad 4/5 → Frecuencia: 5/día
    U->>F: Selecciona "Sudores nocturnos" → Intensidad 3/5
    U->>F: Agrega nota: "Comenzaron después del almuerzo"
    F->>B: POST /menopause/symptoms { date, symptoms[] }
    B->>DB: INSERT INTO menopause_symptom_logs (user_id, menopause_id, date, type, intensity, frequency_per_day)
    B-->>F: 200

    B->>B: Actualiza gráfico de evolución
    alt Patrón detectado: sofocos más intensos en la tarde
        B-->>F: 200 + articleRecommendation: "Manejo de sofocos"
        F->>U: Muestra artículo recomendado
    end
```

---

## 9. Lectura de Contenido Educativo

**Descripción**: Una usuaria lee un artículo de la biblioteca, posiblemente en una lengua originaria.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `articles`, `article_translations`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Biblioteca
    F->>B: GET /articles?stage=PREGNANCY&language=ES
    B->>DB: SELECT * FROM articles WHERE stage = 'PREGNANCY' AND is_published = true
    B-->>F: Lista de artículos
    F->>U: Muestra grid de artículos por categoría

    U->>F: Busca "Embarazo saludable" → Filtra idioma → "Maya"
    F->>B: GET /articles/search?q=embarazo&language=MAY
    B->>DB: Full-text search con filtro de idioma
    B-->>F: Artículo encontrado + traducción en maya

    F->>B: GET /articles/{slug}
    B->>DB: Obtiene artículo + traducción al maya
    B-->>F: ArticleDetailDto con contenido en maya, audio_url en maya

    F->>U: Renderiza contenido en maya
    U->>F: Toca "Escuchar" → Reproduce audio en maya
    U->>F: Toca "Descargar" → Guarda para offline
    U->>F: Toca "Compartir" → Envía por WhatsApp a familiar
```

---

## 10. Invitación de Acompañante

**Descripción**: Una usuaria invita a un familiar como acompañante para que reciba contenido de apoyo y vea su calendario compartido.

**Actores**: Usuaria, Familiar, Sistema

**Tablas involucradas**: `family_members`, `users`, `family_messages`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Acompañamiento → "Invitar acompañante"
    U->>F: Ingresa nombre y correo de su pareja
    U->>F: Selecciona parentesco: "Pareja"
    U->>F: Configura permisos: ✓ Calendario ✓ Alertas ✓ Mensajes
    F->>B: POST /family/invite { name, email, relationship, permissions }

    B->>B: Genera invitation_token
    B->>DB: INSERT INTO family_members (user_id, name, email, relationship, invitation_token, status='PENDING')
    B->>B: Envía email de invitación con link de registro
    B-->>F: 200

    Note over F: Familiar recibe el email
    actor A as Familiar (Pareja)
    A->>A: Abre link de invitación
    alt Familiar ya tiene cuenta LUNA
        A->>F: Inicia sesión
        F->>B: POST /family/accept-invitation { token }
        B->>DB: UPDATE family_members SET status='ACCEPTED', family_user_id=?
    else Familiar no tiene cuenta
        A->>F: Se registra con perfil "FAMILIAR"
        B->>DB: INSERT INTO users (name, email, role='FAMILIAR')
        B->>DB: UPDATE family_members SET status='ACCEPTED', family_user_id=?
    end

    B-->>A: Redirige al dashboard de acompañante
    A->>A: Ve calendario compartido + contenido de apoyo
```

---

## 11. Búsqueda de Profesional

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

---

## 12. Participación en Foro

**Descripción**: Una usuaria participa en el foro de su etapa, crea un post o comenta.

**Actores**: Usuaria, Sistema, Moderación

**Tablas involucradas**: `forum_posts`, `forum_comments`, `moderation_reports`

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

---

## 13. Exportación de Reporte

**Descripción**: Una usuaria exporta su reporte mensual de salud en PDF para compartir con su médico.

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `health_reports`, `cycles`, `cycle_days`, `symptoms`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Perfil → "Reporte de Salud"
    U->>F: Selecciona período: "Junio 2026"
    F->>B: GET /reports/monthly?year=2026&month=6
    B->>DB: Obtiene datos del ciclo, síntomas, estado de ánimo del mes
    B-->>F: { summary, chartData, symptoms, trends }

    F->>U: Muestra resumen: ciclo, síntomas frecuentes, ánimo predominante
    F->>U: Muestra gráficos de tendencia

    U->>F: Toca "Exportar PDF"
    F->>B: GET /reports/export/pdf?type=monthly&month=6&year=2026
    B->>B: Genera PDF con datos del reporte
    B->>DB: INSERT INTO health_reports (user_id, type, period_start, period_end, pdf_url)
    B-->>F: { pdfUrl }

    F->>F: Descarga PDF (expo-file-system)
    F->>U: "PDF descargado"
    U->>F: Toca "Compartir" → Opciones nativas de compartir
```

---

## 14. Notificaciones y Recordatorios

**Descripción**: El sistema envía notificaciones push a la usuaria según sus recordatorios configurados y eventos del ciclo.

**Actores**: Sistema, Usuaria

**Tablas involucradas**: `reminders`, `notifications`, `push_devices`

```mermaid
sequenceDiagram
    participant R as ReminderService
    participant DB as PostgreSQL
    participant P as PushNotificationService
    participant FCM as Firebase Cloud Messaging
    participant F as Frontend

    Note over R: Background service - cada minuto

    R->>DB: SELECT * FROM reminders WHERE is_enabled = true AND (last_triggered_at IS NULL OR last_triggered_at < NOW() - interval)
    R->>R: Para cada reminder vencido, crea notificación
    R->>DB: INSERT INTO notifications (user_id, type, title, body)
    R->>DB: UPDATE reminders SET last_triggered_at = NOW()

    R->>DB: SELECT token FROM push_devices WHERE user_id = ? AND is_active = true
    R->>P: SendToDevice(token, { title, body, data })

    P->>FCM: Envía push notification
    FCM-->>F: Recibe notificación push

    Note over F: App en background
    F->>F: Muestra notificación en bandeja del sistema

    Note over F: App en foreground
    F->>F: Muestra toast in-app + actualiza badge

    actor U as Usuaria
    U->>F: Toca la notificación
    F->>F: Navega a la pantalla correspondiente (deep link)
```

---

## 15. Cambio de Etapa de Vida

**Descripción**: Una usuaria actualiza su etapa de vida (ej: de ciclo activo a embarazo, o a menopausia).

**Actores**: Usuaria, Sistema

**Tablas involucradas**: `users`, `health_profiles`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Perfil → "Mi etapa" → "Cambiar etapa"
    F->>B: GET /users/me
    B-->>F: Etapa actual: ActiveCycle

    F->>U: Muestra selector de etapas disponibles
    U->>F: Selecciona "Embarazo"
    F->>U: Pregunta: Fecha de última menstruación (FUM)
    U->>F: Ingresa FUM
    F->>B: PUT /users/me/life-stage { lifeStage: "PREGNANCY", lastMenstrualPeriod: "2026-01-15" }
    B->>DB: UPDATE users SET life_stage = 'PREGNANCY'
    B->>B: Calcula FPP y semana gestacional
    B->>DB: INSERT INTO pregnancies (user_id, last_menstrual_period, estimated_due_date, current_week)
    B-->>F: 200 { newStage: "PREGNANCY", currentWeek: 21, estimatedDueDate: "2026-10-22" }

    F->>U: Dashboard transformado
    F->>U: Muestra información de semana actual
    F->>U: Sugiere invitar acompañante
    F->>U: Muestra checklist de primer trimestre

    Note over B: Los datos anteriores de ciclo menstrual se preservan
    Note over B: La usuaria puede volver a cambiar si es necesario
```
