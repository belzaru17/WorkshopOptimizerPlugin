using FFXIVClientStructs.FFXIV.Client.Game;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Utils;

public static class ManagerProvider
{
    unsafe public static MJIManager* GetManager()
    {
        MJIManager* manager = MJIManager.Instance();
        if (manager == null) { return null; }
        for (uint i = 0; i < Constants.MaxItems; i++)
        {
            if (manager->GetSupplyForCraftwork(i) != CraftworkSupply.Nonexistent || manager->GetDemandShiftForCraftwork(i) != CraftworkDemandShift.Skyrocketing)
            {
                return manager;
            }
        }
        return null;
    }
}
