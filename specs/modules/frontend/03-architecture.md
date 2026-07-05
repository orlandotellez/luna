# Frontend Architecture

Estructura, patrones y flujo de datos del frontend de LUNA.

Feature-First + Container-Presentational + Co-location.

---

## Arquitectura General

```
┌────────────────────────────────────────────────────────────┐
│                        App.tsx                              │
│        Providers (Navigation, Auth, Theme, Store)           │
├────────────────────────────────────────────────────────────┤
│                    Navigation Tree                          │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌───────────┐ │
│  │ Onboarding│  │   Auth   │  │   Main   │  │  Familia  │ │
│  │  (stack)  │  │  (stack) │  │ (tabs)   │  │  (stack)  │ │
│  └──────────┘  └──────────┘  └────┬─────┘  └───────────┘ │
│                                    │                       │
│  ┌─────────────────────────────────┴───────────────────┐  │
│  │                   src/features/                      │  │
│  │  ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐      │  │
│  │  │cycle │ │preg. │ │menop.│ │library│ │family│ ...  │  │
│  │  └──────┘ └──────┘ └──────┘ └──────┘ └──────┘      │  │
│  └──────────────────────┬──────────────────────────────┘  │
│                         │                                 │
│  ┌──────────────────────┴──────────────────────────────┐  │
│  │                   src/shared/                        │  │
│  │  api/  hooks/  components/  lib/  store/  types/    │  │
│  └─────────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────────┘
```

**Flujo de datos**:
```
screens → hooks (container/lógica) → api (HTTP) → Backend
        ↘ components (UI/view) ↗
```

---

## Folder Structure

