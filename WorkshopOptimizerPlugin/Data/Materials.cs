using Lumina.Excel.GeneratedSheets;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WorkshopOptimizerPlugin.Data;

internal enum MaterialSource
{
    Gatherable,
    Granary,
    Crop,
    RareCrop,
    Dropping,
    RareDropping,
}

internal class Materials
{
    public readonly Material Material;
    public readonly int Count;

    public Materials(Material material, int count)
    {
        this.Material = material;
        Count = count;
    }
}

internal class Material
{
    private static readonly IDictionary<int, Material> MaterialList = new Dictionary<int, Material>();
    private static readonly IDictionary<uint, Material> UniqueMaterialIds = new Dictionary<uint, Material>();

    public static Material? GetMaterialByIndex(int index)
    {
        return MaterialList.TryGetValue(index, out var value)? value : null;
    }

    public static int MaxMaterials => MaterialList.Count;

    public static Material? GetMaterialById(uint id)
    {
        return UniqueMaterialIds.TryGetValue(id, out var value)? value : null;
    }


    public readonly uint Id;
    public readonly string Name;
    public readonly MaterialSource Source;

    public Materials this[int count] { get { return new Materials(this, count); } } 


    // Gatherable
    public static readonly Material PalmLeaf = new(37551, "Palm Leaf", MaterialSource.Gatherable);
    public static readonly Material Branch = new(37553, "Branch", MaterialSource.Gatherable);
    public static readonly Material Stone = new(37554, "Stone", MaterialSource.Gatherable);
    public static readonly Material Clam = new(37555, "Clam", MaterialSource.Gatherable);
    public static readonly Material Laver = new(37556, "Laver", MaterialSource.Gatherable);
    public static readonly Material Coral = new(37557, "Coral", MaterialSource.Gatherable);
    public static readonly Material Islewort = new(37558, "Islewort", MaterialSource.Gatherable);
    public static readonly Material Sand = new(37559, "Sand", MaterialSource.Gatherable);
    public static readonly Material Vine = new(37562, "Vine", MaterialSource.Gatherable);
    public static readonly Material Sap = new(37563, "Sap", MaterialSource.Gatherable);
    public static readonly Material Apple = new(37552, "Apple", MaterialSource.Gatherable);
    public static readonly Material Log = new(37560, "Log", MaterialSource.Gatherable);
    public static readonly Material PalmLog = new(37561, "Palm Log", MaterialSource.Gatherable);
    public static readonly Material CopperOre = new(37564, "Copper Ore", MaterialSource.Gatherable);
    public static readonly Material Limestone = new(37565, "Limestone", MaterialSource.Gatherable);
    public static readonly Material RockSalt = new(37566, "Rock Salt", MaterialSource.Gatherable);
    public static readonly Material Clay = new(37570, "Clay", MaterialSource.Gatherable);
    public static readonly Material Tinsand = new(37571, "Tinsand", MaterialSource.Gatherable);
    public static readonly Material Sugarcane = new(37567, "Sugarcane", MaterialSource.Gatherable);
    public static readonly Material CottonBoll = new(37568, "Cotton Boll", MaterialSource.Gatherable);
    public static readonly Material Hemp = new(37569, "Hemp", MaterialSource.Gatherable);
    public static readonly Material Islefish = new(37575, "Islefish", MaterialSource.Gatherable);
    public static readonly Material Squid = new(37576, "Squid", MaterialSource.Gatherable);
    public static readonly Material Jellyfish = new(37577, "Jellyfish", MaterialSource.Gatherable);
    public static readonly Material IronOre = new(37572, "Iron Ore", MaterialSource.Gatherable);
    public static readonly Material Quartz = new(37573, "Quartz", MaterialSource.Gatherable);
    public static readonly Material Leucogranite = new(37574, "Leucogranite", MaterialSource.Gatherable);
    public static readonly Material MulticoloredIslebloooms = new(39228, "Multicolored Isleblooms", MaterialSource.Gatherable);
    public static readonly Material Resin = new(39224, "Resin", MaterialSource.Gatherable);
    public static readonly Material Coconut = new(39225, "Coconut", MaterialSource.Gatherable);
    public static readonly Material BeehiveChip = new(39226, "Beehive Chip", MaterialSource.Gatherable);
    public static readonly Material WoodOpal = new(39227, "Wood Opal", MaterialSource.Gatherable);
    public static readonly Material Coal = new(39887, "Coal", MaterialSource.Gatherable);
    public static readonly Material Glimshroom = new(39889, "Glimshroom", MaterialSource.Gatherable);
    public static readonly Material EffervescentWater = new(39892, "Effervescent Water", MaterialSource.Gatherable);
    public static readonly Material Shale = new(39888, "Shale", MaterialSource.Gatherable);
    public static readonly Material Marble = new(39890, "Marble", MaterialSource.Gatherable);
    public static readonly Material MythrilOre = new(39891, "Mythril Ore", MaterialSource.Gatherable);
    public static readonly Material Spectrine = new(39893, "Spectrine", MaterialSource.Gatherable);
    public static readonly Material DuriumSand = new(41630, "Durium Sand", MaterialSource.Gatherable);
    public static readonly Material YellowCopperOre = new(41631, "Yellow Copper Ore", MaterialSource.Gatherable);
    public static readonly Material GoldOre = new(41632, "Gold Ore", MaterialSource.Gatherable);
    public static readonly Material HawksEyeSand = new(41633, "Hawk's Eye Sand", MaterialSource.Gatherable);
    public static readonly Material CrystalFormation = new(41634, "Crystal Formation", MaterialSource.Gatherable);

