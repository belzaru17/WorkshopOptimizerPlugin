using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Persistence;

namespace WorkshopOptimizerPlugin.Optimizer;

internal class ItemCache(DynamicDataAdaptor dynamicData, ProducedItemsAdaptor producedItems, WhenOverrides whenOverrides)
{
    public Item? Get(ItemStaticData? staticData)
    {
        return (staticData == null)? null : this[staticData];
    }

    public Item this[ItemStaticData staticData]
    {
        get
        {
            if (items[staticData.Id] == null)
            {
                items[staticData.Id] = new Item(staticData, dynamicData[(int)staticData.Id], producedItems, whenOverrides);
            }
            return items[staticData.Id];
        }
    }

    private readonly DynamicDataAdaptor dynamicData = dynamicData;
    private readonly ProducedItemsAdaptor producedItems = producedItems;
    private readonly WhenOverrides whenOverrides = whenOverrides;
    private readonly Item[] items = new Item[Constants.MaxItems];
}
