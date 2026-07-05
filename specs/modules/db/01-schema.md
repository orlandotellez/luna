# Database Schema

Esquema completo de la base de datos PostgreSQL de LUNA.

---

## Convenciones Generales

- **Primary Keys**: `UUID` generados con `uuid_generate_v4()` en todas las tablas (sin auto-increment)
- **Timestamps**: `created_at` y `updated_at` en todas las entidades; `deleted_at` para soft-delete
- **Soft-delete**: Las tablas con `deleted_at` no eliminan registros físicamente
- **Índices**: Todas las FK tienen índice; las columnas de búsqueda frecuente también
- **Naming**: `snake_case` para columnas, `PascalCase` para tablas

---

## Enums

### `user_role`
| Valor | Descripción |
|-------|-------------|
| `USER` | Usuaria principal — mujer que recibe acompañamiento |
| `FAMILIAR` | Familiar acompañante — pareja, madre, hermana |
| `PROFESSIONAL` | Profesional de la salud |
| `ADMIN` | Administradora del sistema |

### `life_stage`
| Valor | Descripción |
|-------|-------------|
| `ADOLESCENT` | Adolescente / Juventud |
| `ACTIVE_CYCLE` | Edad adulta / ciclo menstrual activo |
| `PREGNANCY` | Embarazo / Postparto |
| `MENOPAUSE` | Perimenopausia / Menopausia |

### `cycle_phase`
| Valor | Descripción |
|-------|-------------|
| `MENSTRUAL` | Fase menstrual (días 1-5 aprox) |
| `FOLLICULAR` | Fase folicular |
| `OVULATORY` | Fase ovulatoria |
| `LUTEAL` | Fase lútea |

### `flow_intensity`
| Valor | Descripción |
|-------|-------------|
| `LIGHT` | Flujo leve |
| `MODERATE` | Flujo moderado |
| `HEAVY` | Flujo abundante |

### `symptom_type`
| Valor | Descripción |
|-------|-------------|
| `CRAMPS` | Cólicos |
| `HEADACHE` | Dolor de cabeza |
| `BLOATING` | Hinchazón |
| `MOOD_SWINGS` | Cambios de ánimo |
| `FATIGUE` | Fatiga |
| `NAUSEA` | Náuseas |
| `BREAST_TENDERNESS` | Senos sensibles |
| `BACK_PAIN` | Dolor lumbar |
| `CRAVINGS` | Antojos |
| `INSOMNIA` | Insomnio |
| `ACNE` | Acné |
| `SPOTTING` | Manchado |

### `mood_type`
| Valor | Descripción |
|-------|-------------|
| `HAPPY` | Feliz |
| `NORMAL` | Normal |
| `SAD` | Triste |
| `ANXIOUS` | Ansiosa |
| `IRRITABLE` | Irritable |
| `CALM` | Calmada |
| `ENERGETIC` | Energética |

### `menopause_symptom_type`
| Valor | Descripción |
|-------|-------------|
| `HOT_FLASH` | Sofocos |
| `NIGHT_SWEATS` | Sudores nocturnos |
| `VAGINAL_DRYNESS` | Sequedad vaginal |
| `INSOMNIA` | Insomnio |
| `MOOD_SWINGS` | Cambios de ánimo |
| `WEIGHT_GAIN` | Aumento de peso |
| `JOINT_PAIN` | Dolor articular |
| `MEMORY_LOSS` | Pérdida de memoria |
| `HAIR_LOSS` | Pérdida de cabello |
| `LIBIDO_DECREASE` | Disminución de libido |

### `article_category`
| Valor | Descripción |
|-------|-------------|
| `CYCLE` | Ciclo menstrual |
| `PREGNANCY` | Embarazo |
| `MENOPAUSE` | Menopausia |
| `GENERAL` | Salud general |
| `MYTHS` | Mitos vs Realidad |
| `NUTRITION` | Nutrición |
| `EXERCISE` | Ejercicio |
| `MENTAL_HEALTH` | Salud mental |

### `language`
| Valor | Descripción |
|-------|-------------|
| `ES` | Español |
| `NAH` | Náhuatl |
| `MAY` | Maya |
| `MIX` | Mixteco |
| `ZAP` | Zapoteco |
| `TSO` | Tsotsil |
| `OTO` | Otomí |

