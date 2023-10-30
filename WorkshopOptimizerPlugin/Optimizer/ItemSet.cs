using System.Collections.Generic;
using System.Linq;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Optimizer;

internal readonly struct ItemSet
{
    public readonly Item[] Items;
    public readonly int RequiredItems;

    public ItemSet(Item[] items)
    {
        Items = items;
        var requiredUniqueItems = new Dictionary<uint, int>();
        for (var i = 0; i < Items.Length; i++)
        {
            var item = Items[i];
            if (item.When == When.Required)
            {
                requiredUniqueItems.TryAdd(item.Id, 0);
                requiredUniqueItems[item.Id] += ItemsPerStep(i);
            }
        }
        RequiredItems = requiredUniqueItems.Count(kv => kv.Value >= 2);
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

internal readonly struct WorkshopsItemSets
{
    public readonly ItemSet[] ItemSets;
    public readonly double EffectiveValue;
    public readonly Groove EndGroove;

    public WorkshopsItemSets(ItemSet[] itemSets, int cycle, Groove groove)
    {
        ItemSets = itemSets;

        EffectiveValue = 0.0;
        var producedItems = new int[Constants.MaxItems];
        var start = new int[Constants.MaxWorkshops];
        var steps = new int[Constants.MaxWorkshops];
        for (var h = 0; h <= Constants.MaxHours; h++)
        {
            var hourGroove = groove;
            var hourProducedItems = new int[Constants.MaxItems];
            producedItems.CopyTo(hourProducedItems, 0);
            for (var w = 0; w < Constants.MaxWorkshops; w++)
            {
                if (steps[w] >= ItemSets[w].Items.Length) { continue; }
                var item = ItemSets[w].Items[steps[w]];

                if (h < start[w] + item.Hours) { continue; }

                var q = ItemSet.ItemsPerStep(steps[w]);
                EffectiveValue += item.EffectiveValue(cycle, hourProducedItems[item.Id]) * q * hourGroove.Multiplier();

                producedItems[item.Id] += q;
                groove = groove.Inc(steps[w]);
                steps[w]++;
                start[w] = h;
            }
        }
        this.EndGroove = groove;
    }
}
