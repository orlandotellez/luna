# Backend Architecture

Arquitectura del backend LUNA API.

## Project Structure

```
Luna.sln
├── src/
│   ├── Domain/
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── Account.cs
│   │   │   ├── Session.cs
│   │   │   ├── Verification.cs
│   │   │   ├── HealthProfile.cs
│   │   │   ├── Cycle.cs
│   │   │   ├── CycleDay.cs
│   │   │   ├── Symptom.cs
│   │   │   ├── Pregnancy.cs
│   │   │   ├── PregnancyWeek.cs
│   │   │   ├── Appointment.cs
│   │   │   ├── FetalMovement.cs
│   │   │   ├── WeightLog.cs
│   │   │   ├── Contraction.cs
│   │   │   ├── BirthPlan.cs
│   │   │   ├── MenopauseSymptom.cs
│   │   │   ├── Article.cs
│   │   │   ├── ArticleTranslation.cs
│   │   │   ├── Myth.cs
│   │   │   ├── GlossaryTerm.cs
│   │   │   ├── FamilyMember.cs
│   │   │   ├── FamilyMessage.cs
│   │   │   ├── Professional.cs
│   │   │   ├── HealthCenter.cs
│   │   │   ├── ForumPost.cs
│   │   │   ├── ForumComment.cs
│   │   │   ├── Reminder.cs
│   │   │   ├── Notification.cs
│   │   │   ├── PushDevice.cs
│   │   │   ├── HealthReport.cs
│   │   │   └── AuditLog.cs
│   │   ├── Enums/
│   │   │   ├── UserRole.cs            # User, Familiar, Professional, Admin
│   │   │   ├── LifeStage.cs           # Adolescent, ActiveCycle, Pregnancy, Menopause
│   │   │   ├── CyclePhase.cs          # Menstrual, Follicular, Ovulation, Luteal
│   │   │   ├── SymptomType.cs         # Cramps, Headache, Bloating, etc.
│   │   │   ├── FlowIntensity.cs       # Light, Moderate, Heavy
│   │   │   ├── MoodType.cs            # Happy, Normal, Sad, Anxious, Irritable
│   │   │   ├── PainLevel.cs           # Scale 1-5
│   │   │   ├── AppointmentType.cs     # Prenatal, Gynecologist, Mammogram, etc.
│   │   │   ├── MenopauseSymptomType.cs # HotFlash, NightSweats, VaginalDryness, etc.
│   │   │   ├── ArticleCategory.cs     # Cycle, Pregnancy, Menopause, General, Myths
│   │   │   ├── Language.cs            # Spanish, Nahuatl, Maya, Mixtec, etc.
│   │   │   ├── FamilyRelationship.cs  # Partner, Mother, Sister, Daughter, Other
│   │   │   ├── ReminderFrequency.cs   # Daily, Weekly, Monthly, Once
│   │   │   └── NotificationType.cs    # Reminder, Content, Support, Alert
│   │   └── Exceptions/
│   │       └── AppException.cs        # Base exception with status code
│   │
│   ├── Application/
│   │   ├── Common/
│   │   │   ├── Interfaces/
│   │   │   │   ├── IUserRepository.cs
│   │   │   │   ├── IAccountRepository.cs
│   │   │   │   ├── ISessionRepository.cs
│   │   │   │   ├── IVerificationRepository.cs
│   │   │   │   ├── ICycleRepository.cs
│   │   │   │   ├── IPregnancyRepository.cs
│   │   │   │   ├── IArticleRepository.cs
│   │   │   │   ├── IFamilyRepository.cs
│   │   │   │   ├── IProfessionalRepository.cs
│   │   │   │   ├── IForumRepository.cs
│   │   │   │   ├── IReminderRepository.cs
│   │   │   │   ├── INotificationRepository.cs
│   │   │   │   ├── IPushDeviceRepository.cs
│   │   │   │   ├── IAuthService.cs
│   │   │   │   ├── ICycleService.cs
│   │   │   │   ├── IPregnancyService.cs
│   │   │   │   ├── IArticleService.cs
│   │   │   │   ├── IFamilyService.cs
│   │   │   │   ├── IDirectoryService.cs
│   │   │   │   ├── IForumService.cs
│   │   │   │   ├── IReminderService.cs
│   │   │   │   ├── INotificationService.cs
│   │   │   │   ├── IPushNotificationService.cs
│   │   │   │   ├── IEmailService.cs
│   │   │   │   ├── IFileStorageService.cs
│   │   │   │   └── IHealthReportService.cs
│   │   │   ├── Mapping/
│   │   │   │   ├── MappingUser.cs
│   │   │   │   ├── MappingCycle.cs
│   │   │   │   ├── MappingPregnancy.cs
│   │   │   │   ├── MappingArticle.cs
│   │   │   │   └── MappingFamily.cs
│   │   │   ├── Models/
│   │   │   │   ├── Auth/
│   │   │   │   │   ├── AuthResult.cs
│   │   │   │   │   ├── LoginRequest.cs
│   │   │   │   │   ├── RegisterRequest.cs
│   │   │   │   │   └── UserDto.cs
│   │   │   │   ├── Cycle/
│   │   │   │   │   ├── CycleDto.cs
│   │   │   │   │   ├── CycleDayDto.cs
│   │   │   │   │   ├── SymptomRequest.cs
│   │   │   │   │   └── CyclePredictionDto.cs
│   │   │   │   ├── Pregnancy/
│   │   │   │   │   ├── PregnancyDto.cs
│   │   │   │   │   ├── WeekInfoDto.cs
│   │   │   │   │   ├── KickLogRequest.cs
│   │   │   │   │   └── BirthPlanDto.cs
│   │   │   │   ├── Menopause/
│   │   │   │   │   ├── MenopauseDto.cs
│   │   │   │   │   └── SymptomLogDto.cs
│   │   │   │   ├── Content/
│   │   │   │   │   ├── ArticleDto.cs
│   │   │   │   │   ├── MythDto.cs
│   │   │   │   │   └── GlossaryDto.cs
│   │   │   │   ├── Family/
│   │   │   │   │   ├── FamilyMemberDto.cs
│   │   │   │   │   ├── InviteRequest.cs
│   │   │   │   │   └── FamilyMessageDto.cs
│   │   │   │   ├── Directory/
│   │   │   │   │   ├── ProfessionalDto.cs
│   │   │   │   │   └── HealthCenterDto.cs
│   │   │   │   ├── Community/
│   │   │   │   │   ├── ForumPostDto.cs
│   │   │   │   │   └── ForumCommentDto.cs
│   │   │   │   ├── Reminders/
│   │   │   │   │   ├── ReminderDto.cs
│   │   │   │   │   └── ReminderRequest.cs
│   │   │   │   └── Notifications/
│   │   │   │       └── NotificationDto.cs
│   │   │   └── Authorization/
│   │   │       └── RolePermissions.cs
│   │   └── Features/
│   │       ├── Auth/
│   │       │   └── AuthService.cs
│   │       ├── Users/
│   │       │   └── UserService.cs
│   │       ├── Cycle/
│   │       │   ├── CycleService.cs
│   │       │   └── CyclePredictor.cs       # Lógica de predicción de ciclo
│   │       ├── Pregnancy/
│   │       │   └── PregnancyService.cs
│   │       ├── Menopause/
│   │       │   └── MenopauseService.cs
│   │       ├── Content/
│   │       │   ├── ArticleService.cs
│   │       │   └── RecommendationEngine.cs # Recomendación de contenido según síntomas
│   │       ├── Family/
│   │       │   └── FamilyService.cs
│   │       ├── Directory/
│   │       │   └── DirectoryService.cs
│   │       ├── Community/
│   │       │   ├── ForumService.cs
│   │       │   └── ModerationService.cs
│   │       ├── Reminders/
│   │       │   └── ReminderService.cs
│   │       └── Reports/
│   │           └── HealthReportService.cs
│   │
│   ├── Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── LunaDbContext.cs
│   │   │   ├── Configurations/           # IEntityTypeConfiguration<T>
│   │   │   │   ├── UserConfiguration.cs
│   │   │   │   ├── CycleConfiguration.cs
│   │   │   │   ├── PregnancyConfiguration.cs
│   │   │   │   ├── ArticleConfiguration.cs
│   │   │   │   ├── FamilyMemberConfiguration.cs
│   │   │   │   ├── ProfessionalConfiguration.cs
│   │   │   │   ├── ForumPostConfiguration.cs
│   │   │   │   └── ... (cada entidad)
│   │   │   ├── Repositories/
│   │   │   │   ├── UserRepository.cs
│   │   │   │   ├── CycleRepository.cs
│   │   │   │   ├── PregnancyRepository.cs
│   │   │   │   ├── ArticleRepository.cs
│   │   │   │   ├── FamilyRepository.cs
│   │   │   │   ├── ProfessionalRepository.cs
│   │   │   │   └── ... (cada repositorio)
│   │   │   ├── DataSeeder.cs
│   │   │   └── Migrations/
│   │   └── Services/
│   │       ├── TokenService.cs
│   │       ├── PasswordService.cs
│   │       ├── PushNotificationService.cs
│   │       ├── EmailService.cs
│   │       └── FileStorageService.cs
│   │
│   └── Api/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── UsersController.cs
│       │   ├── CycleController.cs
│       │   ├── PregnancyController.cs
│       │   ├── MenopauseController.cs
│       │   ├── ArticlesController.cs
│       │   ├── MythsController.cs
│       │   ├── GlossaryController.cs
│       │   ├── FamilyController.cs
│       │   ├── DirectoryController.cs
│       │   ├── ForumController.cs
│       │   ├── RemindersController.cs
│       │   ├── NotificationsController.cs
│       │   ├── ReportsController.cs
│       │   └── AdminController.cs
│       ├── Middleware/
│       │   └── ErrorHandlingMiddleware.cs
│       ├── Helpers/
│       │   ├── AuthHelper.cs
│       │   └── CookieHelper.cs
│       ├── Authorization/
│       │   ├── PermissionHandler.cs
│       │   ├── PermissionRequirement.cs
│       │   └── RequirePermissionAttribute.cs
│       └── Program.cs
└── *.slnx                            # Solution file (new .NET 10 format)
```

