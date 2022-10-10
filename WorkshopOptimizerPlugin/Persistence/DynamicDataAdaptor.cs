using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Persistence.Internal;

namespace WorkshopOptimizerPlugin.Persistence;

internal class DynamicDataAdaptor
{
    public DynamicDataAdaptor(PersistentSeasonData data)
    {
        this.itemData = new ItemDynamicData[Constants.MaxItems];
        foreach (var i in data.ItemData)
        {
            itemData[i.Id] = i.Data;
        }
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] != null) continue;

            var dynamicData = new ItemDynamicData();
            var staticData = ItemStaticData.Get(i);
            if (staticData.IsValid())
            {
                data.ItemData.Add(new PersistentItemData(staticData, dynamicData));
            }
            itemData[i] = dynamicData;
        }
    }

    public ItemDynamicData this[int id]
    {
        get
        {
            return itemData[id];
        }
    }

    private ItemDynamicData[] itemData;
}