### `family_relationship`
| Valor | Descripción |
|-------|-------------|
| `PARTNER` | Pareja |
| `MOTHER` | Madre |
| `SISTER` | Hermana |
| `DAUGHTER` | Hija |
| `OTHER` | Otro |

### `reminder_frequency`
| Valor | Descripción |
|-------|-------------|
| `ONCE` | Una vez |
| `DAILY` | Diario |
| `WEEKLY` | Semanal |
| `MONTHLY` | Mensual |
| `CUSTOM` | Personalizado |

### `appointment_type`
| Valor | Descripción |
|-------|-------------|
| `PRENATAL` | Control prenatal |
| `GYNECOLOGIST` | Consulta ginecológica |
| `MAMMOGRAM` | Mastografía |
| `PAP_SMEAR` | Papanicolaou |
| `DENSITOMETRY` | Densitometría ósea |
| `ULTRASOUND` | Ecografía |
| `BLOOD_TEST` | Análisis de sangre |
| `OTHER` | Otro |

### `notification_type`
| Valor | Descripción |
|-------|-------------|
| `PERIOD_REMINDER` | Recordatorio de período |
| `PILL_REMINDER` | Recordatorio de anticonceptivo |
| `APPOINTMENT` | Recordatorio de cita |
| `CONTENT` | Contenido recomendado |
| `FAMILY_MESSAGE` | Mensaje de acompañante |
| `SUPPORT` | Recordatorio de apoyo |
| `ALERT` | Alerta de salud |
| `AFFIRMATION` | Afirmación diaria |

---

## Tablas

### 1. Users & Auth

#### `users`
Tabla central de usuarias y acompañantes.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` `DEFAULT uuid_generate_v4()` | Identificador único |
| `name` | `text` | `NOT NULL` | Nombre completo |
| `email` | `text` | `NOT NULL` `UNIQUE` | Email de acceso |
| `email_verified` | `boolean` | `NOT NULL` `DEFAULT false` | Email verificado |
| `phone` | `text` | | Teléfono |
| `image` | `text` | | URL de avatar/foto |
| `role` | `user_role` | `NOT NULL` `DEFAULT 'USER'` | Rol del usuario |
| `life_stage` | `life_stage` | `NOT NULL` | Etapa de vida actual |
| `language` | `language` | `NOT NULL` `DEFAULT 'ES'` | Idioma preferido |
| `department_id` | `varchar(10)` | | Departamento de ubicación |
| `municipality_id` | `varchar(10)` | | Municipio de ubicación |
| `date_of_birth` | `date` | | Fecha de nacimiento |
| `is_active` | `boolean` | `NOT NULL` `DEFAULT true` | Cuenta activa/inactiva |
| `last_seen_at` | `timestamptz` | | Última vez online |
| `created_at` | `timestamptz` | `NOT NULL` `DEFAULT CURRENT_TIMESTAMP` | |
| `updated_at` | `timestamptz` | `NOT NULL` `DEFAULT CURRENT_TIMESTAMP` | |
| `deleted_at` | `timestamptz` | | Soft-delete |

**Índices**: `email`, `life_stage`, `department_id`, `created_at`

---

#### `accounts`
Autenticación por password u OAuth.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | Usuario dueño |
| `provider_id` | `text` | `NOT NULL` | Proveedor (credentials, google, apple) |
| `account_id` | `text` | | ID de cuenta en proveedor |
| `password` | `text` | | Password hasheado (auth por credentials) |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `provider_id`, **unique** `(provider_id, account_id)`
`Ref: accounts.user_id > users.id [delete: cascade]`

---

#### `sessions`
Sesiones activas (refresh tokens).

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `token` | `text` | `NOT NULL` `UNIQUE` | Token de sesión |
| `expires_at` | `timestamptz` | `NOT NULL` | |
| `ip_address` | `text` | | |
| `user_agent` | `text` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `token`, `user_id`, `expires_at`
`Ref: sessions.user_id > users.id [delete: cascade]`

---

#### `verifications`
Tokens de verificación (email, password reset).

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `identifier` | `text` | `NOT NULL` | Email o teléfono |
| `value` | `text` | `NOT NULL` | Token |
| `expires_at` | `timestamptz` | `NOT NULL` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `identifier`, `value`, `expires_at`

---

#### `login_logs`
Auditoría de intentos de inicio de sesión.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `FK -> users.id` | Usuario (null si falló) |
| `email` | `text` | | Email intentado |
| `success` | `boolean` | `NOT NULL` | |
| `failure_reason` | `text` | | Motivo del fallo |
| `ip_address` | `text` | | |
| `user_agent` | `text` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `email`, `success`, `created_at`
`Ref: login_logs.user_id > users.id [delete: set null]`

---

### 2. Health Profile

#### `health_profiles`
Perfil de salud completo de la usuaria.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `UNIQUE` `FK -> users.id` | |
| `has_regular_cycle` | `boolean` | | Ciclo regular |
| `cycle_length_days` | `int` | | Duración del ciclo en días |
| `period_length_days` | `int` | | Duración del período en días |
| `has_endometriosis` | `boolean` | `DEFAULT false` | |
| `has_pcos` | `boolean` | `DEFAULT false` | Síndrome de ovario poliquístico |
| `has_thyroid_issues` | `boolean` | `DEFAULT false` | Problemas de tiroides |
| `has_gestational_diabetes` | `boolean` | `DEFAULT false` | Diabetes gestacional |
| `has_fibroids` | `boolean` | `DEFAULT false` | Miomas |
| `has_hypertension` | `boolean` | `DEFAULT false` | Hipertensión |
| `allergies` | `text` | | Alergias conocidas |
| `medications` | `text[]` | | Medicamentos actuales |
| `previous_pregnancies` | `int` | `DEFAULT 0` | Número de embarazos previos |
| `surgeries` | `text` | | Cirugías previas |
| `vaccinations` | `text[]` | | Vacunas recibidas |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`
`Ref: health_profiles.user_id > users.id [delete: cascade]`

