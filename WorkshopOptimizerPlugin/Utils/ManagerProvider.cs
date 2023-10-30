using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.MJI;
using System;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Utils;

internal abstract class Manager
{
    public Manager() { }

    public abstract bool IsValid { get; }

    public abstract int CurrentPopularityIndex { get; }

    public abstract int NextPopularityIndex { get; }

    public abstract Supply GetSupplyForCraftwork(uint i);

    public abstract Demand GetDemandShiftForCraftwork(uint i);
}

internal class DirectReaderManager : Manager
{
    [Signature("E8 ?? ?? ?? ?? 8B 50 10")]
    private readonly unsafe delegate* unmanaged<IntPtr> readerInstance = null!;

    public DirectReaderManager()
    {
        Plugin.GameInteropProvider.InitializeFromAttributes(this);
    }

    public override unsafe bool IsValid
    {
        get {
            var instance = readerInstance();
            if (instance == IntPtr.Zero) return false;

            var any_valid = false;
            for (uint i = 0; i < Constants.MaxItems; i++)
            {
                if (GetSupplyForCraftwork(i) == Supply.Unknown || GetDemandShiftForCraftwork(i) == Demand.Unknown)
                {
                    return false;
                }
                if (GetSupplyForCraftwork(i) != Supply.Nonexistent|| GetDemandShiftForCraftwork(i) != Demand.Skyrocketing)
                {
                    any_valid = true;
                }
            }

            return any_valid;
        }
    }

    private const int OFFSET_ITEMS = 0x2f2;
    private const int OFFSET_CURRENT_POPULARITY_INDEX = OFFSET_ITEMS - 2;
    private const int OFFSET_NEXT_POPULARITY_INDEX = OFFSET_ITEMS - 1;

    public override unsafe int CurrentPopularityIndex {
        get {
            var instance = readerInstance();
            if (instance == IntPtr.Zero) return 0;
            return *(byte*)(instance + OFFSET_CURRENT_POPULARITY_INDEX);
        }
    }

    public override unsafe int NextPopularityIndex
    {
        get
        {
            var instance = readerInstance();
            if (instance == IntPtr.Zero) return 0;
            return *(byte*)(instance + OFFSET_NEXT_POPULARITY_INDEX);
        }
    }

    public override unsafe Supply GetSupplyForCraftwork(uint i)
    {
        var instance = readerInstance();
        if (instance == IntPtr.Zero) return Supply.Unknown;
        return SupplyUtils.FromFFXIV((CraftworkSupply)((*(byte*)(instance + OFFSET_ITEMS + i) & 0xF0) >> 4));
    }

    public override unsafe Demand GetDemandShiftForCraftwork(uint i)
    {
        var instance = readerInstance();
        if (instance == IntPtr.Zero) return Demand.Unknown;
        return DemandUtils.FromFFXIV((CraftworkDemandShift)(*(byte*)(instance + OFFSET_ITEMS + i) & 0x0F));
    }
}


internal class MJIManagerAdaptor : Manager
{
    public unsafe MJIManagerAdaptor()
    {
        var manager = MJIManager.Instance();
        if (manager == null) { return; }
        for (uint i = 0; i < Constants.MaxItems; i++)
        {
            if (manager->GetSupplyForCraftwork(i) != CraftworkSupply.Nonexistent || manager->GetDemandShiftForCraftwork(i) != CraftworkDemandShift.Skyrocketing)
            {
                this.manager = manager;
                return;
            }
        }
    }
    
    public override unsafe bool IsValid { get { return manager != null; } }

    public override unsafe int CurrentPopularityIndex { get { return IsValid? manager->CurrentPopularity : 0; } }

    public override unsafe int NextPopularityIndex { get { return IsValid ? manager->NextPopularity : 0; } }

    public override unsafe Supply GetSupplyForCraftwork(uint i)
    {
        return IsValid? SupplyUtils.FromFFXIV(manager->GetSupplyForCraftwork(i)) : Supply.Unknown;
    }

    public override unsafe Demand GetDemandShiftForCraftwork(uint i)
    {
        return IsValid? DemandUtils.FromFFXIV(manager->GetDemandShiftForCraftwork(i)) : Demand.Unknown;
    }

    private readonly unsafe MJIManager* manager = null;
}

internal static class ManagerProvider
{
    public static Manager GetManager()
    {
        return new DirectReaderManager();
    }
}
