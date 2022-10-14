using System;

namespace WorkshopOptimizerPlugin.Data;

internal static class SeasonUtils
{
    public static int GetCycle()
    {
        DateTime now = DateTime.UtcNow;

        var cycle_shift = 0;
        if (now.Hour < Constants.ResetUTCHour) { cycle_shift = -1; }

        switch (now.DayOfWeek)
        {
            case DayOfWeek.Tuesday:
                return (7 + cycle_shift) % 7;
            case DayOfWeek.Wednesday:
                return 1 + cycle_shift;
            case DayOfWeek.Thursday:
                return 2 + cycle_shift;
            case DayOfWeek.Friday:
                return 3 + cycle_shift;
            case DayOfWeek.Saturday:
                return 4 + cycle_shift;
            case DayOfWeek.Sunday:
                return 5 + cycle_shift;
            case DayOfWeek.Monday:
                return 6 + cycle_shift;
        }
        return -1;
    }

    public static bool IsSameSeason(DateTime seasonStart)
    {
        return DateTime.UtcNow < seasonStart.AddDays(Constants.MaxCycles);
    }

    public static bool IsPreviousSeason(DateTime previousSeasonStart)
    {
        return SeasonStart().Subtract(previousSeasonStart).Days <= Constants.MaxCycles;
    }

    public static DateTime SeasonStart()
    {
        var cycleStart = DateTime.UtcNow;

        if (cycleStart.Hour >= Constants.ResetUTCHour)
        {
            cycleStart = new DateTime(cycleStart.Year, cycleStart.Month, cycleStart.Day).AddHours(Constants.ResetUTCHour);
        }
        else
        {
            cycleStart = new DateTime(cycleStart.Year, cycleStart.Month, cycleStart.Day - 1).AddHours(Constants.ResetUTCHour);
        }

        switch (cycleStart.DayOfWeek)
        {
            case DayOfWeek.Wednesday:
                return cycleStart.AddDays(-1);
            case DayOfWeek.Thursday:
                return cycleStart.AddDays(-2);
            case DayOfWeek.Friday:
                return cycleStart.AddDays(-3);
            case DayOfWeek.Saturday:
                return cycleStart.AddDays(-4);
            case DayOfWeek.Sunday:
                return cycleStart.AddDays(-5);
            case DayOfWeek.Monday:
                return cycleStart.AddDays(-6);
        }
        return cycleStart;
    }
}
