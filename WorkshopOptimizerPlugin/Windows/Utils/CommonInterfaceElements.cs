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

    public int Season => mSeason;
    public int Cycle => UIUtils.FixValue(ref mCycle, 1, 7) - 1;
    public bool StrictCycles => mStrictCycles;
    public int Top => UIUtils.FixValue(ref mTop, 1, 2000);

    public CommonInterfaceElements(Configuration configuration)
    {
        mTop = configuration.DefaultTopValues;
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
}
