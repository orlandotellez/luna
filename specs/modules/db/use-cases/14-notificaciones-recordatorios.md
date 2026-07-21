# 14. Notificaciones y Recordatorios

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
