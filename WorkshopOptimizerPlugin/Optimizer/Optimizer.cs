using System.Collections.Generic;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Optimizer;

internal class Optimizer
{
    private readonly ItemCache itemCache;
    private readonly int cycle;
    private readonly Groove groove;
    private readonly OptimizerOptions options;

    private List<ItemSet>? combinations;
    private int[] ci;
    private List<WorkshopsItemSets> cachedResult;
    private bool cachedDone;

    public Optimizer(ItemCache itemCache, int cycle, Groove groove, OptimizerOptions options)
    {
        this.itemCache = itemCache;
        this.cycle = cycle;
        this.groove = groove;
        this.options = options;

        ci = new int[Constants.MaxWorkshops];
        cachedResult = new();
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

    public List<WorkshopsItemSets>? GenerateAllWorkshops()
    {
        if (cachedDone)
        {
            return cachedResult;
        }

        if (combinations == null)
        {
            combinations = GenerateCombinations();
        }

        var cutoff = options.ItemGenerationCutoff;
        if (cutoff > combinations.Count) { cutoff = combinations.Count; }

        int remain = Constants.MaxComputeItems;
        for (; remain > 0 && ci[0] < cutoff; ci[0]++, ci[1]=0)
        {
            for (; remain > 0 && ci[1] < cutoff; ci[1]++, ci[2]=0)
            {
                for (; remain > 0 && ci[2] < cutoff; ci[2]++, remain--)
                {
                    ItemSet[] newItemSets = new ItemSet[Constants.MaxWorkshops]{ combinations[ci[0]], combinations[ci[1]], combinations[ci[2]] };
                    var (value, endGroove) = recalculateItemSetValue(newItemSets, groove);
                    cachedResult.Add(new WorkshopsItemSets(newItemSets, value, endGroove));
                }
            }
        }
        if (remain > 0)
        {
            cachedResult.Sort((x, y) => y.Value.CompareTo(x.Value));
            cachedDone = true;
            return cachedResult;
        }
        return null;
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
