using System;
using System.Collections.Generic;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin.Optimizer;

internal class Optimizer
{
    private readonly ItemCache itemCache;
    private readonly int cycle;
    private readonly Groove groove;
    private readonly OptimizerOptions options;

    private int ci;
    private readonly List<ItemSet> cachedCombinations;
    private double cachedCombinationsMinValue;
    private bool combinationsDone;

    private readonly int[] wci;
    private readonly List<WorkshopsItemSets> cachedWorkshopCombinations;
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
                    if (itemset.EffectiveValue(cycle) > cachedCombinationsMinValue)
                    {
                        cachedCombinations.Add(itemset);
                    }
                }
            }
        }

        cachedCombinations.Sort((x, y) => y.EffectiveValue(cycle).CompareTo(x.EffectiveValue(cycle)));
        if (cachedCombinations.Count > Constants.MaxComputeItems)
        {
            cachedCombinations.RemoveRange(Constants.MaxComputeItems, cachedCombinations.Count - Constants.MaxComputeItems);
            cachedCombinationsMinValue = cachedCombinations[^1].EffectiveValue(cycle);
        }

        if (ci < Constants.MaxItems)
        {
            return (null, (double)ci / Constants.MaxItems);
        }

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
            return (null, progress/2);
        }

        var cutoff = options.ItemGenerationCutoff;
        if (cutoff > combinations.Count) { cutoff = combinations.Count; }

        var done = false;
        for (var remain = Constants.MaxComputeItems; !done && remain > 0; remain--) {
            var newItemSets = new ItemSet[Constants.MaxWorkshops] { combinations[wci[0]], combinations[wci[1]], combinations[wci[2]] };
            cachedWorkshopCombinations.Add(new WorkshopsItemSets(newItemSets, cycle, groove));
            for (var i = wci.Length-1; i >= 0 && ++wci[i] >= cutoff; i--)
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
        return (null, 0.5 + (((((wci[0] * cutoff) + wci[1]) * cutoff) + wci[2]) / Math.Pow(cutoff, 3) / 2));
    }

    private List<ItemSet> generateCombinationsRecursive(List<Item> items, int hours)
    {
        if (hours == Constants.MaxHours)
        {
            return new() { new ItemSet(items.ToArray()) };
        }

        List<ItemSet> result = new();
        var lastItem = items[^1];
        foreach (var nextItem in CategoryMap.GetEfficientItems(lastItem.Categories))
        {
            if (lastItem.Id == nextItem.Id) { continue; }

            var newHours = hours + nextItem.Hours;
            if (newHours > Constants.MaxHours) { continue; }

            var newItem = itemCache[nextItem];
            if (!checkItem(newItem)) { continue; }

            List<Item> newItems = new(items)
            {
                itemCache[nextItem]
            };
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
