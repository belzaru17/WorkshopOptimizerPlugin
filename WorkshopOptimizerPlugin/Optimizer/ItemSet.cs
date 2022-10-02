using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Optimizer;

internal struct ItemSet
{
    public readonly Item[] Items;

    public ItemSet(Item[] items)
    {
        Items = items;
    }

    public int Hours
    {
        get
        {
            var sum = 0;
            foreach (var item in Items)
            {
                sum += item.Hours;
            }
            return sum;
        }
    }

    public int Value
    {
        get
        {
            var q = 1;
            var sum = 0;
            foreach (var item in Items)
            {
                sum += item.Value * q;
                q = 2;
            }
            return sum;
        }
    }

    public double EffectiveValue(int cycle)
    {
        var q = 1;
        var sum = 0.0;
        foreach (var item in Items)
        {
            sum += item.EffectiveValue(cycle) * q;
            q = 2;
        }
        return sum;
    }

    public static int ItemsPerStep(int step)
    {
        return (step == 0) ? 1 : 2;
    }
}

internal struct WorkshopsItemSets
{
    public readonly ItemSet[] ItemSets;
    public readonly double Value;
    public readonly Groove EndGroove;

    public WorkshopsItemSets(ItemSet[] itemSets, double value, Groove endGroove)
    {
        this.ItemSets = itemSets;
        this.Value = value;
        this.EndGroove = endGroove;
    }
}
