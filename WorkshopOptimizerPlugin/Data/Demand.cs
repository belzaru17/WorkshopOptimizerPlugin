using FFXIVClientStructs.FFXIV.Client.Game.MJI;
using System;

namespace WorkshopOptimizerPlugin.Data;

internal enum Demand
{
    Unknown = 0,
    Skyrocketing = 1,
    Increasing = 2,
    None = 3,
    Decreasing = 4,
    Plummeting = 5,
    Any = 6,
}

internal static class DemandUtils
{
    public static Demand FromFFXIV(CraftworkDemandShift d)
    {
        return d switch
        {
            CraftworkDemandShift.Skyrocketing => Demand.Skyrocketing,
            CraftworkDemandShift.Increasing => Demand.Increasing,
            CraftworkDemandShift.None => Demand.None,
            CraftworkDemandShift.Decreasing => Demand.Decreasing,
            CraftworkDemandShift.Plummeting => Demand.Plummeting,
            _ => Demand.Unknown,
        };
    }

    public static Tuple<Demand, Demand> Adjust(Demand d, int produced)
    {
        for (; produced >= DemandAdjustmentBucket; produced -= DemandAdjustmentBucket)
        {
            d = NextDemand[(int)d];

        }
        if (produced == 0)
        {
            return new Tuple<Demand, Demand>(d, d);
        }
        return new Tuple<Demand, Demand>(d, NextDemand[(int)d]);
    }

    public static bool Within(Demand d, Demand min, Demand max)
    {
        if (min != Demand.Any && (((int)d) < ((int)min))) {
            return false;
        }
        if (max != Demand.Any && (((int)d) > ((int)max))) {
            return false;
        }
        return true;
    }


    private const int DemandAdjustmentBucket = 6;
    private static readonly Demand[] NextDemand =
    [
        Demand.Unknown,
        Demand.Increasing,
        Demand.None,
        Demand.Decreasing,
        Demand.Plummeting,
        Demand.Plummeting,
        Demand.Any,
    ];
}
