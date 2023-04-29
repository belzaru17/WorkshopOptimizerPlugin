using System;
using System.Collections.Generic;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Persistence.Internal;

[Serializable]
internal class PersistentSeasonData
{
    public DateTime SeasonStart { get; set; }

    public DateTime?[] DataCollectionTime { get; set; }

    public List<PersistentItemData> ItemData { get; set; } = new();

    public int[][][] ProducedItems { get; set; }

    public bool[] RestCycles { get; set; }


    public PersistentSeasonData() : this(SeasonUtils.SeasonStart()) { }

    public PersistentSeasonData(int season_offset)
        : this(SeasonUtils.SeasonStart().AddDays(season_offset * Constants.MaxCycles)) { }

    public void InitializeDefaults(Configuration configuration)
    {
        RestCycles[configuration.DefaultRestCycle1] = true;
        RestCycles[configuration.DefaultRestCycle2] = true;
    }


    private PersistentSeasonData(DateTime seasonStart)
    {
        SeasonStart = seasonStart;
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
        RestCycles = new bool[Constants.MaxCycles];
    }
}
