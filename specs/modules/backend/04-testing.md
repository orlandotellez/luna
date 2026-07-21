# Testing

Estrategia de testing para el backend de LUNA.

---

## Current Status

**No test projects exist yet.** The backend has no unit, integration, or E2E tests.

The test infrastructure should be added following this strategy:

---

## Proposed Structure

```
tests/
├── Luna.UnitTests/
│   ├── Domain/                # Entity business logic
│   │   ├── CyclePredictorTests.cs
│   │   └── PregnancyCalculatorTests.cs
│   └── Application/           # Services (mocked dependencies)
│       ├── AuthServiceTests.cs
│       ├── CycleServiceTests.cs
│       ├── PregnancyServiceTests.cs
│       └── RecommendationEngineTests.cs
├── Luna.IntegrationTests/
│   ├── Api/                   # Full API with test DB
│   │   ├── AuthTests.cs
│   │   ├── CycleTests.cs
│   │   ├── PregnancyTests.cs
│   │   └── ContentTests.cs
│   └── TestFixtures/
│       └── LunaWebApplicationFactory.cs
```

## Proposed Tools

- **xUnit** — test framework (.NET standard)
- **Testcontainers** — PostgreSQL container for integration tests
- **Moq / NSubstitute** — mocking in unit tests
- **FluentAssertions** — readable assertions
- **Coverlet** — code coverage

## Proposed Goals

- Unit tests: 80%+ coverage on Domain/Application
  - CyclePredictor: predicción de ciclo, ventana fértil, fecha de ovulación
  - PregnancyCalculator: cálculo de semanas, FPP, trimestres
  - RecommendationEngine: recomendación de contenido según síntomas
- Integration: critical flows
  - Registro + login + refresh token
  - CRUD de ciclo menstrual
  - Registro de embarazo + seguimiento
  - Invitación de acompañante
- Security: test de permisos RBAC
  - Usuaria solo ve sus datos
  - Acompañante no ve datos médicos detallados
  - Admin no ve datos de salud sin auditoría

## Priority Test Cases

### Unit Tests (Domain)

1. **CyclePredictor.PredictNextCycle**
   - Ciclo regular de 28 días → próxima fecha correcta
   - Ciclo irregular → predicción con menor confianza
   - Historial vacío → null prediction
   - Ventana fértil calculada correctamente

2. **PregnancyCalculator**
   - FUM → FPP correcta (Naegele's rule)
   - Cálculo de semana gestacional
   - Determinación de trimestre
   - Validación de FUM (no futura)

### Integration Tests (API)

1. **Auth Flow**
   - Register → 201 + cookies seteadas
   - Login → 200 + cookies seteadas
   - Refresh → nuevos tokens
   - Logout → cookies limpiadas + session revocada
   - Acceso sin auth → 401

2. **Cycle CRUD**
   - Crear registro de período → 201
   - Obtener ciclo actual → 200 + datos correctos
   - Registrar síntomas → 200
   - Obtener stats → 200

3. **Family Access Control**
   - Acompañante NO puede ver síntomas detallados
   - Acompañante SÍ puede ver calendario compartido
   - Revocar acceso → acompañante ya no ve datos
