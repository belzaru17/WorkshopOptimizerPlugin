using FFXIVClientStructs.FFXIV.Client.Game;

namespace WorkshopOptimizerPlugin.Data;

internal class SupplyDemandPattern
{
    public readonly int Cycle;
    public readonly bool Strong;
    public readonly Supply[] SupplyPattern;
    public readonly Demand[] DemandPattern;

    SupplyDemandPattern(int cycle, bool strong, Supply[] supplyPattern, Demand[] demandPattern)
    {
        Cycle = cycle;
        Strong = strong;
        SupplyPattern = supplyPattern;
        DemandPattern = demandPattern;
    }

    public string Name
    {
        get
        {
            return "C" + (Cycle + 1) + (Strong ? "S" : "W");
        }
    }

    public static readonly SupplyDemandPattern[] PatternsTable = {
        new SupplyDemandPattern(1, false,
            new Supply[7]{
                Supply.Insufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            }, new Demand[7]{
                Demand.Increasing, Demand.Increasing, Demand.Plummeting, Demand.None, Demand.None, Demand.None, Demand.None,
            }),
        new SupplyDemandPattern(1, true,
            new Supply[7]{
                Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            }, new Demand[7]{
                Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting, Demand.None, Demand.None, Demand.None, Demand.None,
            }),
        new SupplyDemandPattern(2, false,
            new Supply[7]{
                 Supply.Sufficient, Supply.Insufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            }, new Demand[7]{
                 Demand.Any, Demand.Increasing, Demand.Increasing, Demand.Plummeting, Demand.None, Demand.None, Demand.None,
            }),
        new SupplyDemandPattern(2, true,
            new Supply[7]{
                 Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            }, new Demand[7]{
                 Demand.Any, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting, Demand.None, Demand.None, Demand.None,
            }),
        new SupplyDemandPattern(3, false,
            new Supply[7]{
                 Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            }, new Demand[7]{
                 Demand.Any, Demand.None, Demand.Increasing, Demand.Increasing, Demand.Plummeting, Demand.None, Demand.None,
            }),
        new SupplyDemandPattern(3, true,
            new Supply[7]{
                 Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            }, new Demand[7]{
                 Demand.Any, Demand.None, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting, Demand.None, Demand.None,
            }),
        new SupplyDemandPattern(4, false,
            new Supply[7]{
                 Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient,
            }, new Demand[7]{
                 Demand.Any, Demand.None, Demand.None, Demand.Increasing, Demand.Increasing, Demand.Plummeting, Demand.None,
            }),
        new SupplyDemandPattern(4, true,
            new Supply[7]{
                 Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient, Supply.Sufficient,
            }, new Demand[7]{
                 Demand.Any, Demand.None, Demand.None, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting, Demand.None,
            }),
        new SupplyDemandPattern(5, false,
            new Supply[7]{
                 Supply.Sufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Insufficient, Supply.Sufficient,
            }, new Demand[7]{
                 Demand.Any, Demand.None, Demand.Decreasing, Demand.Increasing, Demand.Increasing, Demand.Increasing, Demand.Plummeting,
            }),
        new SupplyDemandPattern(5, true,
            new Supply[7]{
                 Supply.Sufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient,
            }, new Demand[7]{
                 Demand.Any, Demand.None, Demand.Plummeting, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting,
            }),
        new SupplyDemandPattern(6, false,
            new Supply[7]{
                 Supply.Sufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Insufficient,
            }, new Demand[7]{
                 Demand.Any, Demand.None, Demand.Plummeting, Demand.Increasing, Demand.Increasing, Demand.Increasing, Demand.Increasing,
            }),
        new SupplyDemandPattern(6, true,
            new Supply[7]{
                 Supply.Sufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent,
            }, new Demand[7]{
                 Demand.Any, Demand.None, Demand.Plummeting, Demand.None, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Skyrocketing,
            }),
    };
}
