using System.Collections.Generic;

namespace WorkshopOptimizerPlugin.Optimizer;

public enum Strictness
{
    AllowAnyCycle      = 1 << 0,
    AllowSameCycle     = 1 << 1,
    AllowRestCycles    = 1 << 2,
    AllowEarlierCycles = 1 << 3,
    AllowMultiCycle    = 1 << 4,
    UseMultiCycleLimit = 1 << 5,
    AllowUnknownCycle  = 1 << 6,

    StrictDefaults  = AllowSameCycle,
    RelaxedDefaults = AllowSameCycle | AllowRestCycles | AllowEarlierCycles | AllowMultiCycle | UseMultiCycleLimit | AllowUnknownCycle,
}

internal class OptimizerOptions
{
    public readonly Strictness Strictness;
    public readonly int MultiCycleLimit;
    public readonly IReadOnlyList<bool> RestCycles;
    public readonly int ItemGenerationCutoff;

    public OptimizerOptions(Configuration configuration, Strictness strictness, int multiCycleLmit, IReadOnlyList<bool> restCycles)
    {
        Strictness = strictness;
        MultiCycleLimit = multiCycleLmit;
        RestCycles = restCycles;
        ItemGenerationCutoff = configuration.ItemGenerationCutoff;
    }
}
