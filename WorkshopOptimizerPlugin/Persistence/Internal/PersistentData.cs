using System;

namespace WorkshopOptimizerPlugin.Persistence.Internal;

[Serializable]
internal class PersistentData
{
    public uint Version { get; set; } = 5;

    public PersistentSeasonData CurrentSeason { get; set; } = new();
    public PersistentSeasonData PreviousSeason { get; set; } = new(-1);

    public PersistentData() { }
}
