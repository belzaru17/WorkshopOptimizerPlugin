using FFXIVClientStructs.FFXIV.Client.Game.MJI;

namespace WorkshopOptimizerPlugin.Utils;

internal class IslandProvider
{
    public static unsafe int GetIslandRank()
    {
        return MJIManager.Instance()->IslandState.CurrentRank;
    }
}