    // Granary
    public static readonly Material Alyssum = new(37578, "Alyssum", MaterialSource.Granary);
    public static readonly Material RawGarnet = new(37579, "Raw Garnet", MaterialSource.Granary);
    public static readonly Material SpruceLog = new(37580, "Spruce Log", MaterialSource.Granary);
    public static readonly Material Hammerhead = new(37581, "Hammerhead", MaterialSource.Granary);
    public static readonly Material SilverOre = new(37582, "Silver Ore", MaterialSource.Granary);
    public static readonly Material CaveShrimp = new(39894, "Cave Shrimp", MaterialSource.Granary);

    // Crops
    public static readonly Material Popoto = new(37593, "Popoto", MaterialSource.Crop);
    public static readonly Material Cabbage = new(37594, "Cabbage", MaterialSource.Crop);
    public static readonly Material Isleberry = new(37595, "Isleberry", MaterialSource.RareCrop);
    public static readonly Material Pumpkin = new(37596, "Pumpkin", MaterialSource.Crop);
    public static readonly Material Onion = new(37597, "Onion", MaterialSource.RareCrop);
    public static readonly Material Tomato = new(37598, "Tomato", MaterialSource.RareCrop);
    public static readonly Material Wheat = new(37599, "Wheat", MaterialSource.RareCrop);
    public static readonly Material Corn = new(37600, "Corn", MaterialSource.RareCrop);
    public static readonly Material Parsnip = new(37601, "Parsnip", MaterialSource.Crop);
    public static readonly Material Radish = new(37602, "Radish", MaterialSource.RareCrop);


    // Rare Crop
    public static readonly Material Paprika = new(39231, "Paprika", MaterialSource.RareCrop);
    public static readonly Material Leek = new(39232, "Leek", MaterialSource.RareCrop);
    public static readonly Material RunnerBeans = new(39899, "Runner Beans", MaterialSource.RareCrop);
    public static readonly Material Beet = new(39900, "Beet", MaterialSource.RareCrop);
    public static readonly Material Eggplant = new(39901, "Eggplant", MaterialSource.RareCrop);
    public static readonly Material Zucchini = new(39902, "Zucchini", MaterialSource.RareCrop);
    public static readonly Material Watermelon = new(41635, "Watermelon", MaterialSource.RareCrop);
    public static readonly Material SweetPopoto = new(41636, "Sweet Popoto", MaterialSource.RareCrop);
    public static readonly Material Broccoli = new(41637, "Broccoli", MaterialSource.RareCrop);
    public static readonly Material BuffaloBeans = new(41638, "Buffalo Beans", MaterialSource.RareCrop);

    // Dropping
    public static readonly Material Fleece = new(37603, "Fleece", MaterialSource.Dropping);
    public static readonly Material Claw = new(37604, "Claw", MaterialSource.Dropping);
    public static readonly Material Fur = new(37605, "Fur", MaterialSource.Dropping);
    public static readonly Material Feather = new(37606, "Feather", MaterialSource.Dropping);
    public static readonly Material Egg = new(37607, "Egg", MaterialSource.Dropping);

    // Rare Dropping
    public static readonly Material Carapace = new(37608, "Carapace", MaterialSource.RareDropping);
    public static readonly Material Fang = new(37609, "Fang", MaterialSource.RareDropping);
    public static readonly Material Horn = new(37610, "Horn", MaterialSource.RareDropping);
    public static readonly Material Milk = new(37611, "Milk", MaterialSource.RareDropping);

    private Material(uint id, string name, MaterialSource source)
    {
        Id = id;
        Name = name;
        Source = source;

        MaterialList[MaterialList.Count] = this;
        Debug.Assert(UniqueMaterialIds.TryAdd(id, this), $"Duplicate id {id}");
    }
}
