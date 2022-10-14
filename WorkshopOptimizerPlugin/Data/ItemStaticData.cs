using System.Runtime.CompilerServices;

namespace WorkshopOptimizerPlugin.Data;

internal class ItemStaticData
{
    public readonly uint Id;
    public readonly string Name;
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

    private ItemStaticData(uint id, string name, int hours, int value, When when, Categories[] categories, Materials[] materials)
    {
        Id = id;
        Name = name;
        Hours = hours;
        Value = value;
        When = when;
        Categories = categories;
        Materials = materials;
    }

    private static readonly ItemStaticData[] ItemsData = new ItemStaticData[62]{
        new ItemStaticData( 0, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData( 1, "Potion",              4,  33, When.Always, new Categories[]{Data.Categories.Concoctions},                                      new Materials[]{Material.PalmLeaf[2], Material.Islewort[2]}),
        new ItemStaticData( 2, "Firesand",            4,  33, When.Always, new Categories[]{Data.Categories.Concoctions, Data.Categories.UnburiedTreasures},   new Materials[]{Material.Sand[2], Material.Limestone[1], Material.Islewort[1]}),
        new ItemStaticData( 3, "Wooden Chair",        6,  50, When.Always, new Categories[]{Data.Categories.Furnishings, Data.Categories.Woodworks},           new Materials[]{Material.Log[4], Material.Vine[2]}),
        new ItemStaticData( 4, "Grilled Clam",        4,  33, When.Always, new Categories[]{Data.Categories.Foodstuffs, Data.Categories.MarineMerchandise},    new Materials[]{Material.Clam[2], Material.Laver[2]}),
        new ItemStaticData( 5, "Necklace",            4,  33, When.Always, new Categories[]{Data.Categories.Accessories, Data.Categories.Woodworks},           new Materials[]{Material.Branch[3], Material.Vine[1]}),
        new ItemStaticData( 6, "Coral Ring",          6,  50, When.Always, new Categories[]{Data.Categories.Accessories, Data.Categories.MarineMerchandise},   new Materials[]{Material.Coral[3], Material.Vine[3]}),
        new ItemStaticData( 7, "Barbut",              6,  50, When.Always, new Categories[]{Data.Categories.Attire, Data.Categories.Metalworks},               new Materials[]{Material.CopperOre[3], Material.Sand[3]}),
        new ItemStaticData( 8, "Macuahuitl",          6,  50, When.Always, new Categories[]{Data.Categories.Arms, Data.Categories.Woodworks},                  new Materials[]{Material.PalmLog[3], Material.Stone[3]}),
        new ItemStaticData( 9, "Sauerkraut",          4,  48, When.Always, new Categories[]{Data.Categories.PreservedFood},                                    new Materials[]{Material.Cabbage[1], Material.RockSalt[3]}),
        new ItemStaticData(10, "Baked Pumpkin",       4,  48, When.Always, new Categories[]{Data.Categories.Foodstuffs},                                       new Materials[]{Material.Pumpkin[1], Material.Sap[3]}),
        new ItemStaticData(11, "Tunic",               6,  86, When.Either, new Categories[]{Data.Categories.Attire, Data.Categories.Textiles},                 new Materials[]{Material.Fleece[2], Material.Vine[4]}),
        new ItemStaticData(12, "Culinary Knife",      4,  52, When.Always, new Categories[]{Data.Categories.Sundries, Data.Categories.CreatureCreations},      new Materials[]{Material.Claw[1], Material.PalmLog[3]}),
        new ItemStaticData(13, "Brush",               4,  52, When.Always, new Categories[]{Data.Categories.Sundries, Data.Categories.Woodworks},              new Materials[]{Material.Fur[1], Material.PalmLog[3]}),
        new ItemStaticData(14, "Boiled Egg",          4,  52, When.Always, new Categories[]{Data.Categories.Foodstuffs, Data.Categories.CreatureCreations},    new Materials[]{Material.Egg[1], Material.Laver[3]}),
        new ItemStaticData(15, "Hora",                6,  86, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.CreatureCreations},          new Materials[]{Material.Carapace[2], Material.Stone[4]}),
        new ItemStaticData(16, "Earrings",            4,  52, When.Either, new Categories[]{Data.Categories.Accessories, Data.Categories.CreatureCreations},   new Materials[]{Material.Fang[1], Material.Vine[3]}),
        new ItemStaticData(17, "Butter",              4,  52, When.Either, new Categories[]{Data.Categories.Ingredients, Data.Categories.CreatureCreations},   new Materials[]{Material.Milk[1], Material.RockSalt[3]}),
        new ItemStaticData(18, "Brick Counter",       6,  57, When.Always, new Categories[]{Data.Categories.Furnishings, Data.Categories.UnburiedTreasures},   new Materials[]{Material.Clay[2], Material.Limestone[2], Material.PalmLog[2]}),
        new ItemStaticData(19, "Bronze Sheep",        8,  76, When.Either, new Categories[]{Data.Categories.Furnishings, Data.Categories.Metalworks},          new Materials[]{Material.Tinsand[3], Material.CopperOre[3], Material.Log[2]}),
        new ItemStaticData(20, "Growth Formula",      8, 163, When.Either, new Categories[]{Data.Categories.Concoctions},                                      new Materials[]{Material.Alyssum[2], Material.Islewort[3], Material.Branch[3]}),
        new ItemStaticData(21, "Garnet Rapier",       8, 163, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.UnburiedTreasures},          new Materials[]{Material.RawGarnet[2], Material.CopperOre[3], Material.Tinsand[3]}),
        new ItemStaticData(22, "Spruce Round Shield", 8, 163, When.Either, new Categories[]{Data.Categories.Attire, Data.Categories.Woodworks},                new Materials[]{Material.SpruceLog[2], Material.CopperOre[3], Material.Stone[3]}),
        new ItemStaticData(23, "Shark Oil",           8, 163, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.MarineMerchandise},      new Materials[]{Material.Hammerhead[2], Material.Laver[3], Material.Sap[3]}),
        new ItemStaticData(24, "Silver Ear Cuffs",    8, 163, When.Either, new Categories[]{Data.Categories.Accessories, Data.Categories.Metalworks},          new Materials[]{Material.SilverOre[2], Material.Tinsand[3], Material.Coral[3]}),
        new ItemStaticData(25, "Sweet Popoto",        6,  86, When.Either, new Categories[]{Data.Categories.Confections},                                      new Materials[]{Material.Popoto[1], Material.Milk[1], Material.Sap[3]}),
        new ItemStaticData(26, "Parsnip Salad",       4,  57, When.Always, new Categories[]{Data.Categories.Foodstuffs},                                       new Materials[]{Material.Parsnip[2], Material.Islewort[1], Material.Sap[1]}),
        new ItemStaticData(27, "Caramels",            6,  97, When.Either, new Categories[]{Data.Categories.Confections},                                      new Materials[]{Material.Sugarcane[4], Material.Milk[2]}),
        new ItemStaticData(28, "Ribbon",              6,  64, When.Either, new Categories[]{Data.Categories.Accessories, Data.Categories.Textiles},            new Materials[]{Material.CottonBoll[2], Material.CopperOre[2], Material.Vine[2]}),
        new ItemStaticData(29, "Rope",                4,  43, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.Textiles},               new Materials[]{Material.Hemp[2], Material.Islewort[1], Material.Vine[1]}),
        new ItemStaticData(30, "Cavalier's Hat",      6,  97, When.Either, new Categories[]{Data.Categories.Attire, Data.Categories.Textiles},                 new Materials[]{Material.Feather[2], Material.CottonBoll[2], Material.Hemp[2]}),
        new ItemStaticData(31, "Horn",                6,  97, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.CreatureCreations},      new Materials[]{Material.Horn[2], Material.Clay[2], Material.Hemp[2]}),
        new ItemStaticData(32, "Salt Cod",            6,  64, When.Either, new Categories[]{Data.Categories.PreservedFood, Data.Categories.MarineMerchandise}, new Materials[]{Material.Islefish[4], Material.RockSalt[2]}),
        new ItemStaticData(33, "Squid Ink",           4,  43, When.Always, new Categories[]{Data.Categories.Ingredients, Data.Categories.MarineMerchandise},   new Materials[]{Material.Squid[2], Material.RockSalt[2] }),
        new ItemStaticData(34, "Essential Draught",   6,  64, When.Either, new Categories[]{Data.Categories.Concoctions, Data.Categories.MarineMerchandise},   new Materials[]{Material.Jellyfish[2], Material.PalmLeaf[2], Material.Laver[2]}),
        new ItemStaticData(35, "Isleberry Jam",       6,  93, When.Either, new Categories[]{Data.Categories.Ingredients},                                      new Materials[]{Material.Isleberry[3], Material.Sugarcane[2], Material.Sap[1]}),
        new ItemStaticData(36, "Tomato Relish",       4,  62, When.Either, new Categories[]{Data.Categories.Ingredients},                                      new Materials[]{Material.Tomato[2], Material.Islewort[1], Material.Sap[1]}),
        new ItemStaticData(37, "Onion Soup",          6,  93, When.Either, new Categories[]{Data.Categories.Foodstuffs},                                       new Materials[]{Material.Onion[3], Material.RockSalt[2], Material.Islewort[1]}),
        new ItemStaticData(38, "Islefish Pie",        6,  93, When.Either, new Categories[]{Data.Categories.Confections, Data.Categories.MarineMerchandise},   new Materials[]{Material.Wheat[3], Material.Islefish[2], Material.Sugarcane[1]}),
        new ItemStaticData(39, "Corn Flakes",         4,  62, When.Either, new Categories[]{Data.Categories.PreservedFood},                                    new Materials[]{Material.Corn[2], Material.Sugarcane[2]}),
        new ItemStaticData(40, "Pickled Radish",      8, 124, When.Either, new Categories[]{Data.Categories.PreservedFood},                                    new Materials[]{Material.Radish[4], Material.Apple[2], Material.Sugarcane[2]}),
        new ItemStaticData(41, "Iron Axe",            8,  86, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.Metalworks},                 new Materials[]{Material.IronOre[3], Material.Log[3], Material.Sand[2]}),
        new ItemStaticData(42, "Quartz Ring",         8,  86, When.Either, new Categories[]{Data.Categories.Accessories, Data.Categories.UnburiedTreasures},   new Materials[]{Material.Quartz[3], Material.IronOre[3], Material.Stone[2]}),
        new ItemStaticData(43, "Porcelain Vase",      8,  86, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.UnburiedTreasures},      new Materials[]{Material.Leucogranite[3], Material.Quartz[3], Material.Clay[2]}),
        new ItemStaticData(44, "Vegetable Juice",     6,  93, When.Either, new Categories[]{Data.Categories.Concoctions},                                      new Materials[]{Material.Cabbage[3], Material.Islewort[2], Material.Laver[1]}),
        new ItemStaticData(45, "Pumpkin Pudding",     6 , 93, When.Either, new Categories[]{Data.Categories.Confections},                                      new Materials[]{Material.Pumpkin[3], Material.Egg[1], Material.Milk[1]}),
        new ItemStaticData(46, "Sheepfluff Rug",      6, 108, When.Either, new Categories[]{Data.Categories.Furnishings, Data.Categories.CreatureCreations},   new Materials[]{Material.Fleece[3], Material.CottonBoll[2], Material.Hemp[1]}),
        new ItemStaticData(47, "Garden Scythe",       6, 108, When.Either, new Categories[]{Data.Categories.Sundries, Data.Categories.Metalworks},             new Materials[]{Material.Claw[3], Material.IronOre[2], Material.PalmLog[1]}),
        new ItemStaticData(48, "Bed",                 8, 144, When.Either, new Categories[]{Data.Categories.Furnishings, Data.Categories.Textiles},            new Materials[]{Material.Fur[4], Material.CottonBoll[2], Material.Log[2]}),
        new ItemStaticData(49, "Scale Fingers",       8, 144, When.Either, new Categories[]{Data.Categories.Attire, Data.Categories.CreatureCreations},        new Materials[]{Material.Carapace[4], Material.IronOre[2], Material.CottonBoll[2]}),
        new ItemStaticData(50, "Crook",               8, 144, When.Either, new Categories[]{Data.Categories.Arms, Data.Categories.Woodworks},                  new Materials[]{Material.Fang[4], Material.Quartz[2], Material.Log[2]}),
        new ItemStaticData(51, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(52, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(53, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(54, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(55, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(56, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(57, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(58, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(59, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(60, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
        new ItemStaticData(61, "UNKNOWN",             0,   0, When.Never,  new Categories[]{},                                                                 new Materials[]{}),
    };
}
