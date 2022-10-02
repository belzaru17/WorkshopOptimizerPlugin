using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Persistence.Internal;

internal class PersistentItemData
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public ItemDynamicData Data { get; set; }

    public PersistentItemData() : this(0, "UNKNOWN") { }

    public PersistentItemData(ItemStaticData staticData, ItemDynamicData data) : this(staticData.Id, staticData.Name, data) { }

    public PersistentItemData(uint id, string name) : this(id, name, new ItemDynamicData()) { }

    public PersistentItemData(uint id, string name, ItemDynamicData data)
    {
        Id = id;
        Name = name;
        Data = data;
    }
}
