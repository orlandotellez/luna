# Screens Detail

Detalle de todas las pantallas de LUNA con su implementación según la arquitectura Feature-First + Container-Presentational.

> **Patrón general**: Cada pantalla usa un hook (container) para lógica + datos, y componentes presentacionales para UI.
> Ver [`03-architecture.md`](./03-architecture.md) para los patrones completos.

---

## Onboarding y Registro

### 1. Welcome / Onboarding Screen

**Ruta**: `Onboarding > Welcome`
**Tipo**: Client Component (FlatList horizontal con paginación)

#### Layout
- Full-screen slider con 3 slides (menstruación, embarazo, menopausia)
- Cada slide: ilustración + título + descripción breve
- Page indicator (dots)
- Botón "Siguiente" y "Omitir" (si ya conoce su etapa)
- Botón "Comenzar" en el último slide

#### Data
- Slides son datos estáticos (locales, no requieren API)

---

### 2. Stage Selection Screen

**Ruta**: `Onboarding > StageSelection`
**Tipo**: Client Component

#### Layout
- Título: "¿En qué etapa te encuentras?"
- 4 tarjetas ilustradas:

| Etapa | Icono/Ilustración | Descripción |
|-------|-------------------|-------------|
| Adolescente / Juventud | 🌸 | Información sobre tu ciclo, cambios corporales |
| Edad Adulta / Ciclo Activo | 🌙 | Seguimiento de ciclo, fertilidad, anticoncepción |
| Embarazo / Postparto | 👶 | Seguimiento semana a semana, preparación |
| Perimenopausia / Menopausia | 🦋 | Manejo de síntomas, cambios hormonales |

- Cada tarjeta es touchable, al seleccionar avanza al registro
- Footer: "No estoy segura — Quiero explorar primero" (modo libre)

---

### 3. Register Screen

**Ruta**: `Auth > Register`
**Tipo**: Client Component

#### Hook: `useRegister()`
Maneja validación de formulario, envío de registro, errores.

#### Formulario
- Nombre completo (required)
- Correo electrónico (required, valid format)
- Teléfono (opcional)
- Contraseña (required, min 6 chars)
- Confirmar contraseña
- Ubicación: Selector de Departamento + Municipio
- Términos y condiciones (checkbox required)
- Botón "Crear cuenta" (primary, full-width)
- Link "Ya tengo cuenta → Iniciar sesión"

#### Validación
- Email: formato válido, único en backend
- Password: min 6 caracteres
- Confirmación: debe coincidir

---

### 4. Health Profile Init Screen

**Ruta**: `Onboarding > HealthProfile`
**Tipo**: Client Component

#### Preguntas según etapa

**Si seleccionó Ciclo Menstrual**:
- ¿Tienes ciclo regular? (Sí / No / No estoy segura)
- ¿Cuántos días dura tu ciclo? (selector numérico)
- ¿Cuántos días dura tu período? (selector numérico)
- Fecha de inicio de tu último período

**Si seleccionó Embarazo**:
- ¿Estás embarazada actualmente? (Sí / No / Buscando)
- Fecha de última menstruación (FUM) → calcula FPP
- ¿Es tu primer embarazo? (Sí / No)
- ¿Tienes control prenatal? (Sí / No)

**Si seleccionó Menopausia**:
- ¿Has tenido tu último período hace más de 12 meses? (Sí / No)
- ¿Estás experimentando sofocos? (Sí / No)
- Edad de inicio de síntomas (selector)

---

### 5. Language Selection Screen

**Ruta**: `Onboarding > LanguageSelection`
**Tipo**: Client Component

- Título: "Elige tu idioma preferido"
- Lista de idiomas con bandera/indicador:
  - Español
  - Náhuatl
  - Maya
  - Mixteco
  - Zapoteco
  - Tsotsil
  - Otomí
  - Otros (dropdown con más opciones)
- Botón "Confirmar"
- Footer: "Puedes cambiar el idioma en cualquier momento desde Configuración"

---

### 6. Login Screen

