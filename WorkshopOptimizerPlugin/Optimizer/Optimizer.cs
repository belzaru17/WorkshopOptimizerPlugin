using System.Collections.Generic;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Utils;

namespace WorkshopOptimizerPlugin.Optimizer;

internal class Optimizer
{
    private readonly ItemCache itemCache;
    private readonly int cycle;
    private readonly Groove groove;
    private readonly OptimizerOptions options;
    private readonly int requiredItems;

    private int ci;
    private readonly List<ItemSet> cachedCombinations;
    private double cachedCombinationsMinValue;
    private bool combinationsDone;
    private readonly bool?[] cachedCheckItems = new bool?[Constants.MaxItems];

    private readonly int[] wci;
    private readonly List<WorkshopsItemSets> cachedWorkshopCombinations;
    private bool workshopCombinationsDone;

    public Optimizer(ItemCache itemCache, int cycle, Groove groove, OptimizerOptions options)
    {
        this.itemCache = itemCache;
        this.cycle = cycle;
        this.groove = groove;
        this.options = options;

        cachedCombinations = [];
        wci = new int[Constants.MaxWorkshops];
        cachedWorkshopCombinations = [];

        for (uint i = 0; i < Constants.MaxItems; i++)
        {
            if (this.itemCache[ItemStaticData.Get(i)].When == When.Required)
            {
                requiredItems++;
            }
        }
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
                foreach (var itemset in generateCombinationsRecursive([item], item.Hours))
                {
                    if (itemset.EffectiveValue(cycle) > cachedCombinationsMinValue && itemset.RequiredItems >= requiredItems)
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
            return (null, progress);
        }

        var cutoff = options.ItemGenerationCutoff;
        if (cutoff > combinations.Count) { cutoff = combinations.Count; }

        generateAllWorkshopsRecursive(combinations, 0, 0, cutoff - 1);
        cachedWorkshopCombinations.Sort((x, y) => y.EffectiveValue.CompareTo(x.EffectiveValue));
        workshopCombinationsDone = true;
        return (cachedWorkshopCombinations, 1.0);
    }

    private void generateAllWorkshopsRecursive(List<ItemSet> combinations, int workshop, int start, int end)
    {
        if (workshop == Constants.MaxWorkshops)
        {
            var newItemSets = new ItemSet[Constants.MaxWorkshops] { combinations[wci[0]], combinations[wci[1]], combinations[wci[2]], combinations[wci[3]] };
            cachedWorkshopCombinations.Add(new WorkshopsItemSets(newItemSets, cycle, groove));
            return;
        }

        for (var i = start; i <= end; i++)
        {
            wci[workshop] = i;
            generateAllWorkshopsRecursive(combinations, workshop + 1, i, end);
        }
    }

    private List<ItemSet> generateCombinationsRecursive(List<Item> items, int hours)
    {
        if (hours == Constants.MaxHours)
        {
            return [new ItemSet([.. items])];
        }

        List<ItemSet> result = [];
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
                newItem
            };
            result.AddRange(generateCombinationsRecursive(newItems, newHours));
        }
        return result;
    }

    private bool checkItem(Item item)
    {
        if (!cachedCheckItems[item.Id].HasValue)
        {
            cachedCheckItems[item.Id] = doCheckItem(item);
        }
        return cachedCheckItems[item.Id]!.Value;
    }

    private bool doCheckItem(Item item)
    {
        static bool IsSet(Strictness a, Strictness b) => (a & b) != 0;

        if (item.MinLevel > IslandProvider.GetIslandRank()) { return false; }
        if (item.When == When.Never) { return false; }
        if (!checkMaterials(item)) { return false; }
        if (item.When is When.Always or When.Required) { return true; }
        if (IsSet(options.Strictness, Strictness.AllowAnyCycle)) { return true; }

        var patterns = item.FindPatterns(cycle);
        if (patterns.Count == 0) { return IsSet(options.Strictness, Strictness.AllowUnknownCycle); }
        if ((patterns.Count > 1) &&
            (!IsSet(options.Strictness, Strictness.AllowMultiCycle) ||
             (IsSet(options.Strictness, Strictness.UseMultiCycleLimit) &&
              (item.Value > options.MultiCycleLimit))))
        {
            return false;
        }

        foreach (var pattern in patterns)
        {
            if ((IsSet(options.Strictness, Strictness.AllowSameCycle) && (pattern.Cycle == cycle)) ||
                ((patterns.Count == 1) &&
                 ((IsSet(options.Strictness, Strictness.AllowRestCycles) && options.RestCycles[pattern.Cycle]) ||
                  (IsSet(options.Strictness, Strictness.AllowEarlierCycles) && (pattern.Cycle < cycle)))))
            {
                if ((item.When & (pattern.Strong ? Data.When.Strong : Data.When.Weak)) != 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool checkMaterials(Item item)
    {
        if ((options.Strictness & (Strictness.AllowMissingCommonMaterials | Strictness.AllowMissingRareMaterials)) == (Strictness.AllowMissingCommonMaterials | Strictness.AllowMissingRareMaterials)) return true;

        foreach (var material in item.Materials)
        {
            var option = (material.Material.Source == MaterialSource.Gatherable) ? Strictness.AllowMissingCommonMaterials : Strictness.AllowMissingRareMaterials;
            if ((options.Strictness & option) == 0 && InventoryProvider.GetItemCount(material.Material) < material.Count)
            {
                return false;
            }
        }
        return true;
    }
}
