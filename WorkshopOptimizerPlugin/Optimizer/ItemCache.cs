using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Persistence;

namespace WorkshopOptimizerPlugin.Optimizer;

internal class ItemCache
{
    public ItemCache(DynamicDataAdaptor dynamicData, ProducedItemsAdaptor producedItems, WhenOverrides whenOverrides)
    {
        this.dynamicData = dynamicData;
        this.producedItems = producedItems;
        this.whenOverrides = whenOverrides;
        this.items = new Item[Constants.MaxItems];
    }

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

    private readonly DynamicDataAdaptor dynamicData;
    private readonly ProducedItemsAdaptor producedItems;
    private readonly WhenOverrides whenOverrides;
    private readonly Item[] items;
}
