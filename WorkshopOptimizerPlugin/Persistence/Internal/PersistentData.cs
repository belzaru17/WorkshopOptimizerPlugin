using System;
using System.Collections.Generic;
using System.Data;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Persistence.Internal;


[Serializable]
internal class PersistentData
{

    public uint Version { get; set; } = 4;

    public DateTime SeasonStart { get; set; }

    public DateTime?[] DataCollectionTime { get; set; }

    public List<PersistentItemData> ItemData { get; set; } = new();

    public int[][][] ProducedItems { get; set; }

    public PersistentData()
    {
        DataCollectionTime = new DateTime?[Constants.MaxCycles];
        ProducedItems = new int[Constants.MaxCycles][][];
        for (var c = 0; c < ProducedItems.Length; c++)
        {
            ProducedItems[c] = new int[Constants.MaxWorkshops][];
            for (var w = 0; w < ProducedItems[c].Length; w++)
            {
                ProducedItems[c][w] = new int[Constants.MaxSteps];
                for (var s = 0; s < ProducedItems[c][w].Length; s++)
                {
                    ProducedItems[c][w][s] = -1;
                }
            }
        }

        SeasonStart = getStartOfCycle();
    }

    private static DateTime getStartOfCycle()
    {
        DateTime cycleStart = DateTime.UtcNow;

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
