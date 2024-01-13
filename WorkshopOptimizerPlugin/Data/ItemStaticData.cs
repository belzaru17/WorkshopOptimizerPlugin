using System.Collections.Immutable;
using System.Diagnostics;

namespace WorkshopOptimizerPlugin.Data;

internal class ItemStaticData
{
    public readonly uint Id;
    public readonly string Name;
    public readonly int MinLevel;
    public readonly int Hours;
    public readonly int Value;
    public readonly When When;
    public readonly Categories[] Categories;
    public readonly Materials[] Materials;

    public bool IsValid()
    {
        return Value != 0;
    }

    public static ItemStaticData Get(uint item_index)
    {
        return ItemsData[item_index];
    }

    public static ItemStaticData Get(int item_index)
    {
        return ItemsData[item_index];
    }

    public static ImmutableArray<ItemStaticData> GetAllItems()
    {
        return ItemsData.ToImmutableArray();
    }

    private ItemStaticData(uint id, string name, int minLevel, int hours, int base_value, When when, Categories[] categories, Materials[] materials)
    {
        Id = id;
        Name = name;
        MinLevel = minLevel;
        Hours = hours;
        Value = (int)(base_value * Constants.WorkshopLevelMultiplier);
        When = when;
        Categories = categories;
        Materials = materials;
    }

    static ItemStaticData()
    {
        Debug.Assert(ItemsData.Length == Constants.MaxItems);
    }

