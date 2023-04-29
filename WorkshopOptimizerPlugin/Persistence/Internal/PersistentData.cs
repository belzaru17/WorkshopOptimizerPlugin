using System;

namespace WorkshopOptimizerPlugin.Persistence.Internal;

[Serializable]
internal class PersistentData
{
    public uint Version { get; set; } = 6;

    public PersistentSeasonData CurrentSeason { get; set; } = new();
    public PersistentSeasonData PreviousSeason { get; set; } = new(-1);

    public PersistentData() { }

    public PersistentData(Configuration configuration) {
        CurrentSeason.InitializeDefaults(configuration);
        PreviousSeason.InitializeDefaults(configuration);
    }
}
