namespace WorkshopOptimizerPlugin.Data;

internal class GranaryItemData
{
    public readonly uint Id;
    public readonly string Component;
    public readonly string Mission;

    public static readonly GranaryItemData[] Items = new GranaryItemData[]
    {
        new GranaryItemData(20 /* Growth Formula */, "Alyssum", "Meandering Meadows"),
        new GranaryItemData(21 /* Garnet Rapier */, "Raw Garnet", "Fatal Falls"),
        new GranaryItemData(22 /* Spruce Round Shield */, "Spruce Log", "Wild Woods"),
        new GranaryItemData(23 /* Shark Oil */, "Hammerhead", "Bending Beaches"),
        new GranaryItemData(24 /* Silver Ear Cuffs */, "Silver Ore", "Mossy Mountains"),
        new GranaryItemData(64 /* Bouillabaisse */, "Cave Shrimp", "Crumbling Caverns"),
    };

    private GranaryItemData(uint id, string component, string mission)
    {
        Id = id;
        Component = component;
        Mission = mission;
    }
}