```
luna-mobile/
├── app.json
├── App.tsx                                      # Entry point, providers
├── src/
│   ├── navigation/                              # Navigation configuration
│   │   ├── RootNavigator.tsx                    # Navegador raíz
│   │   ├── OnboardingNavigator.tsx              # Stack de onboarding
│   │   ├── AuthNavigator.tsx                    # Stack de autenticación
│   │   ├── MainTabNavigator.tsx                 # Bottom tabs principal
│   │   ├── FamilyNavigator.tsx                  # Stack de acompañamiento
│   │   └── types.ts                             # Tipos de navegación
│   │
│   ├── features/                                # Features de negocio
│   │   ├── auth/                                # Feature: Autenticación
│   │   │   ├── screens/
│   │   │   │   ├── OnboardingScreen.tsx
│   │   │   │   ├── StageSelectionScreen.tsx
│   │   │   │   ├── LoginScreen.tsx
│   │   │   │   ├── RegisterScreen.tsx
│   │   │   │   └── HealthProfileScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── StageCard.tsx
│   │   │   │   ├── StageCard.styles.ts
│   │   │   │   └── LanguageSelector.tsx
│   │   │   └── hooks/
│   │   │       ├── useAuth.ts
│   │   │       └── useBiometric.ts
│   │   │
│   │   ├── cycle/                               # Feature: Ciclo Menstrual
│   │   │   ├── screens/
│   │   │   │   ├── CycleDashboardScreen.tsx
│   │   │   │   ├── CalendarScreen.tsx
│   │   │   │   ├── SymptomRegisterScreen.tsx
│   │   │   │   ├── CycleDetailScreen.tsx
│   │   │   │   └── ExportReportScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── CalendarView.tsx
│   │   │   │   ├── CalendarView.styles.ts
│   │   │   │   ├── DayMarker.tsx
│   │   │   │   ├── CycleChart.tsx
│   │   │   │   ├── CycleChart.styles.ts
│   │   │   │   ├── SymptomSelector.tsx
│   │   │   │   ├── FlowSelector.tsx
│   │   │   │   └── PainScale.tsx
│   │   │   ├── hooks/
│   │   │   │   ├── useCycleData.ts
│   │   │   │   ├── useCalendar.ts
│   │   │   │   └── useSymptoms.ts
│   │   │   └── utils/
│   │   │       ├── cyclePredictor.ts
│   │   │       └── fertileWindow.ts
│   │   │
│   │   ├── pregnancy/                           # Feature: Embarazo
│   │   │   ├── screens/
│   │   │   │   ├── PregnancyDashboardScreen.tsx
│   │   │   │   ├── WeekDetailScreen.tsx
│   │   │   │   ├── AppointmentTrackerScreen.tsx
│   │   │   │   ├── KickCounterScreen.tsx
│   │   │   │   ├── WeightTrackerScreen.tsx
│   │   │   │   ├── ContractionTimerScreen.tsx
│   │   │   │   ├── BirthPlanScreen.tsx
│   │   │   │   └── PostpartumScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── WeekCard.tsx
│   │   │   │   ├── FetusInfo.tsx
│   │   │   │   ├── WeightChart.tsx
│   │   │   │   ├── AppointmentChecklist.tsx
│   │   │   │   ├── KickButton.tsx
│   │   │   │   └── ContractionLog.tsx
│   │   │   └── hooks/
│   │   │       ├── usePregnancyData.ts
│   │   │       ├── useKickCounter.ts
│   │   │       └── useAppointments.ts
│   │   │
│   │   ├── menopause/                           # Feature: Menopausia
│   │   │   ├── screens/
│   │   │   │   ├── MenopauseDashboardScreen.tsx
│   │   │   │   ├── SymptomTrackerScreen.tsx
│   │   │   │   └── BoneHealthScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── SymptomLog.tsx
│   │   │   │   ├── SymptomChart.tsx
│   │   │   │   ├── IntensitySlider.tsx
│   │   │   │   └── RecommendationCard.tsx
│   │   │   └── hooks/
│   │   │       ├── useMenopauseData.ts
│   │   │       └── useSymptomTracking.ts
│   │   │
│   │   ├── library/                             # Feature: Biblioteca
│   │   │   ├── screens/
│   │   │   │   ├── LibraryHomeScreen.tsx
│   │   │   │   ├── ArticleDetailScreen.tsx
│   │   │   │   ├── MythsVsRealityScreen.tsx
│   │   │   │   └── GlossaryScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── ArticleCard.tsx
│   │   │   │   ├── CategoryGrid.tsx
│   │   │   │   ├── MythCard.tsx
│   │   │   │   ├── AudioPlayer.tsx
│   │   │   │   ├── VideoPlayer.tsx
│   │   │   │   └── GlossaryItem.tsx
│   │   │   └── hooks/
│   │   │       ├── useLibrary.ts
│   │   │       └── useOfflineContent.ts
│   │   │
│   │   ├── family/                              # Feature: Acompañamiento
│   │   │   ├── screens/
│   │   │   │   ├── FamilyDashboardScreen.tsx
│   │   │   │   ├── InviteFamilyScreen.tsx
│   │   │   │   ├── SharedCalendarScreen.tsx
│   │   │   │   └── FamilyChatScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── FamilyMemberCard.tsx
│   │   │   │   ├── InviteForm.tsx
│   │   │   │   └── SupportNotification.tsx
│   │   │   └── hooks/
│   │   │       └── useFamily.ts
│   │   │
│   │   ├── directory/                           # Feature: Directorio
│   │   │   ├── screens/
│   │   │   │   ├── DirectoryMapScreen.tsx
│   │   │   │   ├── ProfessionalListScreen.tsx
│   │   │   │   ├── ProfessionalDetailScreen.tsx
│   │   │   │   ├── HealthCenterScreen.tsx
│   │   │   │   └── TeleconsultScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── ProfessionalCard.tsx
│   │   │   │   ├── MapView.tsx
│   │   │   │   ├── FilterBar.tsx
│   │   │   │   └── EmergencyContacts.tsx
│   │   │   └── hooks/
│   │   │       ├── useDirectory.ts
│   │   │       └── useLocation.ts
│   │   │
│   │   ├── community/                           # Feature: Comunidad
│   │   │   ├── screens/
│   │   │   │   ├── ForumListScreen.tsx
│   │   │   │   ├── ForumDetailScreen.tsx
│   │   │   │   ├── CreatePostScreen.tsx
│   │   │   │   └── StoriesScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── PostCard.tsx
│   │   │   │   ├── CommentThread.tsx
│   │   │   │   ├── Reactions.tsx
│   │   │   │   └── ReportButton.tsx
│   │   │   └── hooks/
│   │   │       ├── useForum.ts
│   │   │       └── useModeration.ts
│   │   │
│   │   ├── reminders/                           # Feature: Recordatorios
│   │   │   ├── screens/
│   │   │   │   ├── ReminderListScreen.tsx
│   │   │   │   └── CreateReminderScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── ReminderCard.tsx
│   │   │   │   └── ReminderForm.tsx
│   │   │   └── hooks/
│   │   │       └── useReminders.ts
│   │   │
│   │   ├── profile/                             # Feature: Perfil
│   │   │   ├── screens/
│   │   │   │   ├── ProfileScreen.tsx
│   │   │   │   ├── HealthProfileScreen.tsx
│   │   │   │   ├── SettingsScreen.tsx
│   │   │   │   ├── PrivacyScreen.tsx
│   │   │   │   ├── HealthReportScreen.tsx
│   │   │   │   └── AboutScreen.tsx
│   │   │   ├── components/
│   │   │   │   ├── ProfileHeader.tsx
│   │   │   │   ├── SettingItem.tsx
│   │   │   │   └── LanguageSelector.tsx
│   │   │   └── hooks/
│   │   │       └── useProfile.ts
│   │   │
│   │   └── health-report/                       # Feature: Reportes
│   │       ├── screens/
│   │       │   └── ReportViewerScreen.tsx
│   │       ├── components/
│   │       │   ├── MonthlySummary.tsx
│   │       │   ├── TrendChart.tsx
│   │       │   └── ExportButton.tsx
│   │       └── hooks/
│   │           └── useHealthReport.ts
│   │
│   └── shared/                                  # Columna vertebral
│       ├── api/                                 # Capa HTTP — solo fetch
│       │   ├── client.ts                        # Config fetch base (baseURL, headers)
│       │   ├── auth.ts
│       │   ├── cycle.ts
│       │   ├── pregnancy.ts
│       │   ├── menopause.ts
│       │   ├── library.ts
│       │   ├── family.ts
│       │   ├── directory.ts
│       │   ├── community.ts
│       │   ├── reminders.ts
│       │   └── profile.ts
│       │
│       ├── components/                          # Componentes compartidos
│       │   ├── Button.tsx
│       │   ├── Button.styles.ts
│       │   ├── Input.tsx
│       │   ├── Input.styles.ts
│       │   ├── Card.tsx
│       │   ├── Card.styles.ts
│       │   ├── LoadingState.tsx
│       │   ├── ErrorState.tsx
│       │   ├── EmptyState.tsx
│       │   ├── Avatar.tsx
│       │   ├── Badge.tsx
│       │   ├── BottomSheet.tsx
│       │   ├── Toast.tsx
│       │   └── Header.tsx
│       │
│       ├── hooks/                               # Hooks compartidos
│       │   ├── useDebounce.ts
│       │   ├── useNetworkStatus.ts
│       │   └── useAppState.ts
│       │
│       ├── lib/                                 # Configuración y utilidades
│       │   ├── constants.ts                     # API_URL, etc.
│       │   ├── mappers.ts                       # Mappers API → UI
│       │   └── i18n.ts                          # Configuración de idiomas
│       │
│       ├── store/                               # Estado global (Zustand)
│       │   ├── useAuthStore.ts                  # Auth + persist
│       │   ├── useProfileStore.ts               # Perfil + etapa actual
│       │   ├── useUiStore.ts                    # UI state (modals, toasts)
│       │   └── useSettingsStore.ts              # Preferencias, idioma
│       │
│       ├── types/                               # Tipos compartidos
│       │   ├── api.ts                           # Tipos de API
│       │   ├── health.ts                        # Tipos de salud
│       │   └── navigation.ts                    # Tipos de navegación
│       │
│       └── utils/                               # Funciones puras helper
│           ├── auth.ts                          # token helpers
│           ├── format.ts                        # formatDate, etc.
│           ├── validation.ts                    # validación de formularios
│           └── notifications.ts                 # scheduling de notificaciones locales
│
├── assets/                                      # Assets estáticos
│   ├── images/
│   ├── illustrations/
│   ├── fonts/
│   └── audio/                                   # Audios en lenguas originarias
│
├── .env.example
├── app.json
├── tsconfig.json
├── babel.config.js
├── metro.config.js
└── package.json
```

