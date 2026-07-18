namespace Luna.Application.Common.Helpers;

public static class PregnancyHelper
{
    public static int CalculateCurrentWeek(DateOnly estimatedDueDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var daysUntilDue = estimatedDueDate.DayNumber - today.DayNumber;
        var weeks = (280 - daysUntilDue) / 7; // 280 días = 40 semanas
        return Math.Clamp(weeks, 0, 42);
    }
}
