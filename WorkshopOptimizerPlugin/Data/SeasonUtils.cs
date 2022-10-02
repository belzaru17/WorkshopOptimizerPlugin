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

}
