# Aplicación Móvil para el Acompañamiento Integral a Mujeres

## Descripción del Proyecto

Aplicación móvil integral para el acompañamiento de la salud femenina a lo largo de todas las etapas de la vida: **menstruación**, **embarazo** y **menopausia**. Proporciona información confiable y contextualizada, herramientas de seguimiento personalizado, recordatorios inteligentes y contenido educativo en español y lenguas originarias para reducir mitos y desinformación. Incluye un módulo de acompañamiento familiar que permite a los familiares cercanos comprender y apoyar a la mujer en cada etapa, promoviendo el autocuidado y el bienestar físico y emocional desde el hogar y la comunidad.

---

## Actores del Sistema

| Actor | Descripción |
|-------|-------------|
| **Mujer / Usuaria** | Usuario principal que registra su etapa de vida, recibe acompañamiento y contenido personalizado |
| **Familiar / Acompañante** | Persona autorizada (pareja, madre, hermana, hija) que accede a contenido de acompañamiento |
| **Profesional de la Salud** | Médico, ginecóloga, obstetra o partera que puede vincularse para dar seguimiento |
| **Administradora del Sistema** | Gestiona contenido, catálogos, moderación y configuración de la plataforma |

---

## Módulos y Funcionalidades

### Módulo 1: Registro, Autenticación y Perfil por Etapa de Vida
- **Registro de usuaria**: nombre, correo electrónico, teléfono, fecha de nacimiento, ubicación (departamento/municipio), idioma preferido (español, lenguas originarias)
- **Selección de etapa de vida al registrarse**:
  - **Adolescente / Juventud**: información sobre primer ciclo menstrual, cambios corporales, salud sexual y reproductiva
  - **Edad Adulta / Etapa Menstrual Activa**: seguimiento de ciclo, fertilidad, anticoncepción
  - **Embarazo / Postparto**: seguimiento semana a semana, preparación para el parto, lactancia
  - **Perimenopausia / Menopausia**: manejo de síntomas, cambios hormonales, salud ósea y cardiovascular
- **Perfil de salud personal**: historial médico, alergias, condiciones preexistentes (endometriosis, SOP, diabetes gestacional), medicamentos actuales
- **Cambio de etapa**: la usuaria puede actualizar su etapa cuando sea necesario (ej. de "menstrual activa" a "embarazo")
- **Autenticación biométrica**: huella digital o reconocimiento facial para acceso rápido
- **Multi-perfil familiar**: posibilidad de gestionar perfiles de hijas adolescentes bajo supervisión

### Módulo 2: Seguimiento del Ciclo Menstrual
- **Registro diario**: fecha de inicio y fin del período, intensidad del flujo (leve, moderado, abundante), dolor (escala 1-5), síntomas asociados (cólicos, dolor de cabeza, hinchazón, cambios de ánimo, senos sensibles)
- **Predicción de ciclo**: cálculo automático de próxima menstruación, ventana fértil, ovulación
- **Calendario menstrual**: vista mensual con código de colores (período, ventana fértil, ovulación, síntomas registrados)
- **Gráficos de tendencia**: evolución de la duración del ciclo, intensidad del flujo, síntomas recurrentes por mes
- **Notas personales**: campo de texto libre para registrar emociones, eventos, observaciones
- **Exportación de historial**: PDF o CSV para compartir con el ginecólogo

### Módulo 3: Seguimiento de Embarazo
- **Registro de embarazo**: fecha de última menstruación (FUM) o fecha probable de parto (FPP), cálculo automático de semana gestacional actual
- **Seguimiento semana a semana**:
  - Información sobre el desarrollo del bebé (tamaño, hitos, movimientos)
  - Cambios en el cuerpo de la madre por semana
  - Recomendaciones: alimentación, ejercicios, cuidados
