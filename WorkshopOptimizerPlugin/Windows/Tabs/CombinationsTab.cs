using ImGuiNET;
using System.Linq;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Utils;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class CombinationsTab : ITab
{
    private readonly Configuration configuration;
    private readonly Icons icons;
    private readonly UIDataSource uiDataSource;
    private CommonInterfaceElements ifData;
    private readonly IItemSetsCache itemSetsCache;

    public CombinationsTab(Configuration configuration, Icons icons, UIDataSource uiDataSource, CommonInterfaceElements ifData, IItemSetsCache itemSetsCache)
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
        ImGui.Spacing();

        if (ImGui.BeginTable("Combinations", 7, ImGuiTableFlags.ScrollY | ImGuiTableFlags.RowBg))
        {
            ImGui.TableSetupColumn("Items", ImGuiTableColumnFlags.WidthFixed, 400);
            ImGui.TableSetupColumn("Patterns", ImGuiTableColumnFlags.WidthFixed, 150);
            ImGui.TableSetupColumn("Hours", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.WidthFixed, 200);
            ImGui.TableSetupColumn("Total", ImGuiTableColumnFlags.WidthFixed, 50);
            ImGui.TableSetupColumn("Grooved Total", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Set", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableHeadersRow();

            var itemSets = itemSetsCache.CachedItemSets[cycle];
            if (itemSets == null)
            {
                var options = new OptimizerOptions(configuration, ifData.mStrictCycles ? Strictness.AllowExactCycle : (Strictness.AllowExactCycle | Strictness.AllowRestCycle | Strictness.AllowEarlierCycle));
                itemSetsCache.CachedItemSets[cycle] = itemSets = new Optimizer.Optimizer(uiDataSource.ItemCache, cycle, startGroove, options).GenerateCombinations();
            }
            var top = UIUtils.FixValue(ref ifData.mTop, 1, 2000);
            foreach (var itemset in itemSets)
            {
                if (top-- == 0) { break; }

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(string.Join("/", itemset.Items.Select(i => i.Name)));
                ImGui.TableNextColumn();
                ImGui.Text(string.Join("/", itemset.Items.Select(i => (i.FindPattern(cycle))?.Name ?? "*")));
                ImGui.TableNextColumn();
                ImGui.Text(string.Join("/", itemset.Items.Select(i => i.Hours)));
                ImGui.TableNextColumn();
                ImGui.Text(string.Join("/", itemset.Items.Select(i => string.Format("{0:F2}", i.EffectiveValue(cycle)))));
                ImGui.TableNextColumn();
                var effValue = itemset.EffectiveValue(cycle);
                ImGui.Text(string.Format("{0:F2}", effValue));
                ImGui.TableNextColumn();
                ImGui.Text(string.Format("{0:F2}", effValue * startGroove.Multiplier()));
                ImGui.TableNextColumn();
                for (int w = 0; w < Constants.MaxWorkshops; w++)
                {
                    if (ImGui.Button($"{w + 1}###C-{w}-{top}"))
                    {
                        for (int s = 0; s < Constants.MaxSteps; s++)
                        {
                            uiDataSource.DataSource.ProducedItems[cycle, w, s] = (s < itemset.Items.Length) ? (int)itemset.Items[s].Id : -1;
                        }
                        uiDataSource.DataChanged(cycle);
                    }
                    ImGui.SameLine();
                }
                if (ImGui.Button($"*###C-*-{top}"))
                {
                    for (int w = 0; w < Constants.MaxWorkshops; w++)
                    {
                        for (int s = 0; s < Constants.MaxSteps; s++)
                        {
                            uiDataSource.DataSource.ProducedItems[cycle, w, s] = (s < itemset.Items.Length) ? (int)itemset.Items[s].Id : -1;
                        }
                    }
                    uiDataSource.DataChanged(cycle);
                }
            }
            ImGui.EndTable();
        }
    }
}
