using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

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

    private static readonly ItemStaticData[] ItemsData = {
        new ItemStaticData( 0, "UNKNOWN",                0, 0,   0, When.Never,  System.Array.Empty<Categories>(),                                                       System.Array.Empty<Materials>()),
        new ItemStaticData( 1, "Potion",                 1, 4,  28, When.Always, new Categories[]{Data.Categories.Concoctions},                                          new Materials[]{Material.PalmLeaf[2], Material.Islewort[2]}),
        new ItemStaticData( 2, "Firesand",               1, 4,  28, When.Always, new Categories[]{Data.Categories.Concoctions, Data.Categories.UnburiedTreasures},       new Materials[]{Material.Sand[2], Material.Limestone[1], Material.Islewort[1]}),
        new ItemStaticData( 3, "Wooden Chair",           1, 6,  42, When.Always, new Categories[]{Data.Categories.Furnishings, Data.Categories.Woodworks},               new Materials[]{Material.Log[4], Material.Vine[2]}),
        new ItemStaticData( 4, "Grilled Clam",           1, 4,  28, When.Always, new Categories[]{Data.Categories.Foodstuffs, Data.Categories.MarineMerchandise},        new Materials[]{Material.Clam[2], Material.Laver[2]}),
        new ItemStaticData( 5, "Necklace",               1, 4,  28, When.Always, new Categories[]{Data.Categories.Accessories, Data.Categories.Woodworks},               new Materials[]{Material.Branch[3], Material.Vine[1]}),
        new ItemStaticData( 6, "Coral Ring",             1, 6,  42, When.Always, new Categories[]{Data.Categories.Accessories, Data.Categories.MarineMerchandise},       new Materials[]{Material.Coral[3], Material.Vine[3]}),
        new ItemStaticData( 7, "Barbut",                 1, 6,  42, When.Always, new Categories[]{Data.Categories.Attire, Data.Categories.Metalworks},                   new Materials[]{Material.CopperOre[3], Material.Sand[3]}),
        new ItemStaticData( 8, "Macuahuitl",             1, 6,  42, When.Always, new Categories[]{Data.Categories.Arms, Data.Categories.Woodworks},                      new Materials[]{Material.PalmLog[3], Material.Stone[3]}),
        new ItemStaticData( 9, "Sauerkraut",             1, 4,  40, When.Always, new Categories[]{Data.Categories.PreservedFood},                                        new Materials[]{Material.Cabbage[1], Material.RockSalt[3]}),
        new ItemStaticData(10, "Baked Pumpkin",          1, 4,  40, When.Always, new Categories[]{Data.Categories.Foodstuffs},                                           new Materials[]{Material.Pumpkin[1], Material.Sap[3]}),
        new ItemStaticData(11, "Tunic",                  1, 6,  72, When.Either, new Categories[]{Data.Categories.Attire, Data.Categories.Textiles},                     new Materials[]{Material.Fleece[2], Material.Vine[4]}),
        new ItemStaticData(12, "Culinary Knife",         1, 4,  44, When.Always, new Categories[]{Data.Categories.Sundries, Data.Categories.CreatureCreations},          new Materials[]{Material.Claw[1], Material.PalmLog[3]}),
        new ItemStaticData(13, "Brush",                  1, 4,  44, When.Always, new Categories[]{Data.Categories.Sundries, Data.Categories.Woodworks},                  new Materials[]{Material.Fur[1], Material.PalmLog[3]}),
        new ItemStaticData(14, "Boiled Egg",             1, 4,  44, When.Always, new Categories[]{Data.Categories.Foodstuffs, Data.Categories.CreatureCreations},        new Materials[]{Material.Egg[1], Material.Laver[3]}),
        new ItemStaticData(15, "Hora",                   1, 6,  72, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.CreatureCreations},              new Materials[]{Material.Carapace[2], Material.Stone[4]}),
        new ItemStaticData(16, "Earrings",               1, 4,  44, When.Either, new Categories[]{Data.Categories.Accessories, Data.Categories.CreatureCreations},       new Materials[]{Material.Fang[1], Material.Vine[3]}),
        new ItemStaticData(17, "Butter",                 1, 4,  44, When.Either, new Categories[]{Data.Categories.Ingredients, Data.Categories.CreatureCreations},       new Materials[]{Material.Milk[1], Material.RockSalt[3]}),
        new ItemStaticData(18, "Brick Counter",          5, 6,  48, When.Always, new Categories[]{Data.Categories.Furnishings, Data.Categories.UnburiedTreasures},       new Materials[]{Material.Clay[2], Material.Limestone[2], Material.PalmLog[2]}),
        new ItemStaticData(19, "Bronze Sheep",           5, 8,  64, When.Either, new Categories[]{Data.Categories.Furnishings, Data.Categories.Metalworks},              new Materials[]{Material.Tinsand[3], Material.CopperOre[3], Material.Log[2]}),
        new ItemStaticData(20, "Growth Formula",         5, 8, 136, When.Either, new Categories[]{Data.Categories.Concoctions},                                          new Materials[]{Material.Alyssum[2], Material.Islewort[3], Material.Branch[3]}),
        new ItemStaticData(21, "Garnet Rapier",          5, 8, 136, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.UnburiedTreasures},              new Materials[]{Material.RawGarnet[2], Material.CopperOre[3], Material.Tinsand[3]}),
        new ItemStaticData(22, "Spruce Round Shield",    5, 8, 136, When.Either, new Categories[]{Data.Categories.Attire, Data.Categories.Woodworks},                    new Materials[]{Material.SpruceLog[2], Material.CopperOre[3], Material.Stone[3]}),
        new ItemStaticData(23, "Shark Oil",              5, 8, 136, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.MarineMerchandise},          new Materials[]{Material.Hammerhead[2], Material.Laver[3], Material.Sap[3]}),
        new ItemStaticData(24, "Silver Ear Cuffs",       5, 8, 136, When.Either, new Categories[]{Data.Categories.Accessories, Data.Categories.Metalworks},              new Materials[]{Material.SilverOre[2], Material.Tinsand[3], Material.Coral[3]}),
        new ItemStaticData(25, "Sweet Popoto",           5, 6,  72, When.Either, new Categories[]{Data.Categories.Confections},                                          new Materials[]{Material.Popoto[1], Material.Milk[1], Material.Sap[3]}),
        new ItemStaticData(26, "Parsnip Salad",          5, 4,  48, When.Always, new Categories[]{Data.Categories.Foodstuffs},                                           new Materials[]{Material.Parsnip[2], Material.Islewort[1], Material.Sap[1]}),
        new ItemStaticData(27, "Caramels",               6, 6,  81, When.Either, new Categories[]{Data.Categories.Confections},                                          new Materials[]{Material.Sugarcane[4], Material.Milk[2]}),
        new ItemStaticData(28, "Ribbon",                 6, 6,  54, When.Either, new Categories[]{Data.Categories.Accessories, Data.Categories.Textiles},                new Materials[]{Material.CottonBoll[2], Material.CopperOre[2], Material.Vine[2]}),
        new ItemStaticData(29, "Rope",                   6, 4,  36, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.Textiles},                   new Materials[]{Material.Hemp[2], Material.Islewort[1], Material.Vine[1]}),
        new ItemStaticData(30, "Cavalier's Hat",         6, 6,  81, When.Either, new Categories[]{Data.Categories.Attire, Data.Categories.Textiles},                     new Materials[]{Material.Feather[2], Material.CottonBoll[2], Material.Hemp[2]}),
        new ItemStaticData(31, "Horn",                   6, 6,  81, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.CreatureCreations},          new Materials[]{Material.Horn[2], Material.Clay[2], Material.Hemp[2]}),
        new ItemStaticData(32, "Salt Cod",               7, 6,  54, When.Either, new Categories[]{Data.Categories.PreservedFood, Data.Categories.MarineMerchandise},     new Materials[]{Material.Islefish[4], Material.RockSalt[2]}),
        new ItemStaticData(33, "Squid Ink",              7, 4,  36, When.Always, new Categories[]{Data.Categories.Ingredients, Data.Categories.MarineMerchandise},       new Materials[]{Material.Squid[2], Material.RockSalt[2] }),
        new ItemStaticData(34, "Essential Draught",      7, 6,  54, When.Either, new Categories[]{Data.Categories.Concoctions, Data.Categories.MarineMerchandise},       new Materials[]{Material.Jellyfish[2], Material.PalmLeaf[2], Material.Laver[2]}),
        new ItemStaticData(35, "Isleberry Jam",          7, 6,  78, When.Either, new Categories[]{Data.Categories.Ingredients},                                          new Materials[]{Material.Isleberry[3], Material.Sugarcane[2], Material.Sap[1]}),
        new ItemStaticData(36, "Tomato Relish",          7, 4,  52, When.Either, new Categories[]{Data.Categories.Ingredients},                                          new Materials[]{Material.Tomato[2], Material.Islewort[1], Material.Sap[1]}),
        new ItemStaticData(37, "Onion Soup",             7, 6,  78, When.Either, new Categories[]{Data.Categories.Foodstuffs},                                           new Materials[]{Material.Onion[3], Material.RockSalt[2], Material.Islewort[1]}),
        new ItemStaticData(38, "Islefish Pie",           7, 6,  78, When.Either, new Categories[]{Data.Categories.Confections, Data.Categories.MarineMerchandise},       new Materials[]{Material.Wheat[3], Material.Islefish[2], Material.Sugarcane[1]}),
        new ItemStaticData(39, "Corn Flakes",            7, 4,  42, When.Either, new Categories[]{Data.Categories.PreservedFood},                                        new Materials[]{Material.Corn[2], Material.Sugarcane[2]}),
        new ItemStaticData(40, "Pickled Radish",         7, 8, 104, When.Either, new Categories[]{Data.Categories.PreservedFood},                                        new Materials[]{Material.Radish[4], Material.Apple[2], Material.Sugarcane[2]}),
        new ItemStaticData(41, "Iron Axe",               8, 8,  72, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.Metalworks},                     new Materials[]{Material.IronOre[3], Material.Log[3], Material.Sand[2]}),
        new ItemStaticData(42, "Quartz Ring",            8, 8,  72, When.Either, new Categories[]{Data.Categories.Accessories, Data.Categories.UnburiedTreasures},       new Materials[]{Material.Quartz[3], Material.IronOre[3], Material.Stone[2]}),
        new ItemStaticData(43, "Porcelain Vase",         8, 8,  72, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.UnburiedTreasures},          new Materials[]{Material.Leucogranite[3], Material.Quartz[3], Material.Clay[2]}),
        new ItemStaticData(44, "Vegetable Juice",        8, 6,  78, When.Either, new Categories[]{Data.Categories.Concoctions},                                          new Materials[]{Material.Cabbage[3], Material.Islewort[2], Material.Laver[1]}),
        new ItemStaticData(45, "Pumpkin Pudding",        8, 6 , 78, When.Either, new Categories[]{Data.Categories.Confections},                                          new Materials[]{Material.Pumpkin[3], Material.Egg[1], Material.Milk[1]}),
        new ItemStaticData(46, "Sheepfluff Rug",         8, 6,  90, When.Either, new Categories[]{Data.Categories.Furnishings, Data.Categories.CreatureCreations},       new Materials[]{Material.Fleece[3], Material.CottonBoll[2], Material.Hemp[1]}),
        new ItemStaticData(47, "Garden Scythe",          9, 6,  90, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.Metalworks},                 new Materials[]{Material.Claw[3], Material.IronOre[2], Material.PalmLog[1]}),
        new ItemStaticData(48, "Bed",                    9, 8, 120, When.Either, new Categories[]{Data.Categories.Furnishings, Data.Categories.Textiles},                new Materials[]{Material.Fur[4], Material.CottonBoll[2], Material.Log[2]}),
        new ItemStaticData(49, "Scale Fingers",          9, 8, 120, When.Either, new Categories[]{Data.Categories.Attire, Data.Categories.CreatureCreations},            new Materials[]{Material.Carapace[4], Material.IronOre[2], Material.CottonBoll[2]}),
        new ItemStaticData(50, "Crook",                  9, 8, 120, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.Woodworks},                      new Materials[]{Material.Fang[4], Material.Quartz[2], Material.Log[2]}),
        new ItemStaticData(51, "Coral Sword",           10, 8,  72, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.MarineMerchandise},              new Materials[]{Material.Coral[3], Material.Resin[3], Material.PalmLog[2]}),
        new ItemStaticData(52, "Coconut Juice",         10, 4,  36, When.Always, new Categories[]{Data.Categories.Confections, Data.Categories.Concoctions},             new Materials[]{Material.Coconut[2], Material.Sugarcane[2]}),
        new ItemStaticData(53, "Honey",                 10, 4,  36, When.Always, new Categories[]{Data.Categories.Confections, Data.Categories.Ingredients},             new Materials[]{Material.BeehiveChip[2], Material.Sap[2]}),
        new ItemStaticData(54, "Seashine Opal",         10, 8,  80, When.Either, new Categories[]{Data.Categories.UnburiedTreasures},                                    new Materials[]{Material.WoodOpal[4], Material.Sand[4]}),
        new ItemStaticData(55, "Dried Flower",          10, 6,  54, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.Furnishings},                new Materials[]{Material.MulticoloredIslebloooms[1], Material.Coconut[2], Material.Sap[1]}),
        new ItemStaticData(56, "Powdered Paprika ",     11, 4,  52, When.Either, new Categories[]{Data.Categories.Ingredients, Data.Categories.Concoctions},             new Materials[]{Material.Paprika[2], Material.Islewort[2]}),
        new ItemStaticData(57, "Cawl Cennin",           11, 6,  90, When.Either, new Categories[]{Data.Categories.Concoctions, Data.Categories.CreatureCreations},       new Materials[]{Material.Leek[3], Material.Milk[1], Material.Laver[3]}),
        new ItemStaticData(58, "Isloaf",                11, 4,  52, When.Either, new Categories[]{Data.Categories.Foodstuffs, Data.Categories.Concoctions},              new Materials[]{Material.Wheat[2], Material.Islefish[1], Material.RockSalt[1]}),
        new ItemStaticData(59, "Popoto Salad",          11, 4,  52, When.Either, new Categories[]{Data.Categories.Foodstuffs},                                           new Materials[]{Material.Popoto[2], Material.Apple[1], Material.Islewort[1]}),
        new ItemStaticData(60, "Dressing",              11, 4,  52, When.Either, new Categories[]{Data.Categories.Ingredients},                                          new Materials[]{Material.Onion[2], Material.Sap[1], Material.Laver[1]}),
        new ItemStaticData(61, "Stove",                 12, 6,  54, When.Either, new Categories[]{Data.Categories.Furnishings, Data.Categories.Metalworks},              new Materials[]{Material.Coal[2], Material.IronOre[3], Material.Leucogranite[1]}),
        new ItemStaticData(62, "Lantern",               12, 8,  80, When.Either, new Categories[]{Data.Categories.Sundries},                                             new Materials[]{Material.Glimshroom[3], Material.Quartz[3], Material.CopperOre[2]}),
        new ItemStaticData(63, "Natron",                12, 4,  36, When.Always, new Categories[]{Data.Categories.Sundries, Data.Categories.Concoctions},                new Materials[]{Material.EffervescentWater[2], Material.RockSalt[2]}),
        new ItemStaticData(64, "Bouillabaisse",         12, 8, 136, When.Either, new Categories[]{Data.Categories.Foodstuffs, Data.Categories.MarineMerchandise},        new Materials[]{Material.CaveShrimp[2], Material.Clam[2], Material.Squid[2], Material.Tomato[2] }),
        new ItemStaticData(65, "Fossil Display",        13, 6,  54, When.Either, new Categories[]{Data.Categories.CreatureCreations, Data.Categories.UnburiedTreasures}, new Materials[]{Material.Shale[3], Material.PalmLog[2], Material.CottonBoll[1]}),
        new ItemStaticData(66, "Bathtub",               13, 8,  72, When.Either, new Categories[]{Data.Categories.Furnishings, Data.Categories.UnburiedTreasures},       new Materials[]{Material.Marble[4], Material.Leucogranite[2], Material.Clay[2]}),
        new ItemStaticData(67, "Spectacles",            13, 6,  54, When.Either, new Categories[]{Data.Categories.Attire, Data.Categories.Sundries},                     new Materials[]{Material.MythrilOre[3], Material.Quartz[2], Material.CopperOre[1]}),
        new ItemStaticData(68, "Cooling Glass",         13, 8,  80, When.Either, new Categories[]{Data.Categories.UnburiedTreasures},                                    new Materials[]{Material.Spectrine[4], Material.Sand[4]}),
        new ItemStaticData(69, "Runner Bean Saute",     14, 4,  52, When.Either, new Categories[]{Data.Categories.Foodstuffs},                                           new Materials[]{Material.RunnerBeans[2], Material.RockSalt[2]}),
        new ItemStaticData(70, "Beet Soup",             14, 6,  78, When.Either, new Categories[]{Data.Categories.Foodstuffs},                                           new Materials[]{Material.Beet[3], Material.Popoto[1], Material.Milk[1]}),
        new ItemStaticData(71, "Imam Bayildi",          14, 6,  90, When.Either, new Categories[]{Data.Categories.Foodstuffs},                                           new Materials[]{Material.Eggplant[2], Material.Onion[2], Material.Tomato[2]}),
        new ItemStaticData(72, "Pickled Zucchini",      14, 8, 104, When.Either, new Categories[]{Data.Categories.PreservedFood},                                        new Materials[]{Material.Zucchini[4], Material.Laver[2], Material.Sugarcane[2]}),
        new ItemStaticData(73, "Brass Serving Dish",    16, 4,  36, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.Metalworks},                 new Materials[]{Material.YellowCopperOre[2], Material.CopperOre[2]}),
        new ItemStaticData(74, "Grinding Wheel",        16, 6,  60, When.Either, new Categories[]{Data.Categories.Sundries},                                             new Materials[]{Material.HawksEyeSand[2], Material.MythrilOre[2], Material.Sand[2]}),
        new ItemStaticData(75, "Durium Tathlums",       17, 6,  54, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.Metalworks},                     new Materials[]{Material.DuriumSand[2], Material.IronOre[2], Material.Quartz[2]}),
        new ItemStaticData(76, "Gold Hairpin",          17, 8,  72, When.Either, new Categories[]{Data.Categories.Accessories, Data.Categories.Metalworks},              new Materials[]{Material.GoldOre[4], Material.YellowCopperOre[2], Material.Quartz[2]}),
        new ItemStaticData(77, "Mammet of Cycle Award", 17, 8,  80, When.Either, new Categories[]{Data.Categories.Furnishings},                                          new Materials[]{Material.CrystalFormation[2], Material.Spectrine[2], Material.MythrilOre[2], Material.Marble[2]}),
        new ItemStaticData(78, "Fruit Punch",           18, 4,  52, When.Either, new Categories[]{Data.Categories.Confections},                                          new Materials[]{Material.Watermelon[1], Material.Isleberry[1], Material.Apple[1], Material.Coconut[1]}),
        new ItemStaticData(79, "Sweet Popoto Pie",      18, 8, 120, When.Either, new Categories[]{Data.Categories.Foodstuffs, Data.Categories.Confections},              new Materials[]{Material.SweetPopoto[3], Material.Wheat[1], Material.Egg[1], Material.Sugarcane[3]}),
        new ItemStaticData(80, "Peperoncino",           18, 6,  75, When.Either, new Categories[]{Data.Categories.Foodstuffs},                                           new Materials[]{Material.Broccoli[2], Material.Wheat[1], Material.RockSalt[3]}),
        new ItemStaticData(81, "Buffalo Bean Salad",    18, 4,  52, When.Either, new Categories[]{Data.Categories.Foodstuffs, Data.Categories.CreatureCreations},        new Materials[]{Material.BuffaloBeans[2], Material.Milk[2]}),
    };
}