**Ruta**: `Auth > Login`
**Tipo**: Client Component

#### Hook: `useLogin()`
Maneja autenticación, biométrico, errores.

#### Formulario
- Correo electrónico
- Contraseña
- "Recordarme" (checkbox)
- Botón "Iniciar sesión"
- Link "¿Olvidaste tu contraseña?"
- Link "¿No tienes cuenta? Regístrate"
- Botón "Acceso biométrico" (si está configurado)

---

## Pantallas Principales (Tabs)

### 7. Home / Dashboard Screen

**Ruta**: `Main > Home > Dashboard`
**Tipo**: Client Component

**Hook**: `useDashboard()` — obtiene datos según etapa actual

#### Layout (ScrollView)

```
┌─────────────────────────────────┐
│  Header: Saludo + avatar        │
│  "Buenos días, María"           │
│  Frase motivacional del día     │
├─────────────────────────────────┤
│  Tarjeta de Etapa Actual        │
│  [Etapa] → "Semana 24"         │
│  Progreso / Días hasta próx.    │
├─────────────────────────────────┤
│  Acceso Rápido (grid 2x2)       │
│  ┌──────┐ ┌──────┐             │
│  │Regist.│ │Calen.│             │
│  │Síntoma│ │dario │             │
│  ├──────┤ ├──────┤             │
│  │Artícu │ │Recor.│             │
│  │Recom. │ │dario │             │
│  └──────┘ └──────┘             │
├─────────────────────────────────┤
│  Contenido Recomendado          │
│  [ArticleCard]                   │
│  [ArticleCard]                   │
├─────────────────────────────────┤
│  Próximos Recordatorios         │
│  - Cita prenatal: 15/06         │
│  - Próximo período: 22/06       │
└─────────────────────────────────┘
```

#### Estados
- **Loading**: Skeleton cards
- **Error**: ErrorState con retry
- **Success**: Dashboard completo

---

### 8. Bottom Tab Navigation

**Ruta**: `Main > MainTabNavigator`
**Tipo**: Client Component

| Tab | Icono | Label | Stack |
|-----|-------|-------|-------|
| 1 | Home | Inicio | Dashboard |
| 2 | Cycle/Moon | Mi Ciclo | StageStack (cambia según etapa) |
| 3 | BookOpen | Biblioteca | LibraryStack |
| 4 | Users | Comunidad | CommunityStack |
| 5 | User | Perfil | ProfileStack |

**Estilo**: Tab bar con fondo blanco, borde superior suave, icono + label, indicador de active en primary color.

---

## Pantallas por Etapa

### 9. Cycle Dashboard / Calendar Screen

**Ruta**: `Stage > Cycle > Calendar`
**Tipo**: Client Component

**Hook**: `useCycleData()`, `useCalendar()`

#### Layout
```
┌─────────────────────────────────┐
│  Header: "Mi Ciclo" + info      │
│  Día actual del ciclo: 12       │
│  Próx. período: en 5 días       │
├─────────────────────────────────┤
│  CalendarView (mes actual)      │
│  ┌──┬──┬──┬──┬──┬──┬──┐       │
│  │  │  │  │  │  │  │  │       │
│  │  │  │🔴│🔴│  │  │  │       │ ← período
│  │  │  │  │  │💧│💧│  │       │ ← fértil
│  │  │  │  │  │  │  │  │       │
│  └──┴──┴──┴──┴──┴──┴──┘       │
├─────────────────────────────────┤
│  Leyenda de colores             │
│  🔴 Período  🟢 Fértil         │
│  💧 Ovulación  🟡 Síntoma      │
├─────────────────────────────────┤
│  Registro Rápido                │
│  [FlowSelector] [PainScale]     │
│  [SymptomSelector]              │
│  Botón "Guardar"                │
└─────────────────────────────────┘
```

#### Calendar Color Code
| Color | Significado |
|-------|-------------|
| 🔴 Rosa | Período |
| 🟢 Verde claro | Ventana fértil |
| 💧 Azul | Ovulación estimada |
| 🟡 Amarillo | Síntoma registrado |
| ⚪ Sin relleno | Día normal |

