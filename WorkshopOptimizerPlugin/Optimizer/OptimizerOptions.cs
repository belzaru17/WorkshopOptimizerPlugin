namespace WorkshopOptimizerPlugin.Optimizer;

internal enum Strictness
{
    AllowAnyCycle     = 1 << 0,
    AllowExactCycle   = 1 << 1,
    AllowRestCycle    = 1 << 2,
    AllowEarlierCycle = 1 << 3,
    AllowMultiCycle   = 1 << 4,
    AllowUnknownCycle = 1 << 5,

    StrictDefaults    = AllowExactCycle,
    RelaxedDefaults   = AllowExactCycle | AllowRestCycle | AllowEarlierCycle | AllowMultiCycle | AllowUnknownCycle,
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