---

### 3. Cycle Tracking

#### `cycles`
Ciclos menstruales registrados.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `cycle_number` | `int` | `NOT NULL` | Número de ciclo (1, 2, 3...) |
| `start_date` | `date` | `NOT NULL` | Fecha de inicio del período |
| `end_date` | `date` | | Fecha de fin del período |
| `period_length_days` | `int` | | Duración del período |
| `cycle_length_days` | `int` | | Duración total del ciclo |
| `flow_intensity` | `flow_intensity` | | Intensidad del flujo |
| `notes` | `text` | | Notas personales |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | |

**Índices**: `user_id`, `start_date`, `cycle_number`
**Unique**: `(user_id, cycle_number)`
`Ref: cycles.user_id > users.id [delete: cascade]`

---

#### `cycle_days`
Registro diario de síntomas dentro de un ciclo.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `cycle_id` | `uuid` | `NOT NULL` `FK -> cycles.id` | |
| `date` | `date` | `NOT NULL` | |
| `day_of_cycle` | `int` | `NOT NULL` | Día del ciclo (1 = primer día de período) |
| `phase` | `cycle_phase` | | Fase del ciclo estimada |
| `flow_intensity` | `flow_intensity` | | Intensidad del flujo (si aplica) |
| `pain_level` | `int` | | Dolor 1-5 |
| `mood` | `mood_type` | | Estado de ánimo |
| `sleep_hours` | `decimal(3,1)` | | Horas de sueño |
| `sleep_quality` | `int` | | Calidad del sueño 1-5 |
| `notes` | `text` | | Notas personales |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `cycle_id`, `date`
**Unique**: `(user_id, date)`
`Ref: cycle_days.user_id > users.id [delete: cascade]`
`Ref: cycle_days.cycle_id > cycles.id [delete: cascade]`

---

#### `symptoms`
Síntomas registrados por día.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `cycle_day_id` | `uuid` | `NOT NULL` `FK -> cycle_days.id` | |
| `type` | `symptom_type` | `NOT NULL` | Tipo de síntoma |
| `intensity` | `int` | `NOT NULL` | Intensidad 1-5 |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `cycle_day_id`, `type`
`Ref: symptoms.user_id > users.id [delete: cascade]`
`Ref: symptoms.cycle_day_id > cycle_days.id [delete: cascade]`

---

### 4. Pregnancy

#### `pregnancies`
Registro de embarazo.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `last_menstrual_period` | `date` | | FUM |
| `estimated_due_date` | `date` | `NOT NULL` | FPP calculada |
| `current_week` | `int` | `NOT NULL` | Semana gestacional actual |
| `is_first_pregnancy` | `boolean` | | Primer embarazo |
| `pregnancy_count` | `int` | `DEFAULT 1` | Número de embarazo (1, 2...) |
| `is_active` | `boolean` | `NOT NULL` `DEFAULT true` | Embarazo activo |
| `ended_at` | `timestamptz` | | Fecha de término (parto) |
| `notes` | `text` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `is_active`
`Ref: pregnancies.user_id > users.id [delete: cascade]`