---

### 10. Symptom Register Screen

**Ruta**: `Stage > Cycle > RegisterSymptom`
**Tipo**: Client Component

**Hook**: `useSymptoms()`

#### Layout
```
┌─────────────────────────────────┐
│  Header: "Registrar Síntomas"   │
│  Fecha: hoy (date picker)       │
├─────────────────────────────────┤
│  Flujo Menstrual                │
│  [Leve] [Moderado] [Abundante]  │
│  (selector tipo pill)           │
├─────────────────────────────────┤
│  Dolor Menstrual                │
│  [PainScale: 1-5 con emojis]    │
│  😊😐😟😢😭                     │
├─────────────────────────────────┤
│  Síntomas (grid checkboxes)     │
│  ☐ Dolor de cabeza              │
│  ☐ Hinchazón                    │
│  ☐ Senos sensibles              │
│  ☐ Fatiga                       │
│  ☐ Náuseas                      │
│  ☐ Cambios de ánimo             │
│  ☐ Antojos                      │
│  ☐ Insomnio                     │
├─────────────────────────────────┤
│  Estado de Ánimo                │
│  [emoji selector: feliz, normal,│
│   triste, ansiosa, irritable]   │
├─────────────────────────────────┤
│  Calidad del Sueño              │
│  [⭐ 1-5]                       │
├─────────────────────────────────┤
│  Nota Personal (textarea)       │
├─────────────────────────────────┤
│  Botón "Guardar"                │
└─────────────────────────────────┘
```

---

### 11. Cycle Detail / Charts Screen

**Ruta**: `Stage > Cycle > Detail`
**Tipo**: Client Component

**Hook**: `useCycleData()`

#### Charts
- **Duración de Ciclo**: bar chart últimos 6 meses
- **Intensidad de Flujo**: line chart por día del período
- **Síntomas Recurrentes**: heatmap de síntomas por mes
- **Estado de Ánimo Promedio**: trend line

#### Stats Cards
- Duración promedio del ciclo
- Duración promedio del período
- Días de ciclo más común
- Regularidad (regular / irregular / variable)

---

### 12. Pregnancy Dashboard Screen

**Ruta**: `Stage > Pregnancy > Dashboard`
**Tipo**: Client Component

**Hook**: `usePregnancyData()`

#### Layout
```
┌─────────────────────────────────┐
│  Header: "Semana 24"            │
│  FPP: 15 de octubre 2026        │
│  Progreso: ████████░░ 70%       │
├─────────────────────────────────┤
│  Desarrollo del Bebé            │
│  [FetusInfo]                    │
│  Tamaño: un melón (30 cm)       │
│  Peso: 600g                     │
│  Hito: Abre los ojos           │
├─────────────────────────────────┤
│  Cambios en Mamá                │
│  - Aumento de peso              │
│  - Movimientos más fuertes      │
├─────────────────────────────────┤
│  Accesos Rápidos (grid 2x2)     │
│  ┌──────┐ ┌──────────┐         │
│  │Kick  │ │Checklist │         │
│  │Counter│ │ Citas    │         │
│  ├──────┤ ├──────────┤         │
│  │Peso  │ │Plan de   │         │
│  │      │ │Parto     │         │
│  └──────┘ └──────────┘         │
├─────────────────────────────────┤
│  Recomendación de la Semana     │
│  [RecommendationCard]           │
│  "Alimentación para el tercer   │
│   trimestre"                    │
└─────────────────────────────────┘
```

---

### 13. Week Detail Screen

**Ruta**: `Stage > Pregnancy > WeekDetail`
**Tipo**: Client Component

- Selector de semana (scroll horizontal)
- Información del bebé: tamaño (comparado con fruta/objeto), peso, hitos
- Cambios en la madre
- Recomendaciones: alimentación, ejercicios, cuidados
- Checklist de la semana: exámenes, vacunas
- Imagen ilustrativa del desarrollo fetal

