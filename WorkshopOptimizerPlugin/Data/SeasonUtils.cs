using System;

namespace WorkshopOptimizerPlugin.Data;

internal static class SeasonUtils
{
    public static int GetCycle()
    {
        var now = DateTime.UtcNow;

        var cycle_shift = 0;
        if (now.Hour < Constants.ResetUTCHour) { cycle_shift = -1; }

        return now.DayOfWeek switch
        {
            DayOfWeek.Tuesday => (7 + cycle_shift) % 7,
            DayOfWeek.Wednesday => 1 + cycle_shift,
            DayOfWeek.Thursday => 2 + cycle_shift,
            DayOfWeek.Friday => 3 + cycle_shift,
            DayOfWeek.Saturday => 4 + cycle_shift,
            DayOfWeek.Sunday => 5 + cycle_shift,
            DayOfWeek.Monday => 6 + cycle_shift,
            _ => -1,
        };
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

        return cycleStart.DayOfWeek switch
        {
            DayOfWeek.Wednesday => cycleStart.AddDays(-1),
            DayOfWeek.Thursday => cycleStart.AddDays(-2),
            DayOfWeek.Friday => cycleStart.AddDays(-3),
            DayOfWeek.Saturday => cycleStart.AddDays(-4),
            DayOfWeek.Sunday => cycleStart.AddDays(-5),
            DayOfWeek.Monday => cycleStart.AddDays(-6),
            _ => cycleStart,
        };
    }
}