- **Control de citas prenatales**: registro de citas programadas, recordatorio automático, checklists por trimestre (exámenes, vacunas, ecografías)
- **Registro de movimientos fetales**: contador de patadas, alerta si disminuye la actividad
- **Gráfico de peso**: seguimiento del aumento de peso gestacional con curva recomendada
- **Contracciones**: cronómetro para medir frecuencia y duración de contracciones (preparto)
- **Plan de parto**: lista de preferencias editable (tipo de parto, acompañante, analgesia, cuidados del recién nacido)
- **Postparto / Lactancia**: seguimiento de recuperación, registro de tomas de lactancia, signos de alerta

### Módulo 4: Acompañamiento en Menopausia
- **Registro de síntomas menopáusicos**: sofocos, sudores nocturnos, sequedad vaginal, insomnio, cambios de ánimo, aumento de peso, pérdida de densidad ósea
- **Escala de intensidad**: registro diario o semanal de severidad de síntomas (escala 1-5)
- **Gráfico de evolución**: visualización de la frecuencia e intensidad de síntomas en el tiempo
- **Recomendaciones personalizadas**:
  - Alimentación para menopausia (calcio, vitamina D, fitoestrógenos)
  - Ejercicios recomendados (fortalecimiento óseo, Kegel, cardio suave)
  - Manejo de sofocos y cambios de ánimo
- **Seguimiento de salud ósea**: recordatorio de densitometría, suplementos de calcio/vitamina D
- **Registro de terapia hormonal**: si aplica, seguimiento de dosis y efectos secundarios

### Módulo 5: Biblioteca de Contenido Educativo
- **Artículos organizados por etapa y tema**:
  - Ciclo menstrual: ¿qué es normal?, mitos sobre la menstruación, higiene íntima, métodos anticonceptivos
  - Embarazo: señales de alerta, nutrición prenatal, preparación para la lactancia, derechos maternos
  - Menopausia: cómo identificar el inicio, manejo sin medicación, vida sexual después de la menopausia
  - Salud general: autoexamen de mama, salud cardiovascular, salud mental
- **Formato multimedia**: texto, audio (para escuchar), video corto, infografías descargables
- **Sección "Mitos vs. Realidad"**: tarjetas interactivas que desmienten creencias populares con respaldo científico
- **Contenido en lenguas originarias**: artículos traducidos a náhuatl, maya, mixteco, zapoteco, tsotsil, otomí, etc. según la región de la usuaria
- **Glosario de términos**: definiciones simples de términos médicos (endometriosis, SOP, prolactina, etc.)
- **Modo offline**: descarga de artículos para lectura sin conexión a internet

### Módulo 6: Acompañamiento Familiar
- **Perfil de familiar acompañante**: la usuaria invita a uno o varios familiares (pareja, madre, hermana, hija mayor)
- **Contenido para acompañantes**: guías sobre cómo apoyar en cada etapa, qué decir y qué evitar, señales de alerta
- **Calendario compartido**: el familiar puede ver citas médicas próximas, fechas importantes (semana de embarazo, período esperado)
- **Notificaciones de apoyo**: recordatorio al familiar de enviar un mensaje de ánimo, acompañar a una cita, o preparar algo especial
- **Foro familiar privado**: espacio de comunicación dentro de la app entre la usuaria y sus acompañantes
- **Modo adolescente supervisado**: para madres con hijas adolescentes, la madre puede recibir notificaciones de contenido educativo sin acceso a datos privados de la hija

### Módulo 7: Directorio de Servicios de Salud
- **Búsqueda de profesionales**: ginecólogas, obstetras, parteras, psicólogas, nutricionistas por ubicación geográfica
- **Filtros**: por especialidad, tipo de servicio (consulta, teleconsulta, emergencia), horario, método de pago, idioma (español/lengua originaria)
- **Perfil del profesional**: foto, especialidad, experiencia, ubicación, calificación de pacientes, costo aproximado
- **Centros de salud públicos**: mapa con clínicas, hospitales maternos, centros de salud comunitarios
- **Líneas de ayuda**: números de emergencia, violencia de género, salud mental, soporte emocional
- **Teleconsulta integrada**: videollamada con profesionales de la salud registrados en la plataforma