---

### 14. Appointment Tracker Screen

**Ruta**: `Stage > Pregnancy > Appointments`
**Tipo**: Client Component

**Hook**: `useAppointments()`

- Lista de citas agendadas (próximas y pasadas)
- Checklist por trimestre:
  - **Primer trimestre**: Ecografía inicial, análisis de sangre, prueba de VIH
  - **Segundo trimestre**: Ecografía morfológica, curva de glucosa
  - **Tercer trimestre**: Vacuna Tdap, monitoreo fetal, cultivo estreptococo
- Botón "Agendar nueva cita" → selector de fecha + tipo de examen
- Recordatorio automático 24h antes

---

### 15. Kick Counter Screen

**Ruta**: `Stage > Pregnancy > KickCounter`
**Tipo**: Client Component

**Hook**: `useKickCounter()`

- Gran botón circular "TOCA CADA VEZ QUE SIENTAS UNA PATADA"
- Contador: 12 patadas en 8 minutos
- Temporizador automático (recomendado: contar 10 movimientos)
- Timeline de patadas registradas
- Historial: últimos 7 días
- Alerta si la actividad disminuye significativamente

---

### 16. Contraction Timer Screen

**Ruta**: `Stage > Pregnancy > ContractionTimer`
**Tipo**: Client Component

- Botón grande: "INICIAR CONTRACCIÓN" / "TERMINAR CONTRACCIÓN"
- Timer en vivo de la contracción actual
- Lista de contracciones registradas: duración, intervalo
- Patrón: cada 5 minutos, duran 45 segundos
- Indicador "¿Es tiempo de ir al hospital?" basado en la regla 5-1-1

---

### 17. Birth Plan Screen

**Ruta**: `Stage > Pregnancy > BirthPlan`
**Tipo**: Client Component

**Hook**: `useBirthPlan()`

- Editor de preferencias editable:
  - Tipo de parto: vaginal / cesárea / lo que decida la naturaleza
  - Acompañante: quien estará presente
  - Manejo del dolor: epidural / métodos naturales / lo que necesite
  - Posición para el parto
  - Cuidados del recién nacido: contacto piel con piel, clampaje tardío
  - Lactancia: inmediata / cuando sea posible
- Checklist de preparación preparto
- Botón "Compartir con mi médico" (exportar PDF)

---

### 18. Postpartum Screen

**Ruta**: `Stage > Pregnancy > Postpartum`
**Tipo**: Client Component

- Seguimiento de recuperación postparto
- Registro de loquios (color, cantidad)
- Registro de tomas de lactancia (pecho izquierdo/derecho, duración)
- Signos de alerta: fiebre, sangrado abundante, dolor intenso
- Salud mental: escala de depresión postparto (Edinburgh)
- Recordatorio de cita postparto (6 semanas)

---

### 19. Menopause Dashboard Screen

**Ruta**: `Stage > Menopause > Dashboard`
**Tipo**: Client Component

**Hook**: `useMenopauseData()`

#### Layout
```
┌─────────────────────────────────┐
│  Header: "Mi Menopausia"        │
│  Días sin período: 245          │
├─────────────────────────────────┤
│  Síntomas Activos (hoy/esta sem)│
│  🔥 Sofocos: 4 (moderado)      │
│  😰 Sudores nocturnos: 2        │
│  😴 Insomnio: leve              │
├─────────────────────────────────┤
│  Acceso Rápido                   │
│  [Registrar Síntoma]            │
│  [Ver Evolución]                │
│  [Recomendaciones]              │
├─────────────────────────────────┤
│  Gráfico de Evolución           │
│  [SymptomChart]                 │
│  "Sofocos últimos 30 días"      │
├─────────────────────────────────┤
│  Recordatorios                   │
│  🦴 Densitometría: en 2 meses   │
│  💊 Calcio + Vitamina D: diario │
└─────────────────────────────────┘
```

---

### 20. Menopause Symptom Tracker Screen

