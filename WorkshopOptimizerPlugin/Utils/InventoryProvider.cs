using FFXIVClientStructs.FFXIV.Client.Game;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Utils;

internal class InventoryProvider
{
    public static unsafe int GetItemCount(Material material)
    {
        var instance = InventoryManager.Instance();
        return instance != null? instance->GetInventoryItemCount(material.Id) : -1;
    }
}
