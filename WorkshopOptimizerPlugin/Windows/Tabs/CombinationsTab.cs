using ImGuiNET;
using System;
using System.Linq;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Windows.Utils;
namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class CombinationsTab : ITab, IUIDataSourceListener
{
    private readonly Configuration configuration;
    private readonly UIDataSource uiDataSource;
    private readonly CommonInterfaceElements ifData;
    private readonly IItemSetsCache[] itemSetsCaches;
    private readonly Optimizer.Optimizer?[,] optimizers;

    public CombinationsTab(Configuration configuration, UIDataSource uiDataSource, CommonInterfaceElements ifData, IItemSetsCache[] itemSetsCaches)
    {
        this.configuration = configuration;
        this.uiDataSource = uiDataSource;
        this.ifData = ifData;
        this.itemSetsCaches = itemSetsCaches;
        this.optimizers = new Optimizer.Optimizer?[Constants.MaxSeasons, Constants.MaxCycles];

        uiDataSource.AddListener(this);
    }

    public void OnDataChange(int cycle)
    {
        for (var i = cycle; i < Constants.MaxCycles; i++)
        {
            this.optimizers[Constants.CurrentSeason, i] = null;
        }
    }

    public void OnOptimizationParameterChange()
    {
        OnDataChange(0);
        for (var i = 0; i < Constants.MaxCycles; i++)
        {
            this.optimizers[Constants.PreviousSeason, i] = null;
        }
    }

    public void OnOpen() { }

    public void Draw()
    {
        ifData.DrawBasicControls(uiDataSource);
        var cycle = ifData.Cycle;
        var startGroove = ifData.GetStartGroove(uiDataSource);
        string formatPattern(Item i)
        {
            var (pattern, some) = i.FindPattern(cycle);
            return some ? pattern?.Name ?? "*" : "?";
        }

        ImGui.SameLine();
        ifData.DrawFilteringControls(uiDataSource);
        ImGui.Spacing();

        var itemCache = ifData.IsCurrentSeason() ? uiDataSource.CurrentItemCache : uiDataSource.PreviousItemCache;
        var itemSets = itemSetsCaches[ifData.Season].CachedItemSets[cycle];
        var progress = 0.0;
        if (itemSets == null)
        {
            var optimizer = optimizers[ifData.Season, cycle];
            if (optimizer == null)
            {
                var options = new OptimizerOptions(configuration, ifData.Strictness, ifData.RestCycles);
                optimizers[ifData.Season, cycle] = optimizer = new Optimizer.Optimizer(itemCache, cycle, startGroove, options);
            }
            (itemSets, progress) = optimizer.GenerateCombinations();

            if (itemSets != null)
            {
                itemSetsCaches[ifData.Season].CachedItemSets[cycle] = itemSets;
            }
        }

        if (itemSets == null)
        {
            var adjProgress = Math.Floor(progress * 20) * 5;
            ImGui.Text($"Calculating, please wait... {adjProgress:F0}%%");
        } else if (ImGui.BeginTable("Combinations", 7, ImGuiTableFlags.ScrollY | ImGuiTableFlags.RowBg))
        {
            ImGui.TableSetupColumn("Items", ImGuiTableColumnFlags.WidthFixed, 400);
            ImGui.TableSetupColumn("Patterns", ImGuiTableColumnFlags.WidthFixed, 150);
            ImGui.TableSetupColumn("Hours", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.WidthFixed, 200);
            ImGui.TableSetupColumn("Total", ImGuiTableColumnFlags.WidthFixed, 50);
            ImGui.TableSetupColumn("Grooved Total", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Set", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableHeadersRow();

            var producedItems = ifData.IsCurrentSeason() ? uiDataSource.DataSource.CurrentProducedItems : uiDataSource.DataSource.PreviousProducedItems;
            var disabled = ifData.IsPreviousSeason();
            var top = ifData.Top;
            foreach (var itemset in itemSets)
            {
                if (top-- == 0) { break; }

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(string.Join("/", itemset.Items.Select(i => i.Name)));
                ImGui.TableNextColumn();
                ImGui.Text(string.Join("/", itemset.Items.Select(i => formatPattern(i))));
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
                if (disabled) { ImGui.BeginDisabled(); }
                for (var w = 0; w < Constants.MaxWorkshops; w++)
                {
                    if (ImGui.Button($"{w + 1}###C-{w}-{top}"))
                    {
                        for (var s = 0; s < Constants.MaxSteps; s++)
                        {
                            producedItems[cycle, w, s] = (s < itemset.Items.Length) ? (int)itemset.Items[s].Id : -1;
                        }
                        uiDataSource.DataChanged(cycle);
                    }
                    ImGui.SameLine();
                }
                if (ImGui.Button($"*###C-*-{top}"))
                {
                    for (var w = 0; w < Constants.MaxWorkshops; w++)
                    {
                        for (var s = 0; s < Constants.MaxSteps; s++)
                        {
                            producedItems[cycle, w, s] = (s < itemset.Items.Length) ? (int)itemset.Items[s].Id : -1;
                        }
                    }
                    uiDataSource.DataChanged(cycle);
                }
                if (disabled) { ImGui.EndDisabled(); }
            }
            ImGui.EndTable();
        }
    }
}
