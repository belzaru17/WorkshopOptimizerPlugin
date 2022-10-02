using System;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.Game;
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
    public static double Multiplier(Supply s)
    {
        return Multipliers[(int)s];
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

    public static Tuple<Supply, Supply> Adjust(Supply s, int produced)
    {
        for (; produced >= SupplyAdjustmentBucket; produced -= SupplyAdjustmentBucket) {
            s = NextSupply[(int)s];
    
        }
        if (produced == 0) {
            return new Tuple<Supply, Supply>(s, s);
        }
        return new Tuple<Supply, Supply>(s, NextSupply[(int)s]);
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
