using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Persistence;

namespace WorkshopOptimizerPlugin.Optimizer;

internal class ItemCache
{
    public ItemCache(DataSource dataSource, WhenOverrides whenOverrides)
    {
        this.dataSource = dataSource;
        this.whenOverrides = whenOverrides;
        this.items = new Item[Constants.MaxItems];
    }

    public Item this[ItemStaticData staticData]
    {
        get
        {
            if (items[staticData.Id] == null)
            {
                items[staticData.Id] = new Item(staticData, dataSource.DynamicData[(int)staticData.Id], dataSource.ProducedItems, whenOverrides);
            }
            return items[staticData.Id];
        }
    }

    private readonly DataSource dataSource;
    private readonly WhenOverrides whenOverrides;
    private Item[] items;
}