**Ruta**: `Stage > Menopause > SymptomTracker`
**Tipo**: Client Component

**Hook**: `useSymptomTracking()`

- Selector de síntomas:

| Síntoma | Tipo de registro |
|---------|-----------------|
| Sofocos | Intensidad (1-5) + Frecuencia/día |
| Sudores nocturnos | Intensidad (1-5) |
| Sequedad vaginal | Intensidad (1-5) |
| Insomnio | Horas de sueño |
| Cambios de ánimo | Estado de ánimo |
| Aumento de peso | Peso en kg |
| Dolor articular | Intensidad (1-5) |
| Pérdida de memoria | Leve/Moderado/Notable |

- Cada síntoma: slider de intensidad, nota opcional
- Botón "Guardar lote de síntomas"

---

### 21. Bone Health Screen

**Ruta**: `Stage > Menopause > BoneHealth`
**Tipo**: Client Component

- Recordatorio de densitometría (frecuencia: cada 2 años)
- Suplementos: registro de calcio + vitamina D (dosis diaria)
- Ejercicios recomendados: fortalecimiento óseo, Kegel, cardio suave
- Factores de riesgo: historial familiar, peso, tabaquismo
- Score de riesgo de osteoporosis (cuestionario)

---

## Pantallas de Biblioteca

### 22. Library Home Screen

**Ruta**: `Library > Home`
**Tipo**: Client Component

**Hook**: `useLibrary()`

```
┌─────────────────────────────────┐
│  SearchBar 🔍                   │
├─────────────────────────────────┤
│  Categorías Grid (scroll horiz) │
│  [Ciclo] [Embarazo] [Menopausia]│
│  [General] [Mitos] [Glosario]  │
├─────────────────────────────────┤
│  Sección: Destacados            │
│  [ArticleCard horizontal]       │
│  [ArticleCard horizontal]       │
├─────────────────────────────────┤
│  Sección: Recomendados para ti  │
│  (basado en etapa + síntomas)   │
│  [ArticleCard] [ArticleCard]    │
├─────────────────────────────────┤
│  Sección: En Lengua Originaria  │
│  [ArticleCard] "En Maya"        │
│  [ArticleCard] "En Náhuatl"    │
└─────────────────────────────────┘
```

---

### 23. Article Detail Screen

**Ruta**: `Library > ArticleDetail`
**Tipo**: Client Component

- Título + imagen de portada
- Contenido renderizado (markdown nativo)
- Opciones multimedia: audio (expo-av), video
- Selector de idioma (si el artículo está traducido)
- Botón "Descargar para leer offline"
- Botón "Compartir" (expo-sharing)
- Artículos relacionados al final
- Fuentes científicas / referencias

---

### 24. Myths vs Reality Screen

**Ruta**: `Library > MythsVsReality`
**Tipo**: Client Component

- Tarjetas interactivas con swipe (react-native-gesture-handler)
- Anverso: "Mito" — creencia popular
- Reverso: "Realidad" — explicación científica
- Ejemplos:
  - "Si tienes la regla no puedes bañarte" → "Es seguro, el agua caliente alivia los cólicos"
  - "La menopausia empieza a los 50" → "Puede iniciar entre los 45-55, pero cada mujer es diferente"
  - "No puedes quedar embarazada si estás amamantando" → "La lactancia NO es método anticonceptivo confiable"
- Botón "Guardar" / "Compartir"
- Contador de mitos vistos: "12/30"
- Categorización: Ciclo / Embarazo / Menopausia / Sexualidad

---

### 25. Glossary Screen

**Ruta**: `Library > Glossary`
**Tipo**: Client Component

- SearchBar para buscar términos
- Lista alfabética con índice lateral (A-Z)
- Cada término expandible: definición simple + explicación
- Términos incluidos: endometriosis, SOP, prolactina, estrógeno, progesterona, FSH, LH, menarca, climaterio, etc.
- Opción de audio para pronunciación

---

## Pantallas de Acompañamiento

### 26. Family Dashboard Screen

