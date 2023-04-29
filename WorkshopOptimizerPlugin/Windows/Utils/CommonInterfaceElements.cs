using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System.Collections.Generic;
using System.Numerics;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Utils;

namespace WorkshopOptimizerPlugin.Windows.Utils;

internal class CommonInterfaceElements
{
    private readonly Configuration configuration;
    private readonly UIDataSource uiDataSource;
    private readonly Icons icons;
    private int mSeason = 0;
    private int mCycle;
    private int mTop;
    private Strictness mStrictness = Strictness.RelaxedDefaults;
    private int mMultiCycleLimit;
    private int mNewStrictness;
    private int mNewMultiCycleLimit;

    public int Season => mSeason;
    public int Cycle => UIUtils.FixValue(ref mCycle, 1, 7) - 1;
    public Strictness Strictness => mStrictness;
    public int MultiCycleLimit => mMultiCycleLimit;
    public int Top => UIUtils.FixValue(ref mTop, 1, Constants.MaxTopItems);
    public IReadOnlyList<bool> RestCycles => IsCurrentSeason() ? uiDataSource.DataSource.CurrentRestCycles : uiDataSource.DataSource.PreviousRestCycles;

    public CommonInterfaceElements(Icons icons, Configuration configuration, UIDataSource uiDataSource)
    {
        this.configuration = configuration;
        this.uiDataSource = uiDataSource;
        this.icons = icons;
        mTop = configuration.DefaultTopValues;
        mMultiCycleLimit = configuration.DefaultMultiCycleLimit;
        mCycle = SeasonUtils.GetCycle() + 1;
    }

    public bool IsCurrentSeason()
    {
        return Season == Constants.CurrentSeason;
    }

    public bool IsPreviousSeason()
    {
        return Season == Constants.PreviousSeason;
    }

    public Groove GetStartGroove()
    {
        if (Cycle == 0) return new Groove();

        var producedItems = IsCurrentSeason() ? uiDataSource.DataSource.CurrentProducedItems : uiDataSource.DataSource.PreviousProducedItems;
        return producedItems.GrooveAtEndOfCycle[Cycle - 1];
    }

    public Groove GetEndGroove()
    {
        var producedItems = IsCurrentSeason() ? uiDataSource.DataSource.CurrentProducedItems : uiDataSource.DataSource.PreviousProducedItems;
        return producedItems.GrooveAtEndOfCycle[Cycle];
    }

    public OptimizerOptions CreateOptimizerOptions()
    {
        return new OptimizerOptions(configuration, Strictness, MultiCycleLimit, RestCycles);
    }

    public void DrawBasicControls()
    {
        ImGui.SetNextItemWidth(100);
        ImGui.Combo("Season", ref mSeason, new string[] { "Current", "Previous" }, 2);
        ImGui.SameLine();
        ImGui.SetNextItemWidth(100);
        ImGui.InputInt("Cycle", ref mCycle);
        ImGui.SameLine();
        ImGui.Text(string.Format("Groove: {0} -> {1}", GetStartGroove(), GetEndGroove())); ;
    }

