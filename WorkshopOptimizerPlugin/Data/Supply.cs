using System;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Gauge;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace WorkshopOptimizerPlugin.Data;

internal enum Supply
{
    Unknown = 0,
    Nonexistent = 1,
    Insufficient = 2,
    Sufficient = 3,
    Surplus = 4,
    Overflowing = 5,
}

internal static class SupplyUtils
{
    public static double Multiplier(Supply s, int produced = 0)
    {
        for (; produced >= SupplyAdjustmentBucket; produced -= SupplyAdjustmentBucket)
        {
            s = NextSupply[(int)s];
        }
        var ns = NextSupply[(int)s];
        return (Multipliers[(int)s] * (SupplyAdjustmentBucket - produced) + Multipliers[(int)ns] * produced) / SupplyAdjustmentBucket;
    }

    public static Supply FromFFXIV(CraftworkSupply s)
    {
        switch (s)
        {
            case CraftworkSupply.Nonexistent:
                return Supply.Nonexistent;
            case CraftworkSupply.Insufficient:
                return Supply.Insufficient;
            case CraftworkSupply.Sufficient:
                return Supply.Sufficient;
            case CraftworkSupply.Surplus:
                return Supply.Surplus;
            case CraftworkSupply.Overflowing:
                return Supply.Overflowing;
        }
        return Supply.Unknown;
    }

    public static bool Within(Supply s, Supply min, Supply max)
    {
        return (((int)s) >= ((int)min)) && (((int)s) <= ((int)max));
    }

    private const int SupplyAdjustmentBucket = 8;
    private static readonly double[] Multipliers = { 0, 1.6, 1.3, 1, 0.8, 0.6 };
    private static readonly Supply[] NextSupply = {
        Supply.Unknown,
        Supply.Insufficient,
        Supply.Sufficient,
        Supply.Surplus,
        Supply.Overflowing,
        Supply.Overflowing,
    };
}