---

## Patrón: Container-Presentational

Los **hooks** son los containers (tienen la lógica, el estado, las funciones).
Los **componentes** son presentacionales (reciben props, renderizan UI).

```
┌──────────────────┐      props       ┌──────────────────┐
│   Hook (Container) │ ──────────────> │ Component (View) │
│   - useState       │                 │   - Renderiza    │
│   - useEffect      │  callbacks      │   - Estilos      │
│   - fetch          │ <────────────── │   - Eventos      │
│   - handlers       │                 │                  │
└──────────────────┘                   └──────────────────┘
```

**EJEMPLO — Pantalla de Dashboard del Ciclo:**

```typescript
// features/cycle/hooks/useCycleData.ts — CONTAINER
export function useCycleData(userId: string) {
  const [currentCycle, setCurrentCycle] = useState<Cycle | null>(null);
  const [predictions, setPredictions] = useState<CyclePrediction | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchCycleData = useCallback(async () => {
    try {
      setLoading(true);
      const data = await getCurrentCycle(userId);
      setCurrentCycle(data);
      setPredictions(predictNextCycle(data));
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error al cargar datos');
    } finally {
      setLoading(false);
    }
  }, [userId]);

  useEffect(() => { fetchCycleData(); }, [fetchCycleData]);

  return { currentCycle, predictions, loading, error, fetchCycleData };
}

// features/cycle/screens/CycleDashboardScreen.tsx — SCREEN
export default function CycleDashboardScreen() {
  const { currentCycle, predictions, loading, error, fetchCycleData } = useCycleData();

  if (loading) return <LoadingState />;
  if (error) return <ErrorState error={error} onRetry={fetchCycleData} />;

  return (
    <ScrollView>
      <CycleSummary cycle={currentCycle} predictions={predictions} />
      <CalendarView cycle={currentCycle} />
      <SymptomSelector onSave={logSymptom} />
      <CycleChart data={currentCycle?.history} />
    </ScrollView>
  );
}
```

