namespace WorkshopOptimizerPlugin.Data;

internal class GranaryItemData
{
    public readonly uint Id;
    public readonly string Component;
    public readonly string Mission;

    public static readonly GranaryItemData[] Items =
    [
        new(20 /* Growth Formula */, "Alyssum", "Meandering Meadows"),
        new(21 /* Garnet Rapier */, "Raw Garnet", "Fatal Falls"),
        new(22 /* Spruce Round Shield */, "Spruce Log", "Wild Woods"),
        new(23 /* Shark Oil */, "Hammerhead", "Bending Beaches"),
        new(24 /* Silver Ear Cuffs */, "Silver Ore", "Mossy Mountains"),
        new(64 /* Bouillabaisse */, "Cave Shrimp", "Crumbling Caverns"),
    ];

    private GranaryItemData(uint id, string component, string mission)
    {
        Id = id;
        Component = component;
        Mission = mission;
    }
}