### Módulo 8: Recordatorios y Notificaciones Inteligentes
- **Recordatorios personalizados**:
  - Tomar anticonceptivo diario / inyección / parche
  - Próximo período esperado
  - Cita prenatal / ginecológica / mastografía / Papanicolaou
  - Examen de rutina (densitometría, perfil hormonal, glucosa)
  - Vacunación (VPH, influenza, Tdap en embarazo)
- **Notificaciones de contenido**: artículo recomendado según etapa y síntomas registrados
- **Afirmaciones diarias**: mensaje motivacional cada mañana ("Hoy escucho a mi cuerpo", "Mi salud es mi prioridad")
- **Modo no molestar**: programar silencio nocturno o en horas laborales
- **Canales**: notificación push, correo electrónico, SMS (para recordatorios críticos)

### Módulo 9: Comunidad y Apoyo
- **Foro por etapa**: espacio de discusión para mujeres en la misma etapa (adolescentes, embarazadas, menopáusicas)
- **Hilos de conversación**: crear preguntas, compartir experiencias, consejos entre usuarias
- **Moderación automática**: filtro de contenido inapropiado, reporte de usuarios
- **Grupos locales**: por región/comunidad para organizar encuentros presenciales o virtuales
- **Historias destacadas**: testimonios de mujeres sobre su experiencia (anonimato respetado)
- **Reacciones**: emojis de apoyo, sin sistema de votos negativos para mantener un entorno positivo

### Módulo 10: Reportes y Analítica (Privacidad)
- **Resumen mensual de salud**: reporte automático con datos del ciclo, síntomas registrados, estado de ánimo predominante
- **Historial exportable**: toda la información de salud puede exportarse en PDF para compartir con el médico
- **Tendencias anuales**: gráficos de evolución de síntomas, regularidad del ciclo, peso en embarazo
- **Reporte de bienestar**: indicador compuesto basado en síntomas, estado de ánimo, calidad del sueño
- **Control de privacidad**: la usuaria decide qué información compartir, con quién y por cuánto tiempo

---

## Pantallas del Sistema

### Pantallas de Onboarding y Registro
1. **Pantalla de Bienvenida (Onboarding)** — slider con las 3 etapas: menstruación, embarazo, menopausia
2. **Selección de Etapa** — "¿En qué etapa te encuentras?" con ilustraciones y descripción breve
3. **Registro** — formulario con nombre, correo, contraseña, fecha de nacimiento, ubicación
4. **Perfil de Salud Inicial** — preguntas básicas: ¿tienes ciclo regular?, ¿estás embarazada?, ¿has llegado a la menopausia?
5. **Configuración de Idioma** — selector de idioma (español y lenguas originarias disponibles)
6. **Inicio de Sesión** — acceso con correo/contraseña o biométrico

### Pantallas Principales (Home)
1. **Dashboard / Inicio** — tarjeta de la etapa actual con información del día, frase motivacional, acceso rápido a registro de síntomas, calendario y contenido recomendado
2. **Menú de Navegación** — acceso a: Mi Ciclo, Mi Embarazo, Mi Menopausia, Biblioteca, Acompañamiento, Directorio, Comunidad, Perfil

### Pantallas por Etapa
1. **Calendario Menstrual** — vista mensual con registro de período, síntomas, ventana fértil, ovulación
2. **Registro Diario de Síntomas** — selector rápido: flujo, dolor, ánimo, sueño, actividad física
3. **Detalle del Ciclo** — gráficos de tendencia, duración del ciclo, comparativa mensual
4. **Semana de Embarazo** — información del desarrollo, checklists de citas, peso, movimientos fetales
5. **Contador de Patadas** — temporizador para registrar movimientos del bebé
6. **Registro de Síntomas de Menopausia** — escala de intensidad, frecuencia, gráfico de evolución
7. **Plan de Parto** — editor de preferencias, checklist de preparación