    private static readonly ItemStaticData[] ItemsData = [
        new( 0, "UNKNOWN",                0, 0,   0, When.Never, [], []),
        new( 1, "Potion",                 1, 4,  28, When.Always, [Data.Categories.Concoctions],                                          [Material.PalmLeaf[2], Material.Islewort[2]]),
        new( 2, "Firesand",               1, 4,  28, When.Always, [Data.Categories.Concoctions, Data.Categories.UnburiedTreasures],       [Material.Sand[2], Material.Limestone[1], Material.Islewort[1]]),
        new( 3, "Wooden Chair",           1, 6,  42, When.Always, [Data.Categories.Furnishings, Data.Categories.Woodworks],               [Material.Log[4], Material.Vine[2]]),
        new( 4, "Grilled Clam",           1, 4,  28, When.Always, [Data.Categories.Foodstuffs, Data.Categories.MarineMerchandise],        [Material.Clam[2], Material.Laver[2]]),
        new( 5, "Necklace",               1, 4,  28, When.Always, [Data.Categories.Accessories, Data.Categories.Woodworks],               [Material.Branch[3], Material.Vine[1]]),
        new( 6, "Coral Ring",             1, 6,  42, When.Always, [Data.Categories.Accessories, Data.Categories.MarineMerchandise],       [Material.Coral[3], Material.Vine[3]]),
        new( 7, "Barbut",                 1, 6,  42, When.Always, [Data.Categories.Attire, Data.Categories.Metalworks],                   [Material.CopperOre[3], Material.Sand[3]]),
        new( 8, "Macuahuitl",             1, 6,  42, When.Always, [Data.Categories.Arms, Data.Categories.Woodworks],                      [Material.PalmLog[3], Material.Stone[3]]),
        new( 9, "Sauerkraut",             1, 4,  40, When.Always, [Data.Categories.PreservedFood],                                        [Material.Cabbage[1], Material.RockSalt[3]]),
        new(10, "Baked Pumpkin",          1, 4,  40, When.Always, [Data.Categories.Foodstuffs],                                           [Material.Pumpkin[1], Material.Sap[3]]),
        new(11, "Tunic",                  1, 6,  72, When.Either, [Data.Categories.Attire, Data.Categories.Textiles],                     [Material.Fleece[2], Material.Vine[4]]),
        new(12, "Culinary Knife",         1, 4,  44, When.Always, [Data.Categories.Sundries, Data.Categories.CreatureCreations],          [Material.Claw[1], Material.PalmLog[3]]),
        new(13, "Brush",                  1, 4,  44, When.Always, [Data.Categories.Sundries, Data.Categories.Woodworks],                  [Material.Fur[1], Material.PalmLog[3]]),
        new(14, "Boiled Egg",             1, 4,  44, When.Always, [Data.Categories.Foodstuffs, Data.Categories.CreatureCreations],        [Material.Egg[1], Material.Laver[3]]),
        new(15, "Hora",                   1, 6,  72, When.Either, [Data.Categories.Arms, Data.Categories.CreatureCreations],              [Material.Carapace[2], Material.Stone[4]]),
        new(16, "Earrings",               1, 4,  44, When.Either, [Data.Categories.Accessories, Data.Categories.CreatureCreations],       [Material.Fang[1], Material.Vine[3]]),
        new(17, "Butter",                 1, 4,  44, When.Either, [Data.Categories.Ingredients, Data.Categories.CreatureCreations],       [Material.Milk[1], Material.RockSalt[3]]),
        new(18, "Brick Counter",          5, 6,  48, When.Always, [Data.Categories.Furnishings, Data.Categories.UnburiedTreasures],       [Material.Clay[2], Material.Limestone[2], Material.PalmLog[2]]),
        new(19, "Bronze Sheep",           5, 8,  64, When.Either, [Data.Categories.Furnishings, Data.Categories.Metalworks],              [Material.Tinsand[3], Material.CopperOre[3], Material.Log[2]]),
        new(20, "Growth Formula",         5, 8, 136, When.Either, [Data.Categories.Concoctions],                                          [Material.Alyssum[2], Material.Islewort[3], Material.Branch[3]]),
        new(21, "Garnet Rapier",          5, 8, 136, When.Either, [Data.Categories.Arms, Data.Categories.UnburiedTreasures],              [Material.RawGarnet[2], Material.CopperOre[3], Material.Tinsand[3]]),
        new(22, "Spruce Round Shield",    5, 8, 136, When.Either, [Data.Categories.Attire, Data.Categories.Woodworks],                    [Material.SpruceLog[2], Material.CopperOre[3], Material.Stone[3]]),
        new(23, "Shark Oil",              5, 8, 136, When.Either, [Data.Categories.Sundries, Data.Categories.MarineMerchandise],          [Material.Hammerhead[2], Material.Laver[3], Material.Sap[3]]),
        new(24, "Silver Ear Cuffs",       5, 8, 136, When.Either, [Data.Categories.Accessories, Data.Categories.Metalworks],              [Material.SilverOre[2], Material.Tinsand[3], Material.Coral[3]]),
        new(25, "Sweet Popoto",           5, 6,  72, When.Either, [Data.Categories.Confections],                                          [Material.Popoto[1], Material.Milk[1], Material.Sap[3]]),
        new(26, "Parsnip Salad",          5, 4,  48, When.Always, [Data.Categories.Foodstuffs],                                           [Material.Parsnip[2], Material.Islewort[1], Material.Sap[1]]),
        new(27, "Caramels",               6, 6,  81, When.Either, [Data.Categories.Confections],                                          [Material.Sugarcane[4], Material.Milk[2]]),
        new(28, "Ribbon",                 6, 6,  54, When.Either, [Data.Categories.Accessories, Data.Categories.Textiles],                [Material.CottonBoll[2], Material.CopperOre[2], Material.Vine[2]]),
        new(29, "Rope",                   6, 4,  36, When.Either, [Data.Categories.Sundries, Data.Categories.Textiles],                   [Material.Hemp[2], Material.Islewort[1], Material.Vine[1]]),
        new(30, "Cavalier's Hat",         6, 6,  81, When.Either, [Data.Categories.Attire, Data.Categories.Textiles],                     [Material.Feather[2], Material.CottonBoll[2], Material.Hemp[2]]),
        new(31, "Horn",                   6, 6,  81, When.Either, [Data.Categories.Sundries, Data.Categories.CreatureCreations],          [Material.Horn[2], Material.Clay[2], Material.Hemp[2]]),
        new(32, "Salt Cod",               7, 6,  54, When.Either, [Data.Categories.PreservedFood, Data.Categories.MarineMerchandise],     [Material.Islefish[4], Material.RockSalt[2]]),
        new(33, "Squid Ink",              7, 4,  36, When.Always, [Data.Categories.Ingredients, Data.Categories.MarineMerchandise],       [Material.Squid[2], Material.RockSalt[2] ]),
        new(34, "Essential Draught",      7, 6,  54, When.Either, [Data.Categories.Concoctions, Data.Categories.MarineMerchandise],       [Material.Jellyfish[2], Material.PalmLeaf[2], Material.Laver[2]]),
        new(35, "Isleberry Jam",          7, 6,  78, When.Either, [Data.Categories.Ingredients],                                          [Material.Isleberry[3], Material.Sugarcane[2], Material.Sap[1]]),
        new(36, "Tomato Relish",          7, 4,  52, When.Either, [Data.Categories.Ingredients],                                          [Material.Tomato[2], Material.Islewort[1], Material.Sap[1]]),
        new(37, "Onion Soup",             7, 6,  78, When.Either, [Data.Categories.Foodstuffs],                                           [Material.Onion[3], Material.RockSalt[2], Material.Islewort[1]]),
        new(38, "Islefish Pie",           7, 6,  78, When.Either, [Data.Categories.Confections, Data.Categories.MarineMerchandise],       [Material.Wheat[3], Material.Islefish[2], Material.Sugarcane[1]]),
        new(39, "Corn Flakes",            7, 4,  42, When.Either, [Data.Categories.PreservedFood],                                        [Material.Corn[2], Material.Sugarcane[2]]),
        new(40, "Pickled Radish",         7, 8, 104, When.Either, [Data.Categories.PreservedFood],                                        [Material.Radish[4], Material.Apple[2], Material.Sugarcane[2]]),
        new(41, "Iron Axe",               8, 8,  72, When.Either, [Data.Categories.Arms, Data.Categories.Metalworks],                     [Material.IronOre[3], Material.Log[3], Material.Sand[2]]),
        new(42, "Quartz Ring",            8, 8,  72, When.Either, [Data.Categories.Accessories, Data.Categories.UnburiedTreasures],       [Material.Quartz[3], Material.IronOre[3], Material.Stone[2]]),
        new(43, "Porcelain Vase",         8, 8,  72, When.Either, [Data.Categories.Sundries, Data.Categories.UnburiedTreasures],          [Material.Leucogranite[3], Material.Quartz[3], Material.Clay[2]]),
        new(44, "Vegetable Juice",        8, 6,  78, When.Either, [Data.Categories.Concoctions],                                          [Material.Cabbage[3], Material.Islewort[2], Material.Laver[1]]),
        new(45, "Pumpkin Pudding",        8, 6 , 78, When.Either, [Data.Categories.Confections],                                          [Material.Pumpkin[3], Material.Egg[1], Material.Milk[1]]),
        new(46, "Sheepfluff Rug",         8, 6,  90, When.Either, [Data.Categories.Furnishings, Data.Categories.CreatureCreations],       [Material.Fleece[3], Material.CottonBoll[2], Material.Hemp[1]]),
        new(47, "Garden Scythe",          9, 6,  90, When.Either, [Data.Categories.Sundries, Data.Categories.Metalworks],                 [Material.Claw[3], Material.IronOre[2], Material.PalmLog[1]]),
        new(48, "Bed",                    9, 8, 120, When.Either, [Data.Categories.Furnishings, Data.Categories.Textiles],                [Material.Fur[4], Material.CottonBoll[2], Material.Log[2]]),
        new(49, "Scale Fingers",          9, 8, 120, When.Either, [Data.Categories.Attire, Data.Categories.CreatureCreations],            [Material.Carapace[4], Material.IronOre[2], Material.CottonBoll[2]]),
        new(50, "Crook",                  9, 8, 120, When.Either, [Data.Categories.Arms, Data.Categories.Woodworks],                      [Material.Fang[4], Material.Quartz[2], Material.Log[2]]),
        new(51, "Coral Sword",           10, 8,  72, When.Either, [Data.Categories.Arms, Data.Categories.MarineMerchandise],              [Material.Coral[3], Material.Resin[3], Material.PalmLog[2]]),
        new(52, "Coconut Juice",         10, 4,  36, When.Always, [Data.Categories.Confections, Data.Categories.Concoctions],             [Material.Coconut[2], Material.Sugarcane[2]]),
        new(53, "Honey",                 10, 4,  36, When.Always, [Data.Categories.Confections, Data.Categories.Ingredients],             [Material.BeehiveChip[2], Material.Sap[2]]),
        new(54, "Seashine Opal",         10, 8,  80, When.Either, [Data.Categories.UnburiedTreasures],                                    [Material.WoodOpal[4], Material.Sand[4]]),
        new(55, "Dried Flower",          10, 6,  54, When.Either, [Data.Categories.Sundries, Data.Categories.Furnishings],                [Material.MulticoloredIslebloooms[1], Material.Coconut[2], Material.Sap[1]]),
        new(56, "Powdered Paprika ",     11, 4,  52, When.Either, [Data.Categories.Ingredients, Data.Categories.Concoctions],             [Material.Paprika[2], Material.Islewort[2]]),
        new(57, "Cawl Cennin",           11, 6,  90, When.Either, [Data.Categories.Concoctions, Data.Categories.CreatureCreations],       [Material.Leek[3], Material.Milk[1], Material.Laver[3]]),
        new(58, "Isloaf",                11, 4,  52, When.Either, [Data.Categories.Foodstuffs, Data.Categories.Concoctions],              [Material.Wheat[2], Material.Islefish[1], Material.RockSalt[1]]),
        new(59, "Popoto Salad",          11, 4,  52, When.Either, [Data.Categories.Foodstuffs],                                           [Material.Popoto[2], Material.Apple[1], Material.Islewort[1]]),
        new(60, "Dressing",              11, 4,  52, When.Either, [Data.Categories.Ingredients],                                          [Material.Onion[2], Material.Sap[1], Material.Laver[1]]),
        new(61, "Stove",                 12, 6,  54, When.Either, [Data.Categories.Furnishings, Data.Categories.Metalworks],              [Material.Coal[2], Material.IronOre[3], Material.Leucogranite[1]]),
        new(62, "Lantern",               12, 8,  80, When.Either, [Data.Categories.Sundries],                                             [Material.Glimshroom[3], Material.Quartz[3], Material.CopperOre[2]]),
        new(63, "Natron",                12, 4,  36, When.Always, [Data.Categories.Sundries, Data.Categories.Concoctions],                [Material.EffervescentWater[2], Material.RockSalt[2]]),
        new(64, "Bouillabaisse",         12, 8, 136, When.Either, [Data.Categories.Foodstuffs, Data.Categories.MarineMerchandise],        [Material.CaveShrimp[2], Material.Clam[2], Material.Squid[2], Material.Tomato[2] ]),
        new(65, "Fossil Display",        13, 6,  54, When.Either, [Data.Categories.CreatureCreations, Data.Categories.UnburiedTreasures], [Material.Shale[3], Material.PalmLog[2], Material.CottonBoll[1]]),
        new(66, "Bathtub",               13, 8,  72, When.Either, [Data.Categories.Furnishings, Data.Categories.UnburiedTreasures],       [Material.Marble[4], Material.Leucogranite[2], Material.Clay[2]]),
        new(67, "Spectacles",            13, 6,  54, When.Either, [Data.Categories.Attire, Data.Categories.Sundries],                     [Material.MythrilOre[3], Material.Quartz[2], Material.CopperOre[1]]),
        new(68, "Cooling Glass",         13, 8,  80, When.Either, [Data.Categories.UnburiedTreasures],                                    [Material.Spectrine[4], Material.Sand[4]]),
        new(69, "Runner Bean Saute",     14, 4,  52, When.Either, [Data.Categories.Foodstuffs],                                           [Material.RunnerBeans[2], Material.RockSalt[2]]),
        new(70, "Beet Soup",             14, 6,  78, When.Either, [Data.Categories.Foodstuffs],                                           [Material.Beet[3], Material.Popoto[1], Material.Milk[1]]),
        new(71, "Imam Bayildi",          14, 6,  90, When.Either, [Data.Categories.Foodstuffs],                                           [Material.Eggplant[2], Material.Onion[2], Material.Tomato[2]]),
        new(72, "Pickled Zucchini",      14, 8, 104, When.Either, [Data.Categories.PreservedFood],                                        [Material.Zucchini[4], Material.Laver[2], Material.Sugarcane[2]]),
        new(73, "Brass Serving Dish",    16, 4,  36, When.Either, [Data.Categories.Sundries, Data.Categories.Metalworks],                 [Material.YellowCopperOre[2], Material.CopperOre[2]]),
        new(74, "Grinding Wheel",        16, 6,  60, When.Either, [Data.Categories.Sundries],                                             [Material.HawksEyeSand[2], Material.MythrilOre[2], Material.Sand[2]]),
        new(75, "Durium Tathlums",       17, 6,  54, When.Either, [Data.Categories.Arms, Data.Categories.Metalworks],                     [Material.DuriumSand[2], Material.IronOre[2], Material.Quartz[2]]),
        new(76, "Gold Hairpin",          17, 8,  72, When.Either, [Data.Categories.Accessories, Data.Categories.Metalworks],              [Material.GoldOre[4], Material.YellowCopperOre[2], Material.Quartz[2]]),
        new(77, "Mammet of Cycle Award", 17, 8,  80, When.Either, [Data.Categories.Furnishings],                                          [Material.CrystalFormation[2], Material.Spectrine[2], Material.MythrilOre[2], Material.Marble[2]]),
        new(78, "Fruit Punch",           18, 4,  52, When.Either, [Data.Categories.Confections],                                          [Material.Watermelon[1], Material.Isleberry[1], Material.Apple[1], Material.Coconut[1]]),
        new(79, "Sweet Popoto Pie",      18, 8, 120, When.Either, [Data.Categories.Foodstuffs, Data.Categories.Confections],              [Material.SweetPopoto[3], Material.Wheat[1], Material.Egg[1], Material.Sugarcane[3]]),
        new(80, "Peperoncino",           18, 6,  75, When.Either, [Data.Categories.Foodstuffs],                                           [Material.Broccoli[2], Material.Wheat[1], Material.RockSalt[3]]),
        new(81, "Buffalo Bean Salad",    18, 4,  52, When.Either, [Data.Categories.Foodstuffs, Data.Categories.CreatureCreations],        [Material.BuffaloBeans[2], Material.Milk[2]]),
    ];
}
