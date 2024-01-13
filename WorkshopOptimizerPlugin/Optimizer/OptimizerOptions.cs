using System.Collections.Generic;

namespace WorkshopOptimizerPlugin.Optimizer;

public enum Strictness
{
    AllowAnyCycle               = 1 << 0,
    AllowSameCycle              = 1 << 1,
    AllowRestCycles             = 1 << 2,
    AllowEarlierCycles          = 1 << 3,
    AllowMultiCycle             = 1 << 4,
    UseMultiCycleLimit          = 1 << 5,
    AllowUnknownCycle           = 1 << 6,
    AllowMissingCommonMaterials = 1 << 7,
    AllowMissingRareMaterials   = 1 << 8,

    RelaxedDefaults = AllowSameCycle | AllowRestCycles | AllowEarlierCycles | AllowMultiCycle | UseMultiCycleLimit | AllowUnknownCycle,
}

internal class OptimizerOptions(Configuration configuration, Strictness strictness, int multiCycleLmit, IReadOnlyList<bool> restCycles)
{
    public readonly Strictness Strictness = strictness;
    public readonly int MultiCycleLimit = multiCycleLmit;
    public readonly IReadOnlyList<bool> RestCycles = restCycles;
    public readonly int ItemGenerationCutoff = configuration.ItemGenerationCutoff;
}
