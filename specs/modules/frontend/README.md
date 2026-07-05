# Frontend Module

Frontend de **LUNA** — React Native + Expo.

## Contents

| Archivo | Descripción |
|---------|-------------|
| [01-stack.md](./01-stack.md) | Stack tecnológico, dependencias y decisiones técnicas |
| [02-design.md](./02-design.md) | Identidad visual: colores, tipografía, UI Kit, componentes |
| [03-architecture.md](./03-architecture.md) | Arquitectura: estructura de carpetas, navegación, flujo de datos |
| [04-screens.md](./04-screens.md) | Detalle de todas las pantallas y sus componentes |
| [05-quality.md](./05-quality.md) | Criterios de calidad obligatorios |
| [06-push-notifications.md](./06-push-notifications.md) | Notificaciones push y recordatorios inteligentes |

## Stack

| Componente | Tecnología |
|------------|------------|
| Framework | React Native 0.76+ con Expo SDK 52+ |
| UI Library | React 19 + TypeScript 5 strict |
| Navegación | React Navigation 7 (Native Stack + Bottom Tabs) |
| Estado global | Zustand 5 |
| Estado feature | Context + useReducer |
| API calls | fetch nativo (sin axios, sin React Query) |
| Iconos | lucide-react-native |
| Charts | react-native-chart-kit o victory-native |
| Almacenamiento local | AsyncStorage |
| Seguridad biométrica | expo-local-authentication |
| Notificaciones push | Firebase Cloud Messaging (expo-notifications) |
| Multimedia | expo-av (audio/video), expo-file-system (offline) |
| Maps | react-native-maps |
| Package manager | pnpm |