---

## Patrón: Co-location de Estilos

Cada componente tiene su archivo `.styles.ts` (StyleSheet) al lado:

```
SymptomSelector.tsx             ← Componente
SymptomSelector.styles.ts       ← Estilos exclusivos
```

```typescript
// SymptomSelector.tsx
import { styles } from './SymptomSelector.styles';

export function SymptomSelector({ onSelect }: Props) {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>¿Cómo te sientes hoy?</Text>
      {/* symptoms grid */}
    </View>
  );
}
```

```typescript
// SymptomSelector.styles.ts
import { StyleSheet } from 'react-native';

export const styles = StyleSheet.create({
  container: {
    padding: 16,
    backgroundColor: '#FFFFFF',
    borderRadius: 16,
    marginVertical: 8,
  },
  title: {
    fontSize: 16,
    fontWeight: '600',
    color: '#2D1B2E',
    marginBottom: 12,
  },
});
```

---

## Patrón: API Layer (Fetch Nativo)

Sin axios, sin React Query. Funciones independientes en `shared/api/`:

```typescript
// shared/api/cycle.ts
import { API_URL } from "../lib/constants";

export async function getCurrentCycle(userId: string): Promise<CycleResponse> {
  const res = await fetch(`${API_URL}/cycle/current`, {
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' },
  });
  if (!res.ok) throw new Error('Error al cargar datos del ciclo');
  return res.json();
}

export async function logSymptom(data: SymptomInput): Promise<void> {
  const res = await fetch(`${API_URL}/cycle/symptoms`, {
    method: 'POST',
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error('Error al registrar síntoma');
}
```