    public void DrawFilteringControls()
    {
        ImGui.SetNextItemWidth(100);
        ImGui.InputInt("Top-N", ref mTop, 5);
        ImGui.SameLine();
        if (ImGui.BeginPopupModal("Optimizer Settings"))
        {
            ImGui.SetWindowSize(new Vector2(350, 280));
            ImGui.CheckboxFlags("Allow items on any cycle", ref mNewStrictness, (int)Strictness.AllowAnyCycle);
            var any_cycle = (mNewStrictness & (int)Strictness.AllowAnyCycle) != 0;
            if (any_cycle) ImGui.BeginDisabled();
            ImGui.CheckboxFlags("Allow items that peak on this cycle", ref mNewStrictness, (int)Strictness.AllowSameCycle);
            ImGui.CheckboxFlags("Allow items that peak on rest cycles", ref mNewStrictness, (int)Strictness.AllowRestCycles);
            ImGui.CheckboxFlags("Allow items that peaked on earlier cycles", ref mNewStrictness, (int)Strictness.AllowEarlierCycles);
            ImGui.CheckboxFlags("Allow items that may peak on other cycles", ref mNewStrictness, (int)Strictness.AllowMultiCycle);
            var multi_disabled = (mNewStrictness & (int)Strictness.AllowMultiCycle) == 0;
            if (multi_disabled) ImGui.BeginDisabled();
            ImGui.Spacing(); ImGui.SameLine();
            ImGui.CheckboxFlags("Maximum value: ", ref mNewStrictness, (int)Strictness.UseMultiCycleLimit);
            ImGui.SameLine();
            var limit_disabled = (mNewStrictness & (int)Strictness.UseMultiCycleLimit) == 0;
            if (limit_disabled) ImGui.BeginDisabled();
            ImGui.SetNextItemWidth(100);
            ImGui.InputInt("##Limit", ref mNewMultiCycleLimit);
            if (limit_disabled) ImGui.EndDisabled();
            if (multi_disabled) ImGui.EndDisabled();
            ImGui.CheckboxFlags("Allow items that we don't know their peak cycle", ref mNewStrictness, (int)Strictness.AllowUnknownCycle);
            if (any_cycle) ImGui.EndDisabled();
            ImGui.Spacing();
            if (ImGui.Button("Cancel"))
            {
                ImGui.CloseCurrentPopup();
            }
            ImGui.SameLine();
            if (ImGui.Button("Defaults"))
            {
                if (mStrictness != Strictness.RelaxedDefaults || mMultiCycleLimit != configuration.DefaultMultiCycleLimit)
                {
                    mStrictness = Strictness.RelaxedDefaults;
                    mMultiCycleLimit = configuration.DefaultMultiCycleLimit;
                    uiDataSource.OptimizationParameterChanged();
                }
                ImGui.CloseCurrentPopup();
            }
            ImGui.SameLine();
            if (ImGui.Button("Apply"))
            {
                if (mStrictness != (Strictness)mNewStrictness || mMultiCycleLimit != mNewMultiCycleLimit)
                {
                    mStrictness = (Strictness)mNewStrictness;
                    mMultiCycleLimit = mNewMultiCycleLimit;
                    uiDataSource.OptimizationParameterChanged();
                }
                ImGui.CloseCurrentPopup();
            }
            ImGui.EndPopup();
        }
        if (UIUtils.ImageButton(icons.OptimizerSettings, "Optimizer Settings"))
        {
            mNewStrictness = (int)Strictness;
            mNewMultiCycleLimit = MultiCycleLimit;
            ImGui.OpenPopup("Optimizer Settings");
        }
    }

    public void DrawRestCycleCheckbox(int cycle)
    {
        var rest_cycles = 0;
        for (var i = 0; i < Constants.MaxCycles; i++)
        {
            if (uiDataSource.DataSource.CurrentRestCycles[i]) { rest_cycles++; }
        }
        var disabled = !IsCurrentSeason() || (rest_cycles >= 2 && !uiDataSource.DataSource.CurrentRestCycles[cycle]);
        if(disabled)
        {
            ImGui.BeginDisabled();
        }
        var mRestCycle = RestCycles[cycle];
        if (ImGui.Checkbox("Rest Cycle", ref mRestCycle))
        {
            if (uiDataSource.DataSource.CurrentRestCycles[cycle])
            {
                var producedItems = uiDataSource.DataSource.CurrentProducedItems;
                for (var w = 0; w < Constants.MaxWorkshops; w++)
                {
                    for (var s = 0; s < Constants.MaxSteps; s++)
                    {
                        producedItems[cycle, w, s] = -1;
                    }
                }
            }
            uiDataSource.DataSource.CurrentRestCycles[cycle] = mRestCycle;
            uiDataSource.DataChanged();
        }
        if (disabled)
        {
            ImGui.EndDisabled();
        }
    }

    public void NextCycle()
    {
        mCycle++;
        UIUtils.FixValue(ref mCycle, 1, 7);
    }
}
