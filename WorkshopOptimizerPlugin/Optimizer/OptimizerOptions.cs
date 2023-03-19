namespace WorkshopOptimizerPlugin.Optimizer;

public enum Strictness
{
    AllowAnyCycle     = 1 << 0,
    AllowSameCycle    = 1 << 1,
    AllowRestCycles    = 1 << 2,
    AllowEarlierCycles = 1 << 3,
    AllowMultiCycle   = 1 << 4,
    AllowUnknownCycle = 1 << 5,

    StrictDefaults  = AllowSameCycle,
    RelaxedDefaults = AllowSameCycle | AllowRestCycles | AllowEarlierCycles | AllowMultiCycle | AllowUnknownCycle,
}

internal class OptimizerOptions
{
    public Strictness Strictness;
    public readonly bool[] RestCycles;
    public int ItemGenerationCutoff;

    public OptimizerOptions(Configuration configuration, Strictness strictness, bool[] restCycles)
    {
        Strictness = strictness;
        RestCycles = restCycles;
        ItemGenerationCutoff = configuration.ItemGenerationCutoff;
    }
}