**Ruta**: `Family > Dashboard`
**Tipo**: Client Component

**Hook**: `useFamily()`

- Lista de acompañantes invitados con estado:
  - 👤 María (pareja) — Activo
  - 👤 Ana (madre) — Pendiente
  - 👤 Sofía (hermana) — Activo
- Botón "Invitar nuevo acompañante"
- Calendario compartido (próximas citas)
- Últimos mensajes del foro familiar

---

### 27. Invite Family Screen

**Ruta**: `Family > Invite`
**Tipo**: Client Component

- Formulario: nombre del familiar + correo electrónico
- Selección de parentesco: Pareja, Madre, Hermana, Hija, Otro
- Botón "Enviar invitación por correo"
- Botón "Compartir por WhatsApp" (deep link)
- Preview de lo que verá el acompañante

---

### 28. Shared Calendar Screen

**Ruta**: `Family > SharedCalendar`
**Tipo**: Client Component

- Vista mensual con eventos visibles para acompañantes:
  - Citas médicas
  - Semana de embarazo actual
  - Período esperado
  - Fechas importantes (ecografías, exámenes)
- Eventos marcados con iconos

---

### 29. Family Chat Screen

**Ruta**: `Family > Chat`
**Tipo**: Client Component

- Chat grupal entre usuaria y sus acompañantes
- Mensajes de texto + opción de adjuntar recursos
- Notificaciones de apoyo predefinidas: "¡Estoy pensando en ti!", "¿Cómo te sientes hoy?"
- Sin mensajes ofensivos (filtro básico)

---

## Pantallas de Directorio

### 30. Directory Map Screen

**Ruta**: `Directory > Map`
**Tipo**: Client Component

**Hook**: `useDirectory()`, `useLocation()`

- Mapa interactivo (react-native-maps) con marcadores
- Categorías de marcadores:
  - 🩺 Ginecólogas
  - 👶 Obstetras
  - 🤱 Parteras
  - 🧠 Psicólogas
  - 🥗 Nutricionistas
  - 🏥 Centros de salud
- Cerca de mí (GPS location)
- Lista debajo del mapa con resultados

---

### 31. Professional Detail Screen

**Ruta**: `Directory > ProfessionalDetail`
**Tipo**: Client Component

- Foto del profesional
- Nombre + especialidad
- Experiencia (años)
- Ubicación + mapa
- Calificación de pacientes (⭐)
- Costo aproximado
- Idiomas que habla (español, lengua originaria)
- Botón "Agendar cita" (link externo o teleconsulta)
- Botón "Llamar" / "WhatsApp"

---

### 32. Emergency Contacts Screen

**Ruta**: `Directory > Emergency`
**Tipo**: Client Component

- Líneas de ayuda con botón de llamada directa:
  - 🚨 911 — Emergencias
  - 👩‍⚕️ Línea de violencia de género
  - 🧠 Línea de salud mental
  - 💜 Línea de soporte emocional
- Centros de ayuda cercanos (mapa)
- Botón "Contacto de emergencia" (configurable en perfil)

---

## Pantallas de Comunidad

### 33. Forum List Screen

**Ruta**: `Community > ForumList`
**Tipo**: Client Component

**Hook**: `useForum()`

- Tabs por etapa: 📌 General | 🌸 Ciclo | 👶 Embarazo | 🦋 Menopausia
- Lista de posts con:
  - Avatar + nombre de usuaria (anonimizado si elige)
  - Título
  - Preview del contenido (2 líneas)
  - Reacciones: 💜 🤗 💪
  - Número de respuestas
  - Fecha (hace X horas/días)
- FAB "Nuevo Post" (floating action button)

---

### 34. Forum Detail Screen

**Ruta**: `Community > ForumDetail`
**Tipo**: Client Component

- Post completo con reacciones
- Comentarios en hilo
- Input para nuevo comentario
- Opción de reportar (moderación)
- Reacciones solo positivas (💜 🤗 💪 🌟)
- Sin sistema de votos negativos

---

