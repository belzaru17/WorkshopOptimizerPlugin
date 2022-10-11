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

    public Groove GetStartGroove(UIDataSource uiDataSource, int cycle)
    {
        if (cycle == 0) return new Groove();

        return uiDataSource.DataSource.ProducedItems.GrooveAtEndOfCycle[cycle - 1];
    }

    public Groove GetEndGroove(UIDataSource uiDataSource, int cycle)
    {
        return uiDataSource.DataSource.ProducedItems.GrooveAtEndOfCycle[cycle];
    }

    public void DrawBasicControls(UIDataSource uiDataSource)
    {
        ImGui.SetNextItemWidth(100);
        ImGui.Combo("Season", ref mSeason, new string[] { "Current", "Previous" }, 2);
        ImGui.SameLine();
        ImGui.SetNextItemWidth(100);
        ImGui.InputInt("Cycle", ref mCycle);
        ImGui.SameLine();
        var cycle = Cycle;
        var startGroove = GetStartGroove(uiDataSource, cycle);
        ImGui.Text(string.Format("Groove: {0} -> {1}", startGroove, GetEndGroove(uiDataSource, cycle))); ;
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
