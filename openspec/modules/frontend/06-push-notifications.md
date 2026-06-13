# Push Notifications & Reminders

Notificaciones push y recordatorios inteligentes con Firebase Cloud Messaging (FCM).

---

## Architecture

- **Push Provider**: Firebase Cloud Messaging (FCM)
- **Local Notifications**: expo-notifications API (scheduling local)
- **Backend**: ASP.NET Core 10 con Firebase Admin SDK para envío server-side
- **Device Token Registration**: se registra al login, se actualiza periódicamente

```
Frontend (RN) ←── FCM ──→ Backend (ASP.NET)
     │                            │
     │  expo-notifications        │  FirebaseAdmin SDK
     │  (local scheduling)        │  (server push)
     └────────────────────────────┘
```

---

## Notification Types

### Recordatorios Programados (locales + server)

| Tipo | Cuándo | Canal |
|------|--------|-------|
| Próximo período | 2 días antes de la fecha estimada | Push + In-App |
| Tomar anticonceptivo | Diario a la hora configurada | Push |
| Cita médica | 24h antes | Push + Email |
| Examen de rutina | Según programación (mamografía anual, etc.) | Push |
| Afirmación diaria | Cada mañana (9:00 AM) | Push (opcional) |
| Vacunación | Recordatorio VPH, influenza, Tdap | Push |

### Notificaciones de Contenido

| Tipo | Disparador | Canal |
|------|-----------|-------|
| Artículo recomendado | Según etapa y síntomas registrados | Push + In-App |
| Nueva historia | Cuando se publica en comunidad | Push |
| Respuesta en foro | Cuando alguien responde tu post | Push |
| Nuevo mensaje familiar | Cuando un acompañante escribe | Push |

### Notificaciones de Apoyo (Acompañamiento)

| Tipo | Disparador | Canal |
|------|-----------|-------|
| Recordatorio de apoyo | "Envía un mensaje de ánimo" | Push al acompañante |
| Cita próxima | Recordatorio al acompañante | Push |
| Semana de embarazo | Cambio de semana gestacional | Push |

### Notificaciones de Alertas

| Tipo | Disparador | Canal |
|------|-----------|-------|
| Actividad fetal baja | < 10 movimientos en 2 horas | Push + Alerta |
| Síntomas severos | Dolor intenso 3 ciclos seguidos | Push + Recomendación |
| Signos de alerta postparto | Basado en registro de síntomas | Push + Contacto médico |

---

## Frontend Integration

### Dependencia

```json
{
  "expo-notifications": "~0.29.0"
}
```

### Permisos

```typescript
// shared/utils/notifications.ts
import * as Notifications from 'expo-notifications';
import * as Device from 'expo-device';
import Constants from 'expo-constants';

export async function registerForPushNotifications(): Promise<string | null> {
  if (!Device.isDevice) {
    return null; // Notifications only on physical devices
  }

  const { status: existingStatus } = await Notifications.getPermissionsAsync();
  let finalStatus = existingStatus;

  if (existingStatus !== 'granted') {
    const { status } = await Notifications.requestPermissionsAsync();
    finalStatus = status;
  }

  if (finalStatus !== 'granted') {
    return null;
  }

  const token = (await Notifications.getExpoPushTokenAsync({
    projectId: Constants.expoConfig?.extra?.eas?.projectId,
  })).data;

  return token;
}
```

### Channel Configuration

```typescript
// App.tsx — al inicio
import * as Notifications from 'expo-notifications';

Notifications.setNotificationHandler({
  handleNotification: async () => ({
    shouldShowAlert: true,
    shouldPlaySound: true,
    shouldSetBadge: true,
  }),
});

// Crear canales (Android)
async function setupChannels() {
  await Notifications.setNotificationChannelAsync('cycle', {
    name: 'Ciclo Menstrual',
    description: 'Recordatorios de período y fertilidad',
    importance: Notifications.AndroidImportance.HIGH,
  });

  await Notifications.setNotificationChannelAsync('appointments', {
    name: 'Citas Médicas',
    description: 'Recordatorios de citas y exámenes',
    importance: Notifications.AndroidImportance.HIGH,
  });

  await Notifications.setNotificationChannelAsync('content', {
    name: 'Contenido',
    description: 'Artículos recomendados y afirmaciones',
    importance: Notifications.AndroidImportance.DEFAULT,
  });

  await Notifications.setNotificationChannelAsync('family', {
    name: 'Acompañamiento',
    description: 'Notificaciones de acompañantes',
    importance: Notifications.AndroidImportance.HIGH,
  });
}
```

