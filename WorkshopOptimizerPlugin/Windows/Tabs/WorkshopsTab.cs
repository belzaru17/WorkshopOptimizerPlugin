using ImGuiNET;
using System.Linq;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Utils;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class WorkshopsTab : ITab
{
    private readonly Configuration configuration;
    private readonly Icons icons;
    private readonly UIDataSource uiDataSource;
    private CommonInterfaceElements ifData;
    private readonly IItemSetsCache itemSetsCache;

    public WorkshopsTab(Configuration configuration, Icons icons, UIDataSource uiDataSource, CommonInterfaceElements ifData, IItemSetsCache itemSetsCache)
    {
        this.configuration = configuration;
        this.icons = icons;
        this.uiDataSource = uiDataSource;
        this.ifData = ifData;
        this.itemSetsCache = itemSetsCache;
    }

    public void OnOpen() { }

    public void Draw()
    {
        ImGui.SetNextItemWidth(100);
        ImGui.InputInt("Cycle", ref ifData.mCycle);
        ImGui.SameLine();
        var cycle = UIUtils.FixValue(ref ifData.mCycle, 1, 7) - 1;
        var startGroove = (cycle == 0) ? new Groove() : uiDataSource.DataSource.ProducedItems.GrooveAtEndOfCycle[cycle - 1];
        ImGui.Text(string.Format("Groove: {0} -> {1}", startGroove, uiDataSource.DataSource.ProducedItems.GrooveAtEndOfCycle[cycle])); ;
        ImGui.SameLine();
        ImGui.SetNextItemWidth(100);
        ImGui.InputInt("Top-N", ref ifData.mTop, 5);
        ImGui.SameLine();
        if (ImGui.Checkbox("Strict Cycles", ref ifData.mStrictCycles))
        {
            uiDataSource.OptimizationParameterChanged();
        }
        if (uiDataSource.Dirty)
        {
            ImGui.SameLine();
            ImGui.Indent(Constants.UIButtonIndent);
            if (UIUtils.ImageButton(icons.SaveData, "Save Data"))
            {
                uiDataSource.Save();
            }
            ImGui.SameLine();
            if (UIUtils.ImageButton(icons.ReloadData, "Reload Data"))
            {
                 uiDataSource.Reload();
            }
            ImGui.Unindent(Constants.UIButtonIndent);
        }
        ImGui.Spacing();

        if (ImGui.BeginTable("Workshop Combinations", 6, ImGuiTableFlags.ScrollY))
        {
            ImGui.TableSetupColumn("Items", ImGuiTableColumnFlags.WidthFixed, 400);
            ImGui.TableSetupColumn("Patterns", ImGuiTableColumnFlags.WidthFixed, 200);
            ImGui.TableSetupColumn("Hours", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Total", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Groove", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Set", ImGuiTableColumnFlags.WidthFixed, 200);
            ImGui.TableHeadersRow();

            var options = new OptimizerOptions(configuration, ifData.mStrictCycles ? Strictness.AllowExactCycle : (Strictness.AllowExactCycle | Strictness.AllowRestCycle | Strictness.AllowEarlierCycle));
            var cWorkshopsItemSets = itemSetsCache.CachedWorkshopsItemSets[cycle];
            if (cWorkshopsItemSets == null)
            {
                cWorkshopsItemSets = new Optimizer.Optimizer(uiDataSource.ItemCache, cycle, options).GenerateAllWorkshops(startGroove);
                itemSetsCache.CachedWorkshopsItemSets[cycle] = cWorkshopsItemSets;
            }
            var top = UIUtils.FixValue(ref ifData.mTop, 1, 2000);
            foreach (var workshopsItemSets in cWorkshopsItemSets)
            {
                if (top-- == 0) { break; }

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                for (int w = 0; w < Constants.MaxWorkshops; w++)
                {
                    ImGui.Text(string.Join("/", workshopsItemSets.ItemSets[w].Items.Select(i => i.Name)));
                }
                ImGui.TableNextColumn();
                for (int w = 0; w < Constants.MaxWorkshops; w++)
                {
                    ImGui.Text(string.Join("/", workshopsItemSets.ItemSets[w].Items.Select(i => (i.FindPattern(cycle))?.Name ?? "*")));
                }
                ImGui.TableNextColumn();
                for (int w = 0; w < Constants.MaxWorkshops; w++)
                {
                    ImGui.Text(string.Join("/", workshopsItemSets.ItemSets[w].Items.Select(i => i.Hours)));
                }
                ImGui.TableNextColumn();
                ImGui.Text("");
                ImGui.Text(string.Format("{0:F2}", workshopsItemSets.Value));
                ImGui.TableNextColumn();
                ImGui.Text("");
                ImGui.Text(string.Format("Groove: {0} -> {1}", startGroove, workshopsItemSets.EndGroove));
                ImGui.TableNextColumn();
                ImGui.Text("");
                if (ImGui.Button($"*###WC-*-{top}"))
                {
                    for (int w = 0; w < Constants.MaxWorkshops; w++)
                    {
                        var itemSet = workshopsItemSets.ItemSets[w];
                        for (int s = 0; s < Constants.MaxSteps; s++)
                        {
                            uiDataSource.DataSource.ProducedItems[cycle, w, s] = (s < itemSet.Items.Length) ? (int)itemSet.Items[s].Id : -1;
                        }
                    }
                    uiDataSource.DataChanged(cycle);
                }
            }
            ImGui.EndTable();
        }
    }
}