---

#### `appointments`
Citas médicas prenatales y ginecológicas.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `FK -> pregnancies.id` | Si aplica |
| `type` | `appointment_type` | `NOT NULL` | Tipo de cita |
| `title` | `varchar(255)` | `NOT NULL` | |
| `description` | `text` | | |
| `scheduled_at` | `timestamptz` | `NOT NULL` | Fecha y hora |
| `location` | `text` | | Ubicación |
| `professional_name` | `text` | | Nombre del profesional |
| `is_completed` | `boolean` | `DEFAULT false` | |
| `notes` | `text` | | Notas post-cita |
| `reminder_sent` | `boolean` | `DEFAULT false` | Recordatorio enviado |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | |

**Índices**: `user_id`, `pregnancy_id`, `scheduled_at`, `type`
`Ref: appointments.user_id > users.id [delete: cascade]`
`Ref: appointments.pregnancy_id > pregnancies.id [delete: cascade]`

---

#### `fetal_movements`
Registro de movimientos fetales (patadas).

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `NOT NULL` `FK -> pregnancies.id` | |
| `start_time` | `timestamptz` | `NOT NULL` | Inicio de la sesión de conteo |
| `total_kicks` | `int` | `NOT NULL` | Total de movimientos |
| `duration_minutes` | `int` | `NOT NULL` | Duración de la sesión |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `pregnancy_id`, `start_time`
`Ref: fetal_movements.user_id > users.id [delete: cascade]`
`Ref: fetal_movements.pregnancy_id > pregnancies.id [delete: cascade]`

---

#### `weight_logs`
Registro de peso durante el embarazo.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `NOT NULL` `FK -> pregnancies.id` | |
| `date` | `date` | `NOT NULL` | |
| `weight_kg` | `decimal(5,2)` | `NOT NULL` | Peso en kg |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `pregnancy_id`, `date`
**Unique**: `(user_id, pregnancy_id, date)`
`Ref: weight_logs.user_id > users.id [delete: cascade]`
`Ref: weight_logs.pregnancy_id > pregnancies.id [delete: cascade]`

---

#### `contractions`
Registro de contracciones (preparto).

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `NOT NULL` `FK -> pregnancies.id` | |
| `session_start` | `timestamptz` | `NOT NULL` | Inicio de la sesión |
| `start_time` | `timestamptz` | `NOT NULL` | Inicio de la contracción |
| `end_time` | `timestamptz` | `NOT NULL` | Fin de la contracción |
| `duration_seconds` | `int` | `NOT NULL` | Duración |
| `interval_seconds` | `int` | | Intervalo desde la anterior |
| `intensity` | `int` | | Intensidad 1-5 |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `pregnancy_id`, `session_start`
`Ref: contractions.user_id > users.id [delete: cascade]`
`Ref: contractions.pregnancy_id > pregnancies.id [delete: cascade]`

---

#### `birth_plans`
Plan de parto de la usuaria.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `pregnancy_id` | `uuid` | `NOT NULL` `FK -> pregnancies.id` | |
| `birth_type` | `text` | | Preferencia de parto (vaginal, cesárea, natural) |
| `companion` | `text` | | Acompañante durante el parto |
| `pain_management` | `text` | | Manejo del dolor |
| `birth_position` | `text` | | Posición preferida |
| `skin_to_skin` | `boolean` | `DEFAULT true` | Contacto piel con piel |
| `delayed_cord_clamping` | `boolean` | `DEFAULT true` | Clampaje tardío del cordón |
| `immediate_breastfeeding` | `boolean` | `DEFAULT true` | Lactancia inmediata |
| `special_requests` | `text` | | Solicitudes especiales |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `pregnancy_id`
**Unique**: `(user_id, pregnancy_id)`
`Ref: birth_plans.user_id > users.id [delete: cascade]`
`Ref: birth_plans.pregnancy_id > pregnancies.id [delete: cascade]`

---

### 5. Menopause