### Scheduling Local Notifications

```typescript
// features/reminders/hooks/useReminders.ts
import * as Notifications from 'expo-notifications';

export function useReminders() {
  const schedulePillReminder = async (time: string) => {
    const [hours, minutes] = time.split(':').map(Number);

    await Notifications.cancelAllScheduledNotificationsAsync(); // limpiar anteriores

    await Notifications.scheduleNotificationAsync({
      content: {
        title: '💊 Tu anticonceptivo',
        body: 'Es hora de tomar tu anticonceptivo diario',
        data: { type: 'pill' },
      },
      trigger: {
        type: Notifications.SchedulableTriggerInputTypes.DAILY,
        hour: hours,
        minute: minutes,
      },
    });
  };

  const schedulePeriodReminder = async (nextPeriodDate: Date) => {
    const reminderDate = new Date(nextPeriodDate);
    reminderDate.setDate(reminderDate.getDate() - 2); // 2 días antes

    await Notifications.scheduleNotificationAsync({
      content: {
        title: '🌸 Tu período se acerca',
        body: 'Esperamos tu período en los próximos días. Prepárate.',
        data: { type: 'period-reminder' },
      },
      trigger: {
        type: Notifications.SchedulableTriggerInputTypes.DATE,
        date: reminderDate,
      },
    });
  };

  return { schedulePillReminder, schedulePeriodReminder };
}
```

### Handle Notification Taps

```typescript
// App.tsx
import * as Notifications from 'expo-notifications';
import { useNavigation } from '@react-navigation/native';

export default function App() {
  const navigation = useNavigation();

  useEffect(() => {
    // Cuando la app está en background y tocan la notificación
    const subscription = Notifications.addNotificationResponseReceivedListener(response => {
      const data = response.notification.request.content.data;

      switch (data.type) {
        case 'pill':
          navigation.navigate('Reminders');
          break;
        case 'period-reminder':
          navigation.navigate('Stage', { screen: 'Calendar' });
          break;
        case 'article':
          navigation.navigate('Library', { screen: 'ArticleDetail', params: { id: data.articleId } });
          break;
        case 'family-message':
          navigation.navigate('Family', { screen: 'Chat' });
          break;
      }
    });

    return () => subscription.remove();
  }, []);
}
```

---

## Backend Integration (Firebase Admin SDK)

El backend envía notificaciones push usando Firebase Admin SDK.

### Server Endpoint

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/notifications/register-token` | Registrar token FCM del dispositivo |
| DELETE | `/notifications/unregister-token` | Eliminar token al logout |
| GET | `/notifications` | Listar notificaciones del usuario |
| PATCH | `/notifications/{id}/read` | Marcar como leída |
| PATCH | `/notifications/read-all` | Marcar todas como leídas |

### Request: Register Token

```json
POST /api/v1/notifications/register-token
{
  "token": "ExponentPushToken[xxxxxxxxxxxxxx]",
  "platform": "ios" | "android"
}
```

---

## Do Not Disturb Mode

Configurable desde Settings:

```typescript
interface DoNotDisturbConfig {
  enabled: boolean;
  startTime: string;  // "22:00"
  endTime: string;    // "07:00"
  allowCritical: boolean; // permitir alertas de salud aunque DND activo
}
```

Las notificaciones en modo DND se silencian pero se guardan en la bandeja de notificaciones in-app.

---

## Notification Store (Zustand)

```typescript
// shared/store/useNotificationStore.ts
import { create } from 'zustand';
import { persist } from 'zustand/middleware';

interface NotificationState {
  pushToken: string | null;
  dndConfig: DoNotDisturbConfig;
  unreadCount: number;
  setPushToken: (token: string | null) => void;
  setDndConfig: (config: Partial<DoNotDisturbConfig>) => void;
  incrementUnread: () => void;
  resetUnread: () => void;
}

export const useNotificationStore = create<NotificationState>()(
  persist(
    (set) => ({
      pushToken: null,
      dndConfig: { enabled: false, startTime: '22:00', endTime: '07:00', allowCritical: true },
      unreadCount: 0,
      setPushToken: (token) => set({ pushToken: token }),
      setDndConfig: (config) => set((state) => ({ dndConfig: { ...state.dndConfig, ...config } })),
      incrementUnread: () => set((state) => ({ unreadCount: state.unreadCount + 1 })),
      resetUnread: () => set({ unreadCount: 0 }),
    }),
    { name: 'notification-store' }
  )
);
```
