# Frontend Stack

Stack tecnológico del frontend de LUNA.

---

## Core Technologies

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **React Native** | 0.76+ | Framework mobile cross-platform (iOS + Android) |
| **Expo** | SDK 52+ | Managed workflow, builds, OTA updates |
| **React** | 19+ | UI Library |
| **TypeScript** | 5+ | Tipado estricto (`strict: true`, sin `any`) |
| **pnpm** | latest | Package manager |

## Navegación

| Librería | Propósito |
|----------|-----------|
| **@react-navigation/native** | Core de navegación |
| **@react-navigation/native-stack** | Stack navigator nativo (transiciones por plataforma) |
| **@react-navigation/bottom-tabs** | Tab navigator para navegación inferior |
| **@react-navigation/drawer** | Drawer para menú lateral (opcional) |

## State Management

| Capa | Herramienta | Para qué |
|------|-------------|----------|
| **Estado local** | `useState` / `useReducer` | Estado de un componente específico |
| **Estado de feature** | `Context` + `useReducer` | Estado compartido dentro de una feature (ej: ciclo actual) |
| **Estado global** | **Zustand** 5+ | Estado compartido entre features (auth, perfil, UI) |
| **Persistencia** | AsyncStorage + zustand/middleware | Datos offline, preferencias |

> **No se usa React Query** — los hooks manejan el estado de fetching manualmente con `useState` + `useEffect`.

## Data Fetching

| Aspecto | Decisión |
|---------|----------|
| **HTTP client** | `fetch` nativo — sin axios, sin React Query |
| **API layer** | Funciones independientes en `shared/api/` — 1 archivo por recurso |
| **Mappers** | Funciones puras en `shared/lib/mappers.ts` para transformar API → UI |

## UI & Components

| Librería | Propósito |
|----------|-----------|
| **lucide-react-native** | Iconos SVG (tree-shakeable) |
| **react-native-chart-kit** | Gráficos y charts para dashboards de salud |
| **react-native-maps** | Mapas para directorio de profesionales |
| **expo-av** | Reproducción de audio/video para contenido educativo |
| **expo-file-system** | Descarga de contenido offline |
| **expo-local-authentication** | Autenticación biométrica (huella / Face ID) |
| **expo-notifications** | Notificaciones push locales y remotas |
| **expo-sharing** | Compartir contenido (artículos, reportes) |
| **react-native-pdf** | Visualización de PDFs (reportes exportados) |
| **react-native-reanimated** | Animaciones fluidas (transiciones, swipe cards) |
| **react-native-gesture-handler** | Gestos (swipe en Mitos vs Realidad) |

## Dependencias Opcionales

Agregar según necesidad del proyecto:

```json
{
  "react-native-push-notification": "^8.1.1",
  "@stripe/stripe-react-native": "^0.40.0",
  "react-native-video": "^6.0.0",
  "react-native-webview": "^13.0.0"
}
```

## Dev Dependencies

```json
{
  "typescript": "^5",
  "jest": "^29",
  "@testing-library/react-native": "^12",
  "eslint": "^9",
  "prettier": "^3"
}
```

## Stack Decisiones Técnicas

| Decisión | Elegido | Por qué |
|----------|---------|---------|
| Framework | React Native + Expo | Cross-platform, OTA updates, managed workflow |
| Navegación | React Navigation 7 | Estándar en RN, Native Stack, soporte profundo |
| Estado global | Zustand | ~1KB, sin boilerplate, persist middleware |
| Estado feature | Context + useReducer | Scope limitado, sin dependencias extra |
| Iconos | lucide-react-native | Tree-shakeable, buenos defaults |
| API calls | fetch nativo | 0KB, suficiente para el 95% de casos |
| Charts | react-native-chart-kit | Declarativo, soporte para gráficos de salud |
| Mapas | react-native-maps | Mapas nativos, clustering, marcadores |
| Notificaciones | expo-notifications + FCM | Push nativas, scheduling local |
| Biometría | expo-local-authentication | API unificada para fingerprint/Face ID |
| Offline | expo-file-system + AsyncStorage | Descarga de contenido y datos offline |
| Package manager | pnpm | Rápido, workspace-ready |
| Type safety | TypeScript strict | No negociable |

## Dependencies Full

```json
{
  "dependencies": {
    "expo": "~52.0.0",
    "react": "^19.0.0",
    "react-native": "^0.76.0",
    "zustand": "^5.0.0",
    "@react-navigation/native": "^7.0.0",
    "@react-navigation/native-stack": "^7.0.0",
    "@react-navigation/bottom-tabs": "^7.0.0",
    "lucide-react-native": "^0.400.0",
    "react-native-chart-kit": "^6.0.0",
    "react-native-maps": "^1.0.0",
    "react-native-reanimated": "^3.0.0",
    "react-native-gesture-handler": "^2.0.0",
    "expo-av": "~15.0.0",
    "expo-file-system": "~18.0.0",
    "expo-notifications": "~0.29.0",
    "expo-local-authentication": "~14.0.0",
    "expo-sharing": "~13.0.0",
    "expo-print": "~14.0.0",
    "@react-native-async-storage/async-storage": "^2.0.0"
  },
  "devDependencies": {
    "typescript": "^5.0.0",
    "@types/react": "~19.0.0",
    "jest": "^29.0.0",
    "@testing-library/react-native": "^12.0.0",
    "eslint": "^9.0.0",
    "prettier": "^3.0.0"
  }
}
```
