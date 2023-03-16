using ImGuiNET;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Utils;

namespace WorkshopOptimizerPlugin.Windows.Utils;

internal class CommonInterfaceElements
{
    private int mSeason = 0;
    private int mCycle;
    private int mTop;
    private bool mStrictCycles = true;
    private bool[] mRestCycles = new bool[Constants.MaxCycles];

    public int Season => mSeason;
    public int Cycle => UIUtils.FixValue(ref mCycle, 1, 7) - 1;
    public bool StrictCycles => mStrictCycles;
    public int Top => UIUtils.FixValue(ref mTop, 1, Constants.MaxTopItems);
    public bool[] RestCycles => mRestCycles;

    public CommonInterfaceElements(Configuration configuration)
    {
        mTop = configuration.DefaultTopValues;
        mCycle = SeasonUtils.GetCycle() + 1;
        mRestCycles[configuration.DefaultRestCycle1] = true;
        mRestCycles[configuration.DefaultRestCycle2] = true;
    }

    public bool IsCurrentSeason()
    {
        return Season == Constants.CurrentSeason;
    }

    public bool IsPreviousSeason()
    {
        return Season == Constants.PreviousSeason;
    }

    public Groove GetStartGroove(UIDataSource uiDataSource)
    {
        if (Cycle == 0) return new Groove();

        var producedItems = IsCurrentSeason() ? uiDataSource.DataSource.CurrentProducedItems : uiDataSource.DataSource.PreviousProducedItems;
        return producedItems.GrooveAtEndOfCycle[Cycle - 1];
    }

    public Groove GetEndGroove(UIDataSource uiDataSource)
    {
        var producedItems = IsCurrentSeason() ? uiDataSource.DataSource.CurrentProducedItems : uiDataSource.DataSource.PreviousProducedItems;
        return producedItems.GrooveAtEndOfCycle[Cycle];
    }

    public void DrawBasicControls(UIDataSource uiDataSource)
    {
        ImGui.SetNextItemWidth(100);
        ImGui.Combo("Season", ref mSeason, new string[] { "Current", "Previous" }, 2);
        ImGui.SameLine();
        ImGui.SetNextItemWidth(100);
        ImGui.InputInt("Cycle", ref mCycle);
        ImGui.SameLine();
        ImGui.Text(string.Format("Groove: {0} -> {1}", GetStartGroove(uiDataSource), GetEndGroove(uiDataSource))); ;
    }

    public void DrawFilteringControls(UIDataSource uiDataSource)
    {
        ImGui.SetNextItemWidth(100);
        ImGui.InputInt("Top-N", ref mTop, 5);
        ImGui.SameLine();
        if (ImGui.Checkbox("Strict Cycles", ref mStrictCycles))
        {
            uiDataSource.OptimizationParameterChanged();
        }
    }

    public void DrawRestCycleCheckbox(UIDataSource uiDataSource, int cycle)
    {
        int rest_cycles = 0;
        for (int i = 0; i < Constants.MaxCycles; i++)
        {
            if (mRestCycles[i]) { rest_cycles++; }
        }
        var disabled = !IsCurrentSeason() || (rest_cycles >= 2 && !mRestCycles[cycle]);
        if(disabled)
        {
            ImGui.BeginDisabled();
        }
        if (ImGui.Checkbox("Rest Cycle", ref mRestCycles[cycle]))
        {
            if (mRestCycles[cycle])
            {
                var producedItems = uiDataSource.DataSource.CurrentProducedItems;
                for (int w = 0; w < Constants.MaxWorkshops; w++)
                {
                    for (int s = 0; s < Constants.MaxSteps; s++)
                    {
                        producedItems[cycle, w, s] = -1;
                    }
                }
                uiDataSource.DataChanged(cycle);
            }
        }
        if (disabled)
        {
            ImGui.EndDisabled();
        }
    }
}
