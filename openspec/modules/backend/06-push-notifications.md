# Push Notifications (Backend) ⏳ Planned

Sistema de notificaciones push con Firebase Cloud Messaging (FCM) — backend.

---

## Status

**⏳ Planned / Future feature.** El backend tiene los endpoints de registro de dispositivos y el servicio de notificaciones está diseñado pero no implementado.

---

## Architecture

```
Frontend (React Native)
    │
    │  POST /notifications/register-device
    ▼
ASP.NET Core API
    │
    │  FirebaseAdmin SDK
    ▼
Firebase Cloud Messaging (FCM)
    │
    ├──► iOS (APNs via FCM)
    └──► Android (FCM direct)
```

## Dependencies

Api.csproj
```xml
<PackageReference Include="FirebaseAdmin" Version="3.0.0" />
```

## PushNotificationService

```csharp
public interface IPushNotificationService
{
    Task SendToUserAsync(Guid userId, NotificationPayload payload);
    Task SendToMultipleAsync(IEnumerable<Guid> userIds, NotificationPayload payload);
    Task SendToTopicAsync(string topic, NotificationPayload payload);
}

public class NotificationPayload
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string? ImageUrl { get; set; }
    public Dictionary<string, string>? Data { get; set; } // Datos para deep linking
}
```

## Notification Triggers

| Disparador | Destination | Payload |
|-----------|-------------|---------|
| Period predicted soon | Usuaria | `{ type: "period-reminder", date: "..." }` |
| Pill reminder time | Usuaria | `{ type: "pill" }` |
| Appointment in 24h | Usuaria + Acompañantes | `{ type: "appointment", appointmentId }` |
| Low fetal activity | Usuaria | `{ type: "fetal-alert" }` |
| New family message | Usuaria | `{ type: "family-message" }` |
| Article recommended | Usuaria | `{ type: "article", articleId }` |
| Support reminder | Acompañante | `{ type: "support-reminder", userId }` |

## Firebase Configuration

El archivo de configuración de Firebase (`firebase-adminsdk.json`) se carga desde variables de entorno o secretos:

```json
{
  "Firebase": {
    "ProjectId": "luna-health-app",
    "CredentialsPath": "/etc/secrets/firebase-adminsdk.json"
  }
}
```

Registration in Program.cs:

```csharp
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(config["Firebase:CredentialsPath"]),
    ProjectId = config["Firebase:ProjectId"],
});
```

## Scheduled Notifications

Recordatorios programados se manejan con un background service:

```csharp
public class ReminderBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Cada minuto: verificar recordatorios que deben dispararse
            var now = DateTime.UtcNow;
            var dueReminders = await _reminderRepository.GetDueRemindersAsync(now);

            foreach (var reminder in dueReminders)
            {
                await _pushNotificationService.SendToUserAsync(
                    reminder.UserId,
                    new NotificationPayload
                    {
                        Title = reminder.Title,
                        Body = reminder.Description,
                        Data = new() { ["type"] = reminder.Type.ToString() }
                    }
                );

                await _reminderRepository.UpdateLastTriggeredAsync(reminder.Id, now);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
```