---

## Patrón: Hook CRUD Completo

```typescript
// features/community/hooks/useForum.ts
export function useForum(stage: LifeStage) {
  const [posts, setPosts] = useState<ForumPost[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState(1);

  const fetchPosts = useCallback(async () => {
    try {
      setLoading(true);
      const data = await getForumPosts(stage, page);
      setPosts(prev => page === 1 ? data : [...prev, ...data]);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error');
    } finally {
      setLoading(false);
    }
  }, [stage, page]);

  useEffect(() => { fetchPosts(); }, [fetchPosts]);

  const handleCreatePost = async (post: CreatePostInput) => {
    await createForumPost(post);
    setPage(1);
    await fetchPosts();
  };

  return {
    posts, loading, error,
    page, setPage,
    fetchPosts, handleCreatePost,
  };
}
```

---

## Manejo de Estados UI

Toda pantalla que carga datos maneja 4 estados:

```typescript
export function SymptomTrackerScreen() {
  const { symptoms, loading, error, fetchSymptoms } = useMenopauseData();

  if (loading) return <LoadingState />;
  if (error) return <ErrorState error={error} onRetry={fetchSymptoms} />;
  if (symptoms.length === 0) return <EmptyState message="No hay síntomas registrados" />;

  return (
    <FlatList
      data={symptoms}
      renderItem={({ item }) => <SymptomLog symptom={item} />}
    />
  );
}
```

---

## Protección de Rutas

Los navigators protegen rutas verificando auth:

```typescript
// navigation/RootNavigator.tsx
export function RootNavigator() {
  const { isAuthenticated, isLoading, hasCompletedOnboarding } = useAuthStore();

  if (isLoading) return <SplashScreen />;

  return (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
      {!hasCompletedOnboarding ? (
        <Stack.Screen name="Onboarding" component={OnboardingNavigator} />
      ) : !isAuthenticated ? (
        <Stack.Screen name="Auth" component={AuthNavigator} />
      ) : (
        <Stack.Screen name="Main" component={MainTabNavigator} />
      )}
    </Stack.Navigator>
  );
}
```

---

## Navigation Structure

```
RootNavigator (NativeStack)
├── OnboardingNavigator (Stack)
│   ├── Welcome (slider 3 etapas)
│   ├── StageSelection
│   ├── LanguageSelection
│   └── HealthProfileInit
│
├── AuthNavigator (Stack)
│   ├── Login
│   ├── Register
│   └── ForgotPassword
│
└── MainTabNavigator (BottomTabs)
    ├── Tab: Inicio (HomeStack)
    │   ├── Dashboard (según etapa)
    │   └── QuickActions
    ├── Tab: Mi Etapa (StageStack)
    │   ├── Ciclo: Calendar, Symptoms, Charts
    │   ├── Embarazo: Weekly, Appointments, KickCounter
    │   └── Menopausia: SymptomTracker, Charts
    ├── Tab: Biblioteca (LibraryStack)
    │   ├── LibraryHome
    │   ├── ArticleDetail
    │   ├── MythsVsReality
    │   └── Glossary
    ├── Tab: Comunidad (CommunityStack)
    │   ├── ForumList
    │   ├── ForumDetail
    │   └── CreatePost
    └── Tab: Perfil (ProfileStack)
        ├── Profile
        ├── HealthProfile
        ├── Settings
        ├── Privacy
        └── HealthReport
```
