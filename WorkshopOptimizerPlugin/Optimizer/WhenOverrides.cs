using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Optimizer;

internal class WhenOverrides
{
    public WhenOverrides()
    {
        overrides = new When[Constants.MaxItems];
        Reset();
    }

    public When this[uint id]
    {
        get
        {
            return overrides[id];
        }
        set
        {
            overrides[id] = value;
        }
    }

    public void Reset()
    {
        for (var i = 0; i < Constants.MaxItems; i++)
        {
            overrides[i] = ItemStaticData.Get(i).When;
        }
    }

    private readonly When[] overrides;
}
