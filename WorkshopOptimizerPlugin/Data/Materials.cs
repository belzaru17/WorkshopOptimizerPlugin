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
    public static readonly Material PalmLeaf = new Material("Palm Leaf", MaterialSource.Gatherable);
    public static readonly Material Apple = new Material("Apple", MaterialSource.Gatherable);
    public static readonly Material Branch = new Material("Branch", MaterialSource.Gatherable);
    public static readonly Material Stone = new Material("Stone", MaterialSource.Gatherable);
    public static readonly Material Clam = new Material("Clam", MaterialSource.Gatherable);
    public static readonly Material Laver = new Material("Laver", MaterialSource.Gatherable);
    public static readonly Material Coral = new Material("Coral", MaterialSource.Gatherable);
    public static readonly Material Islewort = new Material("Islewort", MaterialSource.Gatherable);
    public static readonly Material Sand = new Material("Sand", MaterialSource.Gatherable);
    public static readonly Material Log = new Material("Log", MaterialSource.Gatherable);
    public static readonly Material PalmLog = new Material("Palm Log", MaterialSource.Gatherable);
    public static readonly Material Vine = new Material("Vine", MaterialSource.Gatherable);
    public static readonly Material Sap = new Material("Sap", MaterialSource.Gatherable);
    public static readonly Material CopperOre = new Material("Copper Ore", MaterialSource.Gatherable);
    public static readonly Material Limestone = new Material("Limestone", MaterialSource.Gatherable);
    public static readonly Material RockSalt = new Material("Rock Salt", MaterialSource.Gatherable);
    public static readonly Material Sugarcane = new Material("Sugarcane", MaterialSource.Gatherable);
    public static readonly Material CottonBoll = new Material("Cotton Boll", MaterialSource.Gatherable);
    public static readonly Material Hemp = new Material("Hemp", MaterialSource.Gatherable);
    public static readonly Material Clay = new Material("Clay", MaterialSource.Gatherable);
    public static readonly Material Tinsand = new Material("Tinsand", MaterialSource.Gatherable);
    public static readonly Material IronOre = new Material("Iron Ore", MaterialSource.Gatherable);
    public static readonly Material Quartz = new Material("Quartz", MaterialSource.Gatherable);
    public static readonly Material Leucogranite = new Material("Leucogranite", MaterialSource.Gatherable);
    public static readonly Material Islefish = new Material("Islefish", MaterialSource.Gatherable);
    public static readonly Material Squid = new Material("Squid", MaterialSource.Gatherable);
    public static readonly Material Jellyfish = new Material("Jellyfish", MaterialSource.Gatherable);
    public static readonly Material MulticoloredIslebloooms = new Material("Multicolored Isleblooms", MaterialSource.Gatherable);
    public static readonly Material Resin = new Material("Resin", MaterialSource.Gatherable);
    public static readonly Material Coconut = new Material("Coconut", MaterialSource.Gatherable);
    public static readonly Material BeehiveChip = new Material("Beenhive Chip", MaterialSource.Gatherable);
    public static readonly Material WoodOpal = new Material("Wood Opal", MaterialSource.Gatherable);

    // Granary
    public static readonly Material Alyssum = new Material("Alyssum", MaterialSource.Granary);
    public static readonly Material RawGarnet = new Material("Raw Garnet", MaterialSource.Granary);
    public static readonly Material SpruceLog = new Material("Spruce Log", MaterialSource.Granary);
    public static readonly Material Hammerhead = new Material("Hammerhead", MaterialSource.Granary);
    public static readonly Material SilverOre = new Material("Silver Ore", MaterialSource.Granary);

    // Crop
    public static readonly Material Popoto = new Material("Popoto", MaterialSource.Crop);
    public static readonly Material Cabbage = new Material("Cabbage", MaterialSource.Crop);
    public static readonly Material Pumpkin = new Material("Pumpkin", MaterialSource.Crop);
    public static readonly Material Parsnip = new Material("Parsnip", MaterialSource.Crop);
    // Rare Crop
    public static readonly Material Isleberry = new Material("Isleberry", MaterialSource.RareCrop);
    public static readonly Material Onion = new Material("Onion", MaterialSource.RareCrop);
    public static readonly Material Tomato = new Material("Tomato", MaterialSource.RareCrop);
    public static readonly Material Wheat = new Material("Wheat", MaterialSource.RareCrop);
    public static readonly Material Corn = new Material("Corn", MaterialSource.RareCrop);
    public static readonly Material Radish = new Material("Radish", MaterialSource.RareCrop);

    // Dropping
    public static readonly Material Fleece = new Material("Fleece", MaterialSource.Dropping);
    public static readonly Material Claw = new Material("Claw", MaterialSource.Dropping);
    public static readonly Material Fur = new Material("Fur", MaterialSource.Dropping);
    public static readonly Material Feather = new Material("Feathery", MaterialSource.Dropping);
    public static readonly Material Egg = new Material("Egg", MaterialSource.Dropping);
    // Rare Dropping
    public static readonly Material Carapace = new Material("Carapace", MaterialSource.RareDropping);
    public static readonly Material Fang = new Material("Fang", MaterialSource.RareDropping);
    public static readonly Material Horn = new Material("Horn", MaterialSource.RareDropping);
    public static readonly Material Milk = new Material("Milk", MaterialSource.RareDropping);

    private Material(string name, MaterialSource source)
    {
        Name = name;
        Source = source;
    }
}
