using Lumina.Excel.GeneratedSheets;

namespace WorkshopOptimizerPlugin.Optimizer;

internal struct Strictness
{
    public bool AllowAnyCycle;
    public bool AllowSameCycle;
    public bool AllowRestCycle;
    public bool AllowEarlierCycle;
    public bool AllowMultiCycle;
    public bool AllowUnknownCycle;

    public static Strictness StrictDefaults()
    {
        var ret = new Strictness();
        ret.AllowSameCycle = true;
        return ret;
    }

    public static Strictness RelaxedDefaults()
    {
        var ret = new Strictness();
        ret.AllowSameCycle = ret.AllowRestCycle = ret.AllowEarlierCycle = ret.AllowMultiCycle = ret.AllowUnknownCycle = true;
        return ret;
    }
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
