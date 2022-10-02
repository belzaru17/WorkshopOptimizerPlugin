using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Windows.Utils;

internal class CommonInterfaceElements
{
    // tweakable options with config defaults
    public int mRestCycle1;
    public int mRestCycle2;
    public int mTop;

    // tweakable options
    public int mCycle;
    public bool mStrictCycles = true;

    public CommonInterfaceElements(Configuration configuration)
    {
        mRestCycle1 = configuration.DefaultRestCycle1;
        mRestCycle2 = configuration.DefaultRestCycle2;
        mTop = configuration.DefaultTopValues;
        mCycle = SeasonUtils.GetCycle() + 1;
    }


}
