namespace WorkshopOptimizerPlugin.Data;

internal class SupplyDemandPattern(int cycle, bool strong, Supply[] supplyPattern, Demand[] demandPattern)
{
    public readonly int Cycle = cycle;
    public readonly bool Strong = strong;
    public readonly Supply[] SupplyPattern = supplyPattern;
    public readonly Demand[] DemandPattern = demandPattern;

    public string Name => "C" + (Cycle + 1) + (Strong ? "S" : "W");

    public static readonly SupplyDemandPattern[] PatternsTable = [
        new(1, false,
            [
                Supply.Insufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            ], [
                Demand.Increasing, Demand.Increasing, Demand.Plummeting, Demand.None, Demand.None, Demand.None, Demand.None,
            ]),
        new(1, true,
            [
                Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            ], [
                Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting, Demand.None, Demand.None, Demand.None, Demand.None,
            ]),
        new(2, false,
            [
                 Supply.Sufficient, Supply.Insufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            ], [
                 Demand.Any, Demand.Increasing, Demand.Increasing, Demand.Plummeting, Demand.None, Demand.None, Demand.None,
            ]),
        new(2, true,
            [
                 Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            ], [
                 Demand.Any, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting, Demand.None, Demand.None, Demand.None,
            ]),
        new(3, false,
            [
                 Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            ], [
                 Demand.Any, Demand.None, Demand.Increasing, Demand.Increasing, Demand.Plummeting, Demand.None, Demand.None,
            ]),
        new(3, true,
            [
                 Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient,
            ], [
                 Demand.Any, Demand.None, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting, Demand.None, Demand.None,
            ]),
        new(4, false,
            [
                 Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient,
            ], [
                 Demand.Any, Demand.None, Demand.None, Demand.Increasing, Demand.Increasing, Demand.Plummeting, Demand.None,
            ]),
        new(4, true,
            [
                 Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient, Supply.Sufficient,
            ], [
                 Demand.Any, Demand.None, Demand.None, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting, Demand.None,
            ]),
        new(5, false,
            [
                 Supply.Sufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Insufficient, Supply.Sufficient,
            ], [
                 Demand.Any, Demand.Increasing, Demand.Decreasing, Demand.Increasing, Demand.Increasing, Demand.Increasing, Demand.Plummeting,
            ]),
        new(5, true,
            [
                 Supply.Sufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent, Supply.Sufficient,
            ], [
                 Demand.Any, Demand.Increasing, Demand.Plummeting, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Plummeting,
            ]),
        new(6, false,
            [
                 Supply.Sufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Insufficient,
            ], [
                 Demand.Any, Demand.Increasing, Demand.Plummeting, Demand.Increasing, Demand.Increasing, Demand.Increasing, Demand.Increasing,
            ]),
        new(6, true,
            [
                 Supply.Sufficient, Supply.Insufficient, Supply.Sufficient, Supply.Sufficient, Supply.Sufficient, Supply.Insufficient, Supply.Nonexistent,
            ], [
                 Demand.Any, Demand.Increasing, Demand.Plummeting, Demand.None, Demand.Skyrocketing, Demand.Skyrocketing, Demand.Skyrocketing,
            ]),
    ];
}
