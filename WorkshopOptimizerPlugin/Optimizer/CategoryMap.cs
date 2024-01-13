using System.Collections.Generic;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Optimizer;

internal static class CategoryMap
{
    static CategoryMap()
    {
        CategoryToItems = [];

        for (uint i = 0; i < Constants.MaxItems; i++)
        {
            var item = ItemStaticData.Get(i);
            if (!item.IsValid()) { continue; }

            foreach (var category in item.Categories)
            {
                if (!CategoryToItems.TryGetValue(category, out var value))
                {
                    value = ([]);
                    CategoryToItems.Add(category, value);
                }

                value.Add(item);
            }
        }
    }

    public static List<ItemStaticData> GetEfficientItems(Categories[] categories)
    {
        List<ItemStaticData> items = [];
        foreach (var category in categories)
        {
            items.AddRange(CategoryToItems[category]);
        }
        return items;
    }

    private static readonly Dictionary<Categories, List<ItemStaticData>> CategoryToItems;
}