#### `menopause_tracking`
Seguimiento de menopausia.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `last_period_date` | `date` | | Fecha del último período |
| `months_without_period` | `int` | | Meses sin período |
| `is_in_menopause` | `boolean` | | Confirmación de menopausia |
| `symptom_start_age` | `int` | | Edad de inicio de síntomas |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`
`Ref: menopause_tracking.user_id > users.id [delete: cascade]`

---

#### `menopause_symptom_logs`
Registro diario/semanal de síntomas menopáusicos.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `menopause_id` | `uuid` | `NOT NULL` `FK -> menopause_tracking.id` | |
| `date` | `date` | `NOT NULL` | |
| `type` | `menopause_symptom_type` | `NOT NULL` | Tipo de síntoma |
| `intensity` | `int` | `NOT NULL` | Intensidad 1-5 |
| `frequency_per_day` | `int` | | Frecuencia diaria (ej: 5 sofocos/día) |
| `notes` | `text` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `menopause_id`, `date`, `type`
`Ref: menopause_symptom_logs.user_id > users.id [delete: cascade]`
`Ref: menopause_symptom_logs.menopause_id > menopause_tracking.id [delete: cascade]`

---

### 6. Content Library

#### `articles`
Contenido educativo.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `title` | `varchar(255)` | `NOT NULL` | Título |
| `slug` | `varchar(255)` | `NOT NULL` `UNIQUE` | |
| `category` | `article_category` | `NOT NULL` | Categoría |
| `stage` | `life_stage` | | Etapa de vida asociada |
| `summary` | `text` | | Resumen breve |
| `content` | `text` | `NOT NULL` | Contenido en markdown |
| `image_url` | `text` | | URL de imagen de portada |
| `audio_url` | `text` | | URL de audio (narración) |
| `video_url` | `text` | | URL de video |
| `read_time_minutes` | `int` | | Tiempo de lectura |
| `is_published` | `boolean` | `DEFAULT false` | |
| `is_featured` | `boolean` | `DEFAULT false` | |
| `author` | `text` | | Autor/a del artículo |
| `scientific_refs` | `text[]` | | Referencias científicas |
| `search_vector` | `tsvector` | | Full-text search |
| `published_at` | `timestamptz` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | |

**Índices**: `slug`, `category`, `stage`, `is_published`, `is_featured`, `search_vector` (GIN)

---

#### `article_translations`
Traducciones de artículos a lenguas originarias.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `article_id` | `uuid` | `NOT NULL` `FK -> articles.id` | |
| `language` | `language` | `NOT NULL` | |
| `title` | `varchar(255)` | `NOT NULL` | |
| `summary` | `text` | | |
| `content` | `text` | `NOT NULL` | |
| `audio_url` | `text` | | Audio en esa lengua |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `article_id`, `language`
**Unique**: `(article_id, language)`
`Ref: article_translations.article_id > articles.id [delete: cascade]`

---

#### `myths`
Mitos vs Realidad.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `myth_text` | `text` | `NOT NULL` | El mito |
| `reality_text` | `text` | `NOT NULL` | La realidad con respaldo científico |
| `category` | `article_category` | `NOT NULL` | Categoría |
| `scientific_ref` | `text` | | Referencia científica |
| `is_published` | `boolean` | `DEFAULT false` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `category`, `is_published`

---

#### `glossary_terms`
Glosario de términos médicos.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `term` | `varchar(255)` | `NOT NULL` `UNIQUE` | Término |
| `definition` | `text` | `NOT NULL` | Definición simple |
| `category` | `varchar(100)` | | Categoría |
| `audio_url` | `text` | | Pronunciación (audio) |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `term`

---

### 7. Family / Acompañamiento

#### `family_members`
Acompañantes autorizados por la usuaria.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | Usuaria que invita |
| `family_user_id` | `uuid` | `FK -> users.id` | Usuario acompañante (si se registró) |
| `name` | `text` | `NOT NULL` | Nombre del acompañante |
| `email` | `text` | `NOT NULL` | Email del acompañante |
| `relationship` | `family_relationship` | `NOT NULL` | Parentesco |
| `invitation_token` | `text` | | Token de invitación |
| `status` | `varchar(20)` | `NOT NULL` `DEFAULT 'PENDING'` | PENDING, ACCEPTED, REJECTED, REVOKED |
| `can_view_calendar` | `boolean` | `DEFAULT true` | Permiso: ver calendario |
| `can_receive_alerts` | `boolean` | `DEFAULT true` | Permiso: recibir alertas |
| `can_send_messages` | `boolean` | `DEFAULT true` | Permiso: enviar mensajes |
| `invited_at` | `timestamptz` | `NOT NULL` | |
| `accepted_at` | `timestamptz` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `family_user_id`, `email`, `status`
`Ref: family_members.user_id > users.id [delete: cascade]`
`Ref: family_members.family_user_id > users.id [delete: set null]`

---

#### `family_messages`
Mensajes en el foro familiar.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `family_group_id` | `uuid` | `NOT NULL` | Grupo familiar (user_id del que invita) |
| `sender_id` | `uuid` | `NOT NULL` `FK -> users.id` | Quien envía |
| `message` | `text` | `NOT NULL` | |
| `is_support_message` | `boolean` | `DEFAULT false` | Mensaje predefinido de apoyo |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `family_group_id`, `sender_id`
`Ref: family_messages.sender_id > users.id [delete: cascade]`

---

### 8. Directory

#### `professionals`
Profesionales de la salud.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `name` | `text` | `NOT NULL` | |
| `specialty` | `varchar(100)` | `NOT NULL` | Especialidad |
| `photo_url` | `text` | | |
| `experience_years` | `int` | | |
| `description` | `text` | | |
| `phone` | `text` | | |
| `email` | `text` | | |
| `website` | `text` | | |
| `address` | `text` | | |
| `latitude` | `decimal(10,7)` | | |
| `longitude` | `decimal(10,7)` | | |
| `department_id` | `varchar(10)` | | |
| `municipality_id` | `varchar(10)` | | |
| `languages` | `language[]` | | Idiomas que habla |
| `consultation_fee` | `decimal(10,2)` | | Costo aproximado |
| `accepts_insurance` | `boolean` | | Acepta seguro |
| `offers_teleconsult` | `boolean` | | Ofrece teleconsulta |
| `rating` | `decimal(3,2)` | | Calificación promedio |
| `rating_count` | `int` | `DEFAULT 0` | |
| `is_verified` | `boolean` | `DEFAULT false` | Perfil verificado |
| `is_active` | `boolean` | `DEFAULT true` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | |

**Índices**: `specialty`, `department_id`, `is_verified`, `is_active`

---

#### `health_centers`
Centros de salud, clínicas y hospitales.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `name` | `text` | `NOT NULL` | |
| `type` | `varchar(50)` | `NOT NULL` | clinic, hospital, community_center |
| `phone` | `text` | | |
| `address` | `text` | | |
| `latitude` | `decimal(10,7)` | | |
| `longitude` | `decimal(10,7)` | | |
| `department_id` | `varchar(10)` | | |
| `municipality_id` | `varchar(10)` | | |
| `services` | `text[]` | | Servicios ofrecidos |
| `schedule` | `text` | | Horario |
| `is_public` | `boolean` | `DEFAULT true` | Público o privado |
| `is_active` | `boolean` | `DEFAULT true` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `type`, `department_id`, `is_public`

---

### 9. Community

#### `forum_posts`
Posts del foro por etapa.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | Autora |
| `stage` | `life_stage` | `NOT NULL` | Etapa del foro |
| `title` | `varchar(255)` | `NOT NULL` | |
| `content` | `text` | `NOT NULL` | |
| `is_anonymous` | `boolean` | `DEFAULT false` | Publicar anónimamente |
| `is_published` | `boolean` | `DEFAULT true` | |
| `reactions` | `jsonb` | `DEFAULT '{}'` | Conteo de reacciones { "heart": 5, "hug": 3, "strong": 7 } |
| `comments_count` | `int` | `DEFAULT 0` | |
| `reports_count` | `int` | `DEFAULT 0` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | |

**Índices**: `user_id`, `stage`, `is_published`, `created_at`
`Ref: forum_posts.user_id > users.id [delete: cascade]`

---

#### `forum_comments`
Comentarios en posts del foro.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `post_id` | `uuid` | `NOT NULL` `FK -> forum_posts.id` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `parent_id` | `uuid` | `FK -> forum_comments.id` | Respuesta a comentario |
| `content` | `text` | `NOT NULL` | |
| `reactions` | `jsonb` | | Reacciones positivas |
| `is_edited` | `boolean` | `DEFAULT false` | |
| `is_flagged` | `boolean` | `DEFAULT false` | Reportado |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |
| `deleted_at` | `timestamptz` | | |

**Índices**: `post_id`, `user_id`, `parent_id`
`Ref: forum_comments.post_id > forum_posts.id [delete: cascade]`
`Ref: forum_comments.user_id > users.id [delete: cascade]`
`Ref: forum_comments.parent_id > forum_comments.id`

---

#### `stories`
Historias destacadas de la comunidad.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | Autora (anonimizada) |
| `title` | `varchar(255)` | `NOT NULL` | |
| `content` | `text` | `NOT NULL` | |
| `category` | `varchar(50)` | | superacion, descubrimiento, comunidad, consejos |
| `anonymous_name` | `varchar(100)` | | Nombre usado (ej: "María de la Luz") |
| `is_approved` | `boolean` | `DEFAULT false` | Aprobada por admin |
| `is_published` | `boolean` | `DEFAULT false` | |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `is_approved`, `is_published`, `category`
`Ref: stories.user_id > users.id [delete: cascade]`

---

### 10. Reminders

#### `reminders`
Recordatorios personalizados.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `type` | `varchar(50)` | `NOT NULL` | pill, period, appointment, exam, vaccination, affirmation |
| `title` | `varchar(255)` | `NOT NULL` | |
| `description` | `text` | | |
| `frequency` | `reminder_frequency` | `NOT NULL` | |
| `time` | `time` | | Hora del recordatorio |
| `days_of_week` | `int[]` | | Días de la semana (1=lunes .. 7=domingo) |
| `day_of_month` | `int` | | Día del mes |
| `start_date` | `date` | | Fecha de inicio |
| `end_date` | `date` | | Fecha de fin (opcional) |
| `is_enabled` | `boolean` | `DEFAULT true` | |
| `last_triggered_at` | `timestamptz` | | Última vez que se disparó |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `type`, `is_enabled`
`Ref: reminders.user_id > users.id [delete: cascade]`

---

### 11. Notifications

#### `notifications`
Notificaciones in-app.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `type` | `notification_type` | `NOT NULL` | |
| `title` | `varchar(255)` | `NOT NULL` | |
| `body` | `text` | `NOT NULL` | |
| `image_url` | `text` | | |
| `action_url` | `text` | | Deep link |
| `is_read` | `boolean` | `DEFAULT false` | |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `type`, `is_read`, `created_at`
`Ref: notifications.user_id > users.id [delete: cascade]`

---

#### `push_devices`
Registro de dispositivos para FCM.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `token` | `text` | `NOT NULL` `UNIQUE` | FCM token |
| `platform` | `varchar(10)` | `NOT NULL` | ios, android |
| `is_active` | `boolean` | `DEFAULT true` | |
| `created_at` | `timestamptz` | `NOT NULL` | |
| `updated_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `token`, `is_active`
`Ref: push_devices.user_id > users.id [delete: cascade]`

