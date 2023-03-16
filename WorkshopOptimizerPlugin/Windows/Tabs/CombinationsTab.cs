using ImGuiNET;
using System;
using System.Linq;
using System.Xml.Linq;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class CombinationsTab : ITab
{
    private readonly Configuration configuration;
    private readonly UIDataSource uiDataSource;
    private CommonInterfaceElements ifData;
    private readonly IItemSetsCache[] itemSetsCaches;

    public CombinationsTab(Configuration configuration, UIDataSource uiDataSource, CommonInterfaceElements ifData, IItemSetsCache[] itemSetsCaches)
    {
        this.configuration = configuration;
        this.uiDataSource = uiDataSource;
        this.ifData = ifData;
        this.itemSetsCaches = itemSetsCaches;
    }

    public void OnOpen() { }

    public void Draw()
    {
        ifData.DrawBasicControls(uiDataSource);
        var cycle = ifData.Cycle;
        var startGroove = ifData.GetStartGroove(uiDataSource);
        Func<Item, string> formatPattern = i => {
            var (pattern, some) = i.FindPattern(cycle);
            return some ? pattern?.Name ?? "*" : "?";
        };

        ImGui.SameLine();
        ifData.DrawFilteringControls(uiDataSource);
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

            var itemCache = ifData.IsCurrentSeason() ? uiDataSource.CurrentItemCache : uiDataSource.PreviousItemCache;
            var producedItems = ifData.IsCurrentSeason() ? uiDataSource.DataSource.CurrentProducedItems : uiDataSource.DataSource.PreviousProducedItems;
            var disabled = ifData.IsPreviousSeason();
            var itemSets = itemSetsCaches[ifData.Season].CachedItemSets[cycle];
            if (itemSets == null)
            {
                var options = new OptimizerOptions(configuration, ifData.StrictCycles ? Strictness.StrictDefaults : Strictness.RelaxedDefaults, ifData.RestCycles);
                itemSetsCaches[ifData.Season].CachedItemSets[cycle] = itemSets = new Optimizer.Optimizer(itemCache, cycle, startGroove, options).GenerateCombinations();
            }
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
                for (int w = 0; w < Constants.MaxWorkshops; w++)
                {
                    if (ImGui.Button($"{w + 1}###C-{w}-{top}"))
                    {
                        for (int s = 0; s < Constants.MaxSteps; s++)
                        {
                            producedItems[cycle, w, s] = (s < itemset.Items.Length) ? (int)itemset.Items[s].Id : -1;
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
