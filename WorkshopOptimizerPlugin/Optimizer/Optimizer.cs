using System.Collections.Generic;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Optimizer;

internal class Optimizer
{
    private readonly ItemCache itemCache;
    private readonly int cycle;
    private readonly OptimizerOptions options;

    public Optimizer(ItemCache itemCache, int cycle, OptimizerOptions options)
    {
        this.itemCache = itemCache;
        this.cycle = cycle;
        this.options = options;
    }

    public List<ItemSet> GenerateCombinations()
    {
        var result = new List<ItemSet>();
        for (uint i = 0; i < Constants.MaxItems; i++)
        {
            var staticItem = ItemStaticData.Get(i);
            if (!staticItem.IsValid()) { continue; }

            var item = itemCache[staticItem];
            if (!checkItem(item)) { continue; }

            foreach (var itemset in generateCombinationsRecursive(new List<Item> { item }, item.Hours))
            {
                result.Add(itemset);
            }
        }
        result.Sort((x,y) => y.EffectiveValue(cycle).CompareTo(x.EffectiveValue(cycle)));
        return result;
    }

    public List<WorkshopsItemSets> GenerateAllWorkshops(Groove groove)
    {
        var combs = GenerateCombinations();
        var cutoff = options.ItemGenerationCutoff;

        if (cutoff > combs.Count) { cutoff = combs.Count; }

        List<WorkshopsItemSets> result = new();
        for (int i = 0; i < cutoff; i++)
        {
            for (int j = 0; j < cutoff; j++)
            {
                for (int k = 0; k < cutoff; k++)
                {
                    ItemSet[] newItemSets = new ItemSet[Constants.MaxWorkshops]{ combs[i], combs[j], combs[k] };
                    var (value, endGroove) = recalculateItemSetValue(newItemSets, groove);
                    result.Add(new WorkshopsItemSets(newItemSets, value, endGroove));
                }
            }
        }
        result.Sort((x, y) => y.Value.CompareTo(x.Value));
        return result;
    }

    private List<ItemSet> generateCombinationsRecursive(List<Item> items, int hours)
    {
        if (hours == Constants.MaxHours)
        {
            return new() { new ItemSet(items.ToArray()) };
        }

        List<ItemSet> result = new();
        var lastItem = items[items.Count - 1];
        foreach (var nextItem in CategoryMap.GetEfficientItems(lastItem.Categories))
        {
            if (lastItem.Id == nextItem.Id) { continue; }

            int newHours = hours + nextItem.Hours;
            if (newHours > Constants.MaxHours) { continue; }

            var newItem = itemCache[nextItem];
            if (!checkItem(newItem)) { continue; }

            List<Item> newItems = new(items);
            newItems.Add(itemCache[nextItem]);
            foreach (var itemset in generateCombinationsRecursive(newItems, newHours))
            {
                result.Add(itemset);
            }
        }
        return result;
    }

    private bool checkItem(Item item)
    {
        return item.CheckCycles(cycle, options.RestCycles, options.Strictness);
    }

    private (double, Groove) recalculateItemSetValue(ItemSet[] itemSets, Groove groove)
    {
        var total_value = 0.0;
        var producedItems = new int[Constants.MaxItems];
        var start = new int[Constants.MaxWorkshops];
        var steps = new int[Constants.MaxWorkshops];
        for (int h = 0; h <= Constants.MaxHours; h++)
        {
            for (int w = 0; w < Constants.MaxWorkshops; w++)
            {
                if (steps[w] >= itemSets[w].Items.Length) { continue;  }
                var item = itemSets[w].Items[steps[w]];

                if (h < start[w] + item.Hours) { continue; }

                var q = ItemSet.ItemsPerStep(steps[w]);
                total_value += item.EffectiveValue(cycle, producedItems[item.Id]) * q * groove.Multiplier();

                producedItems[item.Id] += q;
                groove = groove.Inc(steps[w]);
                steps[w]++;
                start[w] = h;
            }
        }
        return (total_value, groove);
    }
}