---

### 12. Reports & Audit

#### `health_reports`
Reportes de salud generados.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `NOT NULL` `FK -> users.id` | |
| `type` | `varchar(20)` | `NOT NULL` | monthly, trend, export |
| `period_start` | `date` | `NOT NULL` | |
| `period_end` | `date` | `NOT NULL` | |
| `pdf_url` | `text` | | URL del PDF generado |
| `data` | `jsonb` | | Datos del reporte (cache) |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `type`, `period_start`
`Ref: health_reports.user_id > users.id [delete: cascade]`

---

#### `audit_logs`
Log de auditoría para acceso a datos sensibles.

| Columna | Tipo | Constraints | Descripción |
|---------|------|-------------|-------------|
| `id` | `uuid` | `PK` | |
| `user_id` | `uuid` | `FK -> users.id` | Quien accedió |
| `target_user_id` | `uuid` | | Usuario cuyos datos fueron accedidos |
| `action` | `varchar(255)` | `NOT NULL` | data_access, permission_change, account_delete |
| `entity_type` | `varchar(255)` | | |
| `entity_id` | `uuid` | | |
| `ip_address` | `varchar(45)` | | |
| `created_at` | `timestamptz` | `NOT NULL` | |

**Índices**: `user_id`, `target_user_id`, `action`, `created_at`

---

## Full-Text Search

La tabla `articles` tiene un índice GIN sobre `search_vector` (type `tsvector`) para búsqueda de texto completo:

```sql
CREATE INDEX idx_articles_search ON articles USING GIN(search_vector);
```

Esto permite búsquedas eficientes del tipo:

```sql
SELECT * FROM articles
WHERE search_vector @@ to_tsquery('spanish', 'embarazo & alimentacion');
```
