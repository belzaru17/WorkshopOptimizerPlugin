using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.MJI;
using System;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Utils;

internal abstract class Manager
{
    public Manager() { }

    abstract public bool IsValid { get; }

    abstract public int CurrentPopularityIndex { get; }

    abstract public int NextPopularityIndex { get; }

    public abstract Supply GetSupplyForCraftwork(uint i);

    public abstract Demand GetDemandShiftForCraftwork(uint i);
}

internal class DirectReaderManager : Manager
{
    [Signature("E8 ?? ?? ?? ?? 8B 50 10")]
    private readonly unsafe delegate* unmanaged<IntPtr> readerInstance = null!;

    public DirectReaderManager()
    {
        SignatureHelper.Initialise(this);
    }

    public override unsafe bool IsValid {
        get {
            var instance = readerInstance();
            if (instance == IntPtr.Zero) return false;

            for (uint i = 0; i < Constants.MaxItems; i++)
            {
                if (GetSupplyForCraftwork(i) != Supply.Nonexistent || GetDemandShiftForCraftwork(i) != Demand.Skyrocketing)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public override unsafe int CurrentPopularityIndex {
        get {
            var instance = readerInstance();
            if (instance == IntPtr.Zero) return 0;
            return *(byte*)(instance + 0x270);
        }
    }

    public override unsafe int NextPopularityIndex
    {
        get
        {
            var instance = readerInstance();
            if (instance == IntPtr.Zero) return 0;
            return *(byte*)(instance + 0x271);
        }
    }

    public override unsafe Supply GetSupplyForCraftwork(uint i)
    {
        var instance = readerInstance();
        if (instance == IntPtr.Zero) return Supply.Unknown;
        return SupplyUtils.FromFFXIV((CraftworkSupply)((*(byte*)(instance + 0x272 + i) & 0xF0) >> 4));
    }

    public override unsafe Demand GetDemandShiftForCraftwork(uint i)
    {
        var instance = readerInstance();
        if (instance == IntPtr.Zero) return Demand.Unknown;
        return DemandUtils.FromFFXIV((CraftworkDemandShift)(*(byte*)(instance + 0x272 + i) & 0x0F));
    }
}


internal class MJIManagerAdaptor : Manager
{
    public unsafe MJIManagerAdaptor()
    {
        MJIManager* manager = MJIManager.Instance();
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

    private unsafe MJIManager* manager = null;
}

internal static class ManagerProvider
{
    public static Manager GetManager()
    {
        return new DirectReaderManager();
    }
}