### Pantallas de Contenido
1. **Biblioteca** — categorías por etapa, buscador, artículos destacados
2. **Artículo / Video** — reproductor multimedia, opción de descarga, compartición, audios en lenguas originarias
3. **Mitos vs. Realidad** — tarjetas interactivas con swipe (deslizar para ver la verdad)
4. **Glosario** — buscador de términos con definiciones simples

### Pantallas de Acompañamiento
1. **Mis Acompañantes** — lista de familiares invitados, estado de invitación
2. **Invitación de Acompañante** — formulario para enviar invitación por correo o WhatsApp
3. **Contenido para Acompañantes** — guías y artículos seleccionados para familiares
4. **Calendario Compartido** — vista de fechas importantes visibles para acompañantes

### Pantallas de Comunidad y Directorio
1. **Foro por Etapa** — lista de hilos, crear nuevo hilo, buscador
2. **Detalle del Hilo** — respuestas, reacciones, opción de reportar
3. **Buscar Profesional** — mapa y lista con filtros
4. **Perfil del Profesional** — información, calificaciones, costo, agendar cita
5. **Centros de Salud** — mapa interactivo con ubicaciones y datos de contacto

### Pantallas de Perfil y Configuración
1. **Mi Perfil** — datos personales, etapa actual, cambio de etapa
2. **Mi Salud** — historial médico, condiciones, medicamentos
3. **Preferencias** — idioma, notificaciones, privacidad
4. **Privacidad y Seguridad** — control de datos compartidos, eliminar cuenta, descargar mis datos
5. **Reporte de Salud** — resumen mensual exportable
6. **Acerca de** — información de la app, contacto, términos y condiciones

---

## Casos de Uso Principales

| # | Caso de Uso | Actor | Descripción |
|---|-------------|-------|-------------|
| 1 | Registrarse en la app | Mujer | Crea su cuenta, selecciona etapa de vida e idioma preferido |
| 2 | Registrar período menstrual | Mujer | Registra inicio y fin del período, intensidad de flujo y síntomas |
| 3 | Ver predicción de próxima menstruación | Mujer | Consulta el calendario con la fecha estimada del próximo ciclo |
| 4 | Registrar síntomas diarios | Mujer | Ingresa estado de ánimo, dolor, sueño y actividad física |
| 5 | Iniciar seguimiento de embarazo | Mujer | Ingresa FUM, el sistema calcula semana gestacional y FPP |
| 6 | Registrar movimientos fetales | Mujer | Activa contador de patadas y registra frecuencia |
| 7 | Consultar desarrollo semanal del bebé | Mujer | Lee información del tamaño, hitos y cambios de la semana actual |
| 8 | Registrar síntoma menopáusico | Mujer | Selecciona tipo de síntoma e intensidad, se guarda en gráfico |
| 9 | Leer artículo de la biblioteca | Mujer | Busca por etapa o tema, lee o escucha en audio |
| 10 | Acceder a contenido en lengua originaria | Mujer | Cambia idioma desde configuración o selecciona artículo traducido |
| 11 | Invitar familiar acompañante | Mujer | Envía invitación, el familiar se registra con perfil de acompañante |
| 12 | Ver contenido como acompañante | Familiar | Accede a guías de apoyo y calendario compartido |
| 13 | Buscar ginecóloga cercana | Mujer | Filtra por ubicación, especialidad e idioma, ve perfil y agenda |
| 14 | Programar recordatorio de cita médica | Mujer | Configura alerta para Papanicolaou, mastografía o consulta |
| 15 | Participar en foro de comunidad | Mujer | Crea o responde un hilo en el foro de su etapa |
| 16 | Exportar reporte mensual de salud | Mujer | Descarga PDF con resumen del ciclo y síntomas para el médico |
| 17 | Desmentir un mito | Mujer | Abre sección "Mitos vs. Realidad", lee la explicación científica |
| 18 | Configurar privacidad de datos | Mujer | Decide qué información compartir con acompañantes y profesionales |
| 19 | Cambiar de etapa de vida | Mujer | Actualiza su etapa de "menstrual" a "embarazo" o a "menopausia" |
| 20 | Reportar contenido inapropiado | Mujer | Reporta un comentario en foro, el equipo de moderación revisa |

