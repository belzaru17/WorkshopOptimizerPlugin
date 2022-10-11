using ImGuiNET;
using System.Linq;
using System.Numerics;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Utils;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class WorkshopsTab : ITab, IUIDataSourceListener
{
    private readonly Configuration configuration;
    private readonly Icons icons;
    private readonly UIDataSource uiDataSource;
    private CommonInterfaceElements ifData;
    private readonly IItemSetsCache itemSetsCache;
    private Optimizer.Optimizer?[] optimizers;

    public WorkshopsTab(Configuration configuration, Icons icons, UIDataSource uiDataSource, CommonInterfaceElements ifData, IItemSetsCache itemSetsCache)
    {
        this.configuration = configuration;
        this.icons = icons;
        this.uiDataSource = uiDataSource;
        this.ifData = ifData;
        this.itemSetsCache = itemSetsCache;
        this.optimizers = new Optimizer.Optimizer?[Constants.MaxCycles];

        uiDataSource.AddListener(this);
    }

    public void OnDataChange(int cycle)
    {
        for (int i = cycle; i < Constants.MaxCycles; i++)
        {
            this.optimizers[i] = null;
        }
    }

    public void OnOptimizationParameterChange()
    {
        OnDataChange(0);
    }

    public void OnOpen() { }

    public void Draw()
    {
        ifData.DrawBasicControls(uiDataSource);
        var cycle = ifData.Cycle;
        var startGroove = ifData.GetStartGroove(uiDataSource, cycle);
        ImGui.SameLine();
        ifData.DrawFilteringControls(uiDataSource);
        ImGui.Spacing();

        var optimizer = optimizers[cycle];
        if (optimizer == null)
        {
            var options = new OptimizerOptions(configuration, ifData.StrictCycles? Strictness.AllowExactCycle : (Strictness.AllowExactCycle | Strictness.AllowRestCycle | Strictness.AllowEarlierCycle));
            optimizers[cycle] = optimizer = new Optimizer.Optimizer(uiDataSource.ItemCache, cycle, startGroove, options);

        }
        var cWorkshopsItemSets = itemSetsCache.CachedWorkshopsItemSets[cycle];
        if (cWorkshopsItemSets == null)
        {
            itemSetsCache.CachedWorkshopsItemSets[cycle] = cWorkshopsItemSets = optimizer.GenerateAllWorkshops();
        }
        if (cWorkshopsItemSets == null)
        {
            ImGui.Text("Calculating, please wait...");
        } else if (ImGui.BeginTable("Workshop Combinations", 6, ImGuiTableFlags.ScrollY | ImGuiTableFlags.RowBg))
        {
            ImGui.TableSetupColumn("Items", ImGuiTableColumnFlags.WidthFixed, 400);
            ImGui.TableSetupColumn("Patterns", ImGuiTableColumnFlags.WidthFixed, 200);
            ImGui.TableSetupColumn("Hours", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Total", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Groove", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Set", ImGuiTableColumnFlags.WidthFixed, 200);
            ImGui.TableHeadersRow();

            var top = ifData.Top;
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
