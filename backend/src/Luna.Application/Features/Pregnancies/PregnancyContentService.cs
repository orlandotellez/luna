using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Models.Pregnancy;

namespace Luna.Application.Features.Pregnancies;

public class PregnancyContentService : IPregnancyContentService
{
    private readonly IReadOnlyDictionary<int, PregnancyWeekContentDto> _weeks;

    public PregnancyContentService()
    {
        _weeks = SeedWeeks().ToDictionary(w => w.WeekNumber);
    }

    public Task<PregnancyWeekContentDto?> GetWeekAsync(int weekNumber)
    {
        if (weekNumber < 1 || weekNumber > 42)
            return Task.FromResult<PregnancyWeekContentDto?>(null);

        _weeks.TryGetValue(weekNumber, out var content);
        return Task.FromResult(content);
    }

    public Task<List<PregnancyWeekContentDto>> GetAllWeeksAsync()
    {
        return Task.FromResult(_weeks.Values.OrderBy(w => w.WeekNumber).ToList());
    }

    public Task<PregnancyWeeksResponseDto> GetWeeksProgressAsync(int? currentWeek)
    {
        var progressList = _weeks.Values
            .OrderBy(w => w.WeekNumber)
            .Select(w => new PregnancyWeekProgressDto
            {
                WeekNumber = w.WeekNumber,
                Trimester = w.Trimester,
                Title = w.Title,
                IsCompleted = currentWeek.HasValue && w.WeekNumber < currentWeek.Value,
                IsCurrent = currentWeek.HasValue && w.WeekNumber == currentWeek.Value
            })
            .ToList();

        return Task.FromResult(new PregnancyWeeksResponseDto { Weeks = progressList });
    }

    private static IEnumerable<PregnancyWeekContentDto> SeedWeeks()
    {
        return new List<PregnancyWeekContentDto>
        {
            new() { WeekNumber = 1,  Title = "Semana 1 — Inicio del embarazo",
                BabySizeCm = 0.0m, BabyWeightG = 0,
                Highlights = new() { "Fecundación del óvulo", "Inicio de la división celular" },
                TipsForMom = new() { "Tomar ácido fólico", "Evitar alcohol y tabaco" },
                Alerts = new() },

            new() { WeekNumber = 13, Title = "Semana 13 — Cierre del primer trimestre",
                BabySizeCm = 7.4m, BabyWeightG = 23,
                Highlights = new() { "Huellas dactilares formadas", "Voz y cuerdas vocales" },
                TipsForMom = new() { "Anunciar el embarazo si lo deseas", "Empezar ropa de maternidad cómoda" },
                Alerts = new() },

            new() { WeekNumber = 20, Title = "Semana 20 — Mitad del camino",
                BabySizeCm = 16.4m, BabyWeightG = 300,
                Highlights = new() { "Movimientos activos", "Desarrollo del sueño REM" },
                TipsForMom = new() { "Ecografía morfológica", "Dormir del lado izquierdo" },
                Alerts = new() },

            new() { WeekNumber = 26, Title = "Semana 26 — El bebé responde a sonidos",
                BabySizeCm = 35.6m, BabyWeightG = 760,
                Highlights = new() { "Desarrollo auditivo", "Reflejos de sobresalto" },
                TipsForMom = new() { "Hablarle al bebé", "Dormir sobre el lado izquierdo" },
                Alerts = new() },

            new() { WeekNumber = 27, Title = "Semana 27 — Final del segundo trimestre",
                BabySizeCm = 36.6m, BabyWeightG = 875,
                Highlights = new() { "Apertura de párpados", "Reconoce tu voz" },
                TipsForMom = new() { "Iniciar clases de preparación para el parto", "Monitorear movimientos fetales" },
                Alerts = new() },

            new() { WeekNumber = 28, Title = "Semana 28 — Inicio del tercer trimestre",
                BabySizeCm = 37.6m, BabyWeightG = 1005,
                Highlights = new() { "Sueño más activo", "Aumenta la grasa subcutánea" },
                TipsForMom = new() { "Visitas prenatales cada 2 semanas", "Llevar registro de patadas" },
                Alerts = new() { "Signos de preeclampsia: dolor de cabeza intenso, hinchazón súbita" } },

            new() { WeekNumber = 36, Title = "Semana 36 — Casi en la meta",
                BabySizeCm = 47.4m, BabyWeightG = 2622,
                Highlights = new() { "Posición cefálica probable", "Pulmones casi maduros" },
                TipsForMom = new() { "Preparar maleta para el hospital", "Confirmar plan de parto" },
                Alerts = new() { "Contracciones regulares antes de tiempo requieren evaluación" } },

            new() { WeekNumber = 40, Title = "Semana 40 — Fecha probable de parto",
                BabySizeCm = 51.2m, BabyWeightG = 3402,
                Highlights = new() { "Totalmente desarrollado", "Listo para nacer" },
                TipsForMom = new() { "Mantener calma", "Monitorear contracciones" },
                Alerts = new() { "Si no hay movimiento fetal, ir a urgencias inmediatamente" } },

            new() { WeekNumber = 42, Title = "Semana 42 — Post-término",
                BabySizeCm = 53.0m, BabyWeightG = 3700,
                Highlights = new() { "Placenta envejeciendo", "Mayor vigilancia médica" },
                TipsForMom = new() { "Asistir a control diario", "Considerar inducción según indicación médica" },
                Alerts = new() { "Requiere vigilancia estrecha por riesgo de insuficiencia placentaria" } }
        };
    }
}