---

## Flujos de Navegación Clave

### Flujo de Onboarding y Primer Uso
```
Mujer descarga la app → Pantalla de bienvenida (3 etapas)
→ Selecciona su etapa actual → Registro con correo y contraseña
→ Configura perfil de salud inicial → Selecciona idioma (español / lengua originaria)
→ Accede al Dashboard personalizado
→ Recibe recomendación de primer artículo según su etapa
→ Configura su primer recordatorio (próximo período, cita médica, etc.)
```

### Flujo de Registro de Ciclo Menstrual
```
Mujer abre app → Dashboard → "Registrar período"
→ Selecciona fecha de inicio → Intensidad del flujo (leve, moderado, abundante)
→ Registra síntomas: dolor (1-5), dolor de cabeza, hinchazón, cambios de ánimo
→ Agrega nota opcional → Guardar
→ Calendario se actualiza con marcador rojo en fecha de inicio
→ Sistema recalcula predicción del próximo ciclo
→ Si registra dolor intenso 3 ciclos seguidos → Sugerencia: "Consulta a tu ginecóloga"
```

### Flujo de Inicio de Embarazo
```
Mujer cambia etapa a "Embarazo" → Ingresa fecha de última menstruación
→ Sistema calcula FPP y semana gestacional actual (ej. "Semana 8")
→ Dashboard se transforma: muestra información de la semana actual
→ Checklist de primer trimestre: "Agenda tu primera ecografía"
→ Configura recordatorio de cita prenatal
→ Activa contador de patadas (a partir de semana 24)
→ Invita a su pareja como acompañante para que reciba contenido de apoyo
```

### Flujo de Lectura de Contenido en Lengua Originaria
```
Mujer abre Biblioteca → Busca "Embarazo saludable"
→ Filtra idioma → Selecciona "Maya"
→ Artículo se muestra en maya con opción de audio
→ Escucha la narración en maya → Puede descargar para leer offline
→ Al final del artículo, botón "Compartir" → Envía por WhatsApp a familiar
```

### Flujo de Acompañamiento Familiar
```
Mujer va a Acompañamiento → "Invitar acompañante"
→ Ingresa nombre y correo de su pareja → Envía invitación
→ Pareja recibe enlace → Se registra con perfil "Acompañante"
→ Pareja accede a contenido: "Cómo apoyar a tu pareja en el embarazo"
→ Ve calendario compartido: próxima cita prenatal, semana actual
→ Recibe notificación: "Mañana es la ecografía de las 20 semanas"
→ Puede enviar mensaje de ánimo desde la app
```

### Flujo de Consulta de Mito vs. Realidad
```
Mujer abre Biblioteca → Sección "Mitos vs. Realidad"
→ Ve tarjeta: "Si tienes la regla no puedes bañarte" (Mito)
→ Desliza la tarjeta → Aparece la verdad:
"Es totalmente seguro bañarse durante la menstruación.
De hecho, el agua caliente puede aliviar los cólicos.
Solo usa protección adecuada (toalla o tampón)."
→ Puede guardar la tarjeta, compartirla o leer la fuente científica
```

### Flujo de Reporte de Síntomas Menopáusicos
```
Mujer en etapa menopausia abre app → Dashboard → "Registrar síntoma"
→ Selecciona: Sofoco → Intensidad: 4/5 → Hora: 3:00 PM
→ Agrega nota: "Comenzó después del almuerzo"
→ Guardar → Gráfico de evolución se actualiza
→ Sistema detecta patrón: sofocos más intensos en la tarde
→ Recomienda artículo: "Manejo de sofocos: alimentación y técnicas de relajación"
```