### 35. Create Post Screen

**Ruta**: `Community > CreatePost`
**Tipo**: Client Component

- Selector de etapa/categoría
- Título del post
- Contenido (textarea)
- Opción de publicar anónimamente
- Botón "Publicar"
- Filtro automático de contenido inapropiado

---

### 36. Stories Screen

**Ruta**: `Community > Stories`
**Tipo**: Client Component

- Grid de tarjetas con "Historias Destacadas"
- Cada historia: título + preview + ilustración
- Detalle: testimonio completo
- Anonimato respetado (nombres cambiados)
- Categorías: Superación, Descubrimiento, Comunidad, Consejos
- Botón "Compartir mi historia" (formulario para enviar)

---

## Pantallas de Perfil y Configuración

### 37. Profile Screen

**Ruta**: `Profile > MyProfile`
**Tipo**: Client Component

**Hook**: `useProfile()`

```
┌─────────────────────────────────┐
│  Avatar (foto o iniciales)      │
│  Nombre completo                 │
│  Correo electrónico              │
│  Etapa actual                    │
│  Botón "Cambiar etapa"          │
├─────────────────────────────────┤
│  Lista de opciones (SettingItem) │
│  🏥 Mi Salud                    │
│  👨‍👩‍👧‍👦 Mis Acompañantes         │
│  🔔 Notificaciones              │
│  🌐 Idioma                      │
│  🔒 Privacidad y Seguridad      │
│  📊 Reporte de Salud            │
│  ℹ️ Acerca de                   │
├─────────────────────────────────┤
│  Botón "Cerrar sesión"          │
│  (rojo, al final)               │
└─────────────────────────────────┘
```

---

### 38. Health Profile Screen

**Ruta**: `Profile > HealthProfile`
**Tipo**: Client Component

- Información personal: fecha de nacimiento, ubicación
- Historial médico: condiciones preexistentes
  - Endometriosis
  - SOP (Síndrome de Ovario Poliquístico)
  - Diabetes gestacional
  - Miomas
  - Problemas de tiroides
- Alergias (textarea)
- Medicamentos actuales (lista)
- Cirugías previas
- Historial de embarazos (número)
- Vacunas (VPH, influenza, Tdap)

---

### 39. Settings Screen

**Ruta**: `Profile > Settings`
**Tipo**: Client Component

- Notificaciones:
  - Recordatorio de período
  - Recordatorio de anticonceptivo
  - Citas médicas
  - Contenido nuevo
  - Afirmaciones diarias
  - Modo no molestar (horario)
- Idioma: selector de idioma
- Apariencia: tema claro/oscuro (futuro)
- Privacidad: control de datos compartidos
- Sonido: tono de notificación

---

### 40. Privacy Screen

**Ruta**: `Profile > Privacy`
**Tipo**: Client Component

- Control de datos compartidos:
  - ¿Compartir datos del ciclo con acompañantes?
  - ¿Compartir citas médicas con acompañantes?
  - ¿Compartir ubicación para directorio?
- Exportar mis datos (descargar JSON/PDF)
- Eliminar cuenta (con confirmación)
- Política de privacidad (link)
- Términos y condiciones (link)

---

### 41. Health Report Screen

**Ruta**: `Profile > HealthReport`
**Tipo**: Client Component

**Hook**: `useHealthReport()`

- Selector de período: Este mes | Últimos 3 meses | Último año
- Resumen mensual:
  - Ciclos registrados
  - Síntomas más frecuentes
  - Estado de ánimo predominante
  - Calidad del sueño promedio
- Gráficos de tendencia
- Botón "Exportar PDF" (expo-print)
- Botón "Compartir con mi médico"
- Botón "Descargar CSV" (datos crudos)

---

### 42. About Screen

**Ruta**: `Profile > About`
**Tipo**: Client Component

- Logo + nombre de la app
- Versión
- Descripción de la app
- Créditos
- Contacto: correo de soporte
- Términos y condiciones
- Política de privacidad
- Licencias de código abierto
