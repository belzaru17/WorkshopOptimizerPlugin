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

    private int ci;
    private List<ItemSet> cachedCombinations;
    private bool combinationsDone;

    private int[] wci;
    private List<WorkshopsItemSets> cachedWorkshopCombinations;
    private bool workshopCombinationsDone;

    public Optimizer(ItemCache itemCache, int cycle, Groove groove, OptimizerOptions options)
    {
        this.itemCache = itemCache;
        this.cycle = cycle;
        this.groove = groove;
        this.options = options;

        cachedCombinations = new();
        wci = new int[Constants.MaxWorkshops];
        cachedWorkshopCombinations = new();
    }

    public (List<ItemSet>?, double) GenerateCombinations()
    {
        if (combinationsDone)
        {
            return (cachedCombinations, 1.0);
        }

        var staticItem = ItemStaticData.Get(ci++);
        if (staticItem.IsValid())
        {
            var item = itemCache[staticItem];
            if (checkItem(item))
            {
                foreach (var itemset in generateCombinationsRecursive(new List<Item> { item }, item.Hours))
                {
                    cachedCombinations.Add(itemset);
                }
            }
        }

        if (ci < Constants.MaxItems)
        {
            return (null, (double)ci / Constants.MaxItems);
        }

        cachedCombinations.Sort((x, y) => y.EffectiveValue(cycle).CompareTo(x.EffectiveValue(cycle)));
        combinationsDone = true;
        return (cachedCombinations, 1.0);
    }

    public (List<WorkshopsItemSets>?, double) GenerateAllWorkshops()
    {
        if (workshopCombinationsDone)
        {
            return (cachedWorkshopCombinations, 1.0);
        }

        var (combinations, progress) = GenerateCombinations();
        if (combinations == null)
        {
            return (null, progress);
        }

        var cutoff = options.ItemGenerationCutoff;
        if (cutoff > combinations.Count) { cutoff = combinations.Count; }

        bool done = false;
        for (int remain = Constants.MaxComputeItems; !done && remain > 0; remain--) {
            ItemSet[] newItemSets = new ItemSet[Constants.MaxWorkshops] { combinations[wci[0]], combinations[wci[1]], combinations[wci[2]] };
            cachedWorkshopCombinations.Add(new WorkshopsItemSets(newItemSets, cycle, groove));
            for (int i = wci.Length-1; i >= 0 && ++wci[i] >= cutoff; i--)
            {
                wci[i] = 0;
                if (i == 0)
                {
                    done = true;
                    break;
                }
            }
        }
        if (done)
        {
            cachedWorkshopCombinations.Sort((x, y) => y.EffectiveValue.CompareTo(x.EffectiveValue));
            workshopCombinationsDone = true;
            return (cachedWorkshopCombinations, 1.0);
        }
        return (null, (((wci[0] * cutoff + wci[1]) * cutoff) + wci[2]) / Math.Pow(cutoff, 3));
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
