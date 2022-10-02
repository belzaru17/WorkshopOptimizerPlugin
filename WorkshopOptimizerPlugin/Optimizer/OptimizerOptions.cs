namespace WorkshopOptimizerPlugin.Optimizer;

internal enum Strictness
{
    AllowExactCycle   = 1 << 0,
    AllowRestCycle    = 1 << 1,
    AllowEarlierCycle = 1 << 2,
    AllowAnyCycle     = 1 << 3,
}


internal class OptimizerOptions
{
    public Strictness Strictness;
    public bool[] RestCycles;
    public int ItemGenerationCutoff;

    public OptimizerOptions(Configuration configuration, Strictness strictness = Strictness.AllowExactCycle)
    {
        Strictness = strictness;
        RestCycles = new bool[7];
        RestCycles[configuration.DefaultRestCycle1] = true;
        RestCycles[configuration.DefaultRestCycle2] = true;
        ItemGenerationCutoff = configuration.ItemGenerationCutoff;
    }
}
