using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Persistence.Internal;

internal class PersistentItemData(uint id, string name, ItemDynamicData data)
{
    public uint Id { get; set; } = id;
    public string Name { get; set; } = name;
    public ItemDynamicData Data { get; set; } = data;

    public PersistentItemData() : this(0, "UNKNOWN") { }

    public PersistentItemData(ItemStaticData staticData, ItemDynamicData data) : this(staticData.Id, staticData.Name, data) { }

    public PersistentItemData(uint id, string name) : this(id, name, new ItemDynamicData()) { }
}
