using System.Collections.Generic;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;

namespace WorkshopOptimizerPlugin.Windows.Utils;

internal interface IItemSetsCache
{
    public List<ItemSet>?[] CachedItemSets { get; }
    public List<WorkshopsItemSets>?[] CachedWorkshopsItemSets { get; }
}

internal class ItemSetsCache : IItemSetsCache, IUIDataSourceListener
{
    public List<ItemSet>?[] CachedItemSets => cachedItemSets;
    public List<WorkshopsItemSets>?[] CachedWorkshopsItemSets => cachedWorkshopsItemSets;

    public ItemSetsCache()
    {
        cachedItemSets = new List<ItemSet>[Constants.MaxCycles];
        cachedWorkshopsItemSets = new List<WorkshopsItemSets>[Constants.MaxCycles];
    }

    public void OnDataChange(int cycle)
    {
        for (var c = cycle; c < Constants.MaxCycles; c++)
        {
            cachedItemSets[c] = null;
            cachedWorkshopsItemSets[c] = null;
        }
    }

    private List<ItemSet>?[] cachedItemSets;
    private List<WorkshopsItemSets>?[] cachedWorkshopsItemSets;
}
