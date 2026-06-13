# Quality Criteria

Criterios de calidad obligatorios para el frontend de LUNA.

---

## Code Quality
- **Zero mocks** in production — all data from PostgreSQL backend
- **TypeScript strict**: no `any`, no `@ts-ignore`
- **StyleSheet co-location**: estilos en archivos `.styles.ts` al lado del componente
- **No inline styles**: todos los estilos en StyleSheet.create()
- **No console.log** en producción (usar logging service)

## UX States
Toda pantalla que carga datos debe manejar estos 4 estados:

| Estado | Componente | Acción |
|--------|-----------|--------|
| **Loading** | `LoadingState` (spinner/skeleton) | — |
| **Error** | `ErrorState` (mensaje + retry) | `fetch()` callback |
| **Empty** | `EmptyState` (mensaje contextual) | CTA para crear/explorar |
| **Success** | Datos renderizados | — |

## Responsiveness
- Adaptado a todos los tamaños de pantalla (iPhone SE a Pro Max, Android pequeño a tablet)
- SafeAreaView en todas las pantallas
- Dynamic Type / Font Scaling respetado
- Landscape mode soportado (al menos en modo lectura)

## Accessibility
- **VoiceOver / TalkBack** labels en todos los elementos interactivos
- `accessibilityLabel` en iconos y botones
- `accessibilityRole` correcto (button, header, image, etc.)
- Contraste suficiente (WCAG AA mínimo)
- Touch targets mínimos de 44x44pt (HIG)
- Soporte para Dynamic Type (tamaño de fuente del sistema)
- Animaciones reducidas si el sistema lo solicita (`prefers-reduced-motion`)

## Performance
- **FlatList** con `getItemLayout` para listas largas (no ScrollView)
- **Image caching** con expo-image o FastImage
- **Lazy loading** de pantallas (React.lazy / Suspense)
- **Debounce** en búsquedas (300ms)
- **Memoización** con `useMemo` y `useCallback` en renders costosos
- **Avoid re-renders**: componentes puros con `React.memo` cuando sea necesario
- Offline first: cache de respuestas API en AsyncStorage

## Data Privacy
- Datos de salud NUNCA se almacenan en localStorage/AsyncStorage sin encriptar
- Datos sensibles solo en memoria (Zustand store, no persistente)
- Opción de "modo incógnito" para contenido sensible
- Borrado automático de datos locales al cerrar sesión
- Consentimiento explícito para compartir datos

## Offline Support
- Contenido educativo descargable para lectura offline
- Registro de síntomas offline (cola de sincronización)
- Datos del ciclo cacheados localmente
- Indicador de "sin conexión" en la UI
- Sincronización automática cuando vuelve la conexión

## Error Handling
- Error boundaries en todos los navigators
- Manejo de errores en hooks: `try/catch` + `setError`
- Mensajes de error claros en español y lenguas originarias
- Timeout en peticiones API (10s default)
- Retry automático en fallos de red (3 intentos)

## Internationalization (i18n)
- Todos los textos de UI en archivos de traducción (i18n)
- Soporte inicial: español + 6 lenguas originarias
- Fechas en formato local
- Números (decimales, calendarios) según región

## Privacy by Design
- Datos de ciclo menstrual NUNCA compartidos sin consentimiento explícito
- Opción de anonimato en foros
- Exportación y borrado de datos (GDPR-style)
- Mínimo permiso necesario en cada funcionalidad
- Auditoría de acceso a datos por parte de acompañantes
