using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    [MemberNotNull(nameof(combinations))]
    public List<ItemSet> GenerateCombinations()
    {
        if (combinations == null)
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
            result.Sort((x, y) => y.EffectiveValue(cycle).CompareTo(x.EffectiveValue(cycle)));
            combinations = result;
        }
        return combinations;
    }

    public (List<WorkshopsItemSets>?, double) GenerateAllWorkshops()
    {
        if (cachedDone)
        {
            return (cachedResult, 1.0);
        }

        if (combinations == null)
        {
            GenerateCombinations();
        }

        var cutoff = options.ItemGenerationCutoff;
        if (cutoff > combinations.Count) { cutoff = combinations.Count; }

        bool done = false;
        for (int remain = Constants.MaxComputeItems; !done && remain > 0; remain--) {
            ItemSet[] newItemSets = new ItemSet[Constants.MaxWorkshops] { combinations[ci[0]], combinations[ci[1]], combinations[ci[2]] };
            cachedResult.Add(new WorkshopsItemSets(newItemSets, cycle, groove));
            for (int i = ci.Length-1; i >= 0 && ++ci[i] >= cutoff; i--)
            {
                ci[i] = 0;
                if (i == 0)
                {
                    done = true;
                    break;
                }
            }
        }
        if (done)
        {
            cachedResult.Sort((x, y) => y.EffectiveValue.CompareTo(x.EffectiveValue));
            cachedDone = true;
            return (cachedResult, 1.0);
        }
        return (null, (((ci[0] * cutoff + ci[1]) * cutoff) + ci[2]) / Math.Pow(cutoff, 3));
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
}
