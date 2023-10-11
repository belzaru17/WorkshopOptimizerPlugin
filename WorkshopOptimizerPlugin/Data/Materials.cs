using Lumina.Excel.GeneratedSheets;

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
    public readonly string Name;
    public readonly MaterialSource Source;

    public Materials this[int count] { get { return new Materials(this, count); } } 

    // Gatherable
    public static readonly Material PalmLeaf = new("Palm Leaf", MaterialSource.Gatherable);
    public static readonly Material Apple = new("Apple", MaterialSource.Gatherable);
    public static readonly Material Branch = new("Branch", MaterialSource.Gatherable);
    public static readonly Material Stone = new("Stone", MaterialSource.Gatherable);
    public static readonly Material Clam = new("Clam", MaterialSource.Gatherable);
    public static readonly Material Laver = new("Laver", MaterialSource.Gatherable);
    public static readonly Material Coral = new("Coral", MaterialSource.Gatherable);
    public static readonly Material Islewort = new("Islewort", MaterialSource.Gatherable);
    public static readonly Material Sand = new("Sand", MaterialSource.Gatherable);
    public static readonly Material Log = new("Log", MaterialSource.Gatherable);
    public static readonly Material PalmLog = new("Palm Log", MaterialSource.Gatherable);
    public static readonly Material Vine = new("Vine", MaterialSource.Gatherable);
    public static readonly Material Sap = new("Sap", MaterialSource.Gatherable);
    public static readonly Material CopperOre = new("Copper Ore", MaterialSource.Gatherable);
    public static readonly Material Limestone = new("Limestone", MaterialSource.Gatherable);
    public static readonly Material RockSalt = new("Rock Salt", MaterialSource.Gatherable);
    public static readonly Material Sugarcane = new("Sugarcane", MaterialSource.Gatherable);
    public static readonly Material CottonBoll = new("Cotton Boll", MaterialSource.Gatherable);
    public static readonly Material Hemp = new("Hemp", MaterialSource.Gatherable);
    public static readonly Material Clay = new("Clay", MaterialSource.Gatherable);
    public static readonly Material Tinsand = new("Tinsand", MaterialSource.Gatherable);
    public static readonly Material IronOre = new("Iron Ore", MaterialSource.Gatherable);
    public static readonly Material Quartz = new("Quartz", MaterialSource.Gatherable);
    public static readonly Material Leucogranite = new("Leucogranite", MaterialSource.Gatherable);
    public static readonly Material Islefish = new("Islefish", MaterialSource.Gatherable);
    public static readonly Material Squid = new("Squid", MaterialSource.Gatherable);
    public static readonly Material Jellyfish = new("Jellyfish", MaterialSource.Gatherable);
    public static readonly Material MulticoloredIslebloooms = new("Multicolored Isleblooms", MaterialSource.Gatherable);
    public static readonly Material Resin = new("Resin", MaterialSource.Gatherable);
    public static readonly Material Coconut = new("Coconut", MaterialSource.Gatherable);
    public static readonly Material BeehiveChip = new("Beenhive Chip", MaterialSource.Gatherable);
    public static readonly Material WoodOpal = new("Wood Opal", MaterialSource.Gatherable);
    public static readonly Material Coal = new("Coal", MaterialSource.Gatherable);
    public static readonly Material Glimshroom = new("Glimshroom", MaterialSource.Gatherable);
    public static readonly Material Shale = new("Shale", MaterialSource.Gatherable);
    public static readonly Material Marble = new("Marble", MaterialSource.Gatherable);
    public static readonly Material MythrilOre = new("Mythril Ore", MaterialSource.Gatherable);
    public static readonly Material Spectrine = new("Spectrine", MaterialSource.Gatherable);
    public static readonly Material EffervescentWater = new("Effervescent Water", MaterialSource.Gatherable);
    public static readonly Material DuriumSand = new("Durium Sand", MaterialSource.Gatherable);
    public static readonly Material YellowCopperOre = new("Yellow Copper Ore", MaterialSource.Gatherable);
    public static readonly Material GoldOre = new("Gold Ore", MaterialSource.Gatherable);
    public static readonly Material HawksEyeSand = new("Hawk's Eye Sand", MaterialSource.Gatherable);
    public static readonly Material CrystalFormation = new("Crystal Formation", MaterialSource.Gatherable);

    // Granary
    public static readonly Material Alyssum = new("Alyssum", MaterialSource.Granary);
    public static readonly Material RawGarnet = new("Raw Garnet", MaterialSource.Granary);
    public static readonly Material SpruceLog = new("Spruce Log", MaterialSource.Granary);
    public static readonly Material Hammerhead = new("Hammerhead", MaterialSource.Granary);
    public static readonly Material SilverOre = new("Silver Ore", MaterialSource.Granary);
    public static readonly Material CaveShrimp = new("CaveShrimp", MaterialSource.Granary);

    // Crop
    public static readonly Material Popoto = new("Popoto", MaterialSource.Crop);
    public static readonly Material Cabbage = new("Cabbage", MaterialSource.Crop);
    public static readonly Material Pumpkin = new("Pumpkin", MaterialSource.Crop);
    public static readonly Material Parsnip = new("Parsnip", MaterialSource.Crop);
    // Rare Crop
    public static readonly Material Isleberry = new("Isleberry", MaterialSource.RareCrop);
    public static readonly Material Onion = new("Onion", MaterialSource.RareCrop);
    public static readonly Material Tomato = new("Tomato", MaterialSource.RareCrop);
    public static readonly Material Wheat = new("Wheat", MaterialSource.RareCrop);
    public static readonly Material Corn = new("Corn", MaterialSource.RareCrop);
    public static readonly Material Radish = new("Radish", MaterialSource.RareCrop);
    public static readonly Material Paprika = new("Paprika", MaterialSource.RareCrop);
    public static readonly Material Leek = new("Leek", MaterialSource.RareCrop);
    public static readonly Material RunnerBeans = new("Runner Beans", MaterialSource.RareCrop);
    public static readonly Material Beet = new("Beet", MaterialSource.RareCrop);
    public static readonly Material Eggplant = new("Eggplant", MaterialSource.RareCrop);
    public static readonly Material Zucchini = new("Zucchini", MaterialSource.RareCrop);
    public static readonly Material Watermelon = new("Watermelon", MaterialSource.RareCrop);
    public static readonly Material SweetPopoto = new("Sweet Popoto", MaterialSource.RareCrop);
    public static readonly Material Broccoli = new("Broccoli", MaterialSource.RareCrop);
    public static readonly Material BuffaloBeans = new("Buffalo Beans", MaterialSource.RareCrop);

    // Dropping
    public static readonly Material Fleece = new("Fleece", MaterialSource.Dropping);
    public static readonly Material Claw = new("Claw", MaterialSource.Dropping);
    public static readonly Material Fur = new("Fur", MaterialSource.Dropping);
    public static readonly Material Feather = new("Feathery", MaterialSource.Dropping);
    public static readonly Material Egg = new("Egg", MaterialSource.Dropping);
    // Rare Dropping
    public static readonly Material Carapace = new("Carapace", MaterialSource.RareDropping);
    public static readonly Material Fang = new("Fang", MaterialSource.RareDropping);
    public static readonly Material Horn = new("Horn", MaterialSource.RareDropping);
    public static readonly Material Milk = new("Milk", MaterialSource.RareDropping);

    private Material(string name, MaterialSource source)
    {
        Name = name;
        Source = source;
    }
}
