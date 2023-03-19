using System;
using System.Collections.Generic;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Persistence;

namespace WorkshopOptimizerPlugin.Optimizer;

internal class Item
{
    // Static Data
    public uint Id => staticData.Id;
    public string Name => staticData.Name;
    public int Hours => staticData.Hours;
    public int Value => staticData.Value;
    public When When => whenOverrides[staticData.Id];
    public Categories[] Categories => staticData.Categories;
    public Materials[] Materials => staticData.Materials;

    // Dynamic Data
    public Popularity Popularity => dynamicData.Popularity;
    public Popularity NextPopularity => dynamicData.NextPopularity;
    public Supply[] Supply => dynamicData.Supply;
    public Demand[] Demand => dynamicData.Demand;

    public Item(ItemStaticData staticData, ItemDynamicData dynamicData, ProducedItemsAdaptor producedItems, WhenOverrides whenOverrides)
    {
        this.staticData = staticData;
        this.dynamicData = dynamicData;
        this.producedItems = producedItems;
        this.whenOverrides = whenOverrides;
    }

    public int AccumulatedProduced(int cycle)
    {
        return producedItems.ItemsProducedUntil(cycle, this.Id);
    }

    public List<SupplyDemandPattern> FindPatterns(int cycle)
    {
        if (cycle > 4)
        {
            cycle = 4;
        }

        List<SupplyDemandPattern> ret = new List<SupplyDemandPattern>();

        foreach (SupplyDemandPattern pattern in SupplyDemandPattern.PatternsTable)
        {
            var mismatch = false;
            for (var i = 0; !mismatch && i < cycle; i++)
            {
                mismatch = (pattern.SupplyPattern[i] != Supply[i]) ||
                           ((pattern.DemandPattern[i] != Data.Demand.Any) && (pattern.DemandPattern[i] != Demand[i]));
            }
            if (!mismatch)
            {
                ret.Add(pattern);
            }
        }
        return ret;
    }

    public (SupplyDemandPattern?, bool) FindPattern(int cycle)
    {
        List<SupplyDemandPattern> patterns = FindPatterns(cycle);
        if (patterns.Count != 1 || patterns[0].Cycle != cycle)
        {
            return (null, patterns.Count > 0);
        }
        
        return (patterns[0], true);
    }

    public double EffectiveValue(int cycle, int add = 0)
    {
        var produced = AccumulatedProduced(cycle) + add;

        var sup_mult = 0.0;
        var n = 0;
        if (Supply[cycle] != Data.Supply.Unknown)
        {
            sup_mult += SupplyUtils.Multiplier(Supply[cycle], produced);
            n++;
        }
        else
        {
            var patterns = FindPatterns(cycle);
            if (patterns.Count == 0)
            {
                sup_mult += SupplyUtils.Multiplier(Data.Supply.Sufficient, produced);
                n++;
            }
            else
            {

                foreach (var pattern in patterns)
                {
                    sup_mult += SupplyUtils.Multiplier(pattern.SupplyPattern[cycle], produced);
                    n++;
                }
            }
        }

        return ((double)Value) * PopularityUtils.Multiplier(Popularity) * (sup_mult / n);
    }

    public bool CheckCycles(int cycle, bool[] restCycles, Strictness strictness)
    {
        Func<Strictness, Strictness, bool> IsSet = (a, b) => (a & b) != 0;

        if (When == Data.When.Never) { return false; }
        if (When == Data.When.Always) { return true; }
        if (IsSet(strictness, Strictness.AllowAnyCycle)) { return true; }

        var patterns = FindPatterns(cycle);
        if (patterns.Count == 0) { return IsSet(strictness, Strictness.AllowUnknownCycle); }
        if (patterns.Count > 1 && !IsSet(strictness, Strictness.AllowMultiCycle)) { return false; }

        foreach (var pattern in patterns)
        {
            if ((IsSet(strictness, Strictness.AllowSameCycle) && (pattern.Cycle == cycle)) ||
                (IsSet(strictness, Strictness.AllowRestCycle) && restCycles[pattern.Cycle]) ||
                (IsSet(strictness, Strictness.AllowEarlierCycle) && (pattern.Cycle < cycle))) 
            {
                if ((When & (pattern.Strong? Data.When.Strong : Data.When.Weak)) != 0) {
                    return true;
                }
            }
        }

        return false;
    }

    private readonly ItemStaticData staticData;
    private readonly ItemDynamicData dynamicData;
    private readonly ProducedItemsAdaptor producedItems;
    private readonly WhenOverrides whenOverrides;
}