## Error Handling

### AppException

```csharp
public class AppException : Exception
{
    public int StatusCode { get; }
    public string Code { get; }

    // Factory methods via static class:
    // AppExceptions.NotFound("msg")     → 404
    // AppExceptions.BadRequest("msg")   → 400
    // AppExceptions.Unauthorized("msg") → 401
    // AppExceptions.Forbidden("msg")    → 403
    // AppExceptions.Conflict("msg")     → 409
    // AppExceptions.Unprocessable("msg")→ 422
}
```

### Global Exception Middleware

Returns **ProblemDetails** (RFC 7807):

```json
{
  "type": "https://tools.ietf.org/html/rfc7807",
  "title": "Not Found",
  "status": 404,
  "detail": "User with id 'xxx' not found",
  "errors": null
}
```

## Clean Architecture Layers

| Layer | Responsibility |
|-------|---------------|
| **Domain** | Entities, Enums, Exceptions — zero dependencies |
| **Application** | Services, Interfaces, DTOs, Mapping — depends only on Domain |
| **Infrastructure** | EF Core, Repositories, External Services — depends on Application + Domain |
| **API** | Controllers, Middleware, DI registration — depends on Infrastructure + Application |

## Services Layer (replaces CQRS)

Instead of MediatR commands/queries, the backend uses direct service injection:

```
Controller → IService → Service → IRepository → Repository → DbContext
```

Each service method is a self-contained operation:
- `AuthService.RegisterAsync()` — validates, creates user, generates tokens
- `CycleService.LogSymptomAsync()` — validates, records symptom, updates cycle data
- `PregnancyService.RegisterKickAsync()` — registers fetal movement, checks alert thresholds
- `ArticleService.GetRecommendedAsync()` — returns articles based on user's stage and symptoms

## Cycle Predictor

La lógica de predicción del ciclo menstrual vive en `CyclePredictor.cs` en Application layer:

```csharp
public class CyclePredictor
{
    public CyclePrediction PredictNextCycle(IEnumerable<Cycle> history)
    {
        // Calcula duración promedio del ciclo
        // Calcula duración promedio del período
        // Estima ventana fértil (días 9-19 del ciclo o método billings)
        // Estima fecha de ovulación
        // Retorna CyclePrediction con fechas estimadas y nivel de confianza
    }
}
```
