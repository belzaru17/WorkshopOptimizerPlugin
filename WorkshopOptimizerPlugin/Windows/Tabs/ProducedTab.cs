using ImGuiNET;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Utils;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class ProducedTab : ITab
{
    private readonly Icons icons;
    private readonly UIDataSource uiDataSource;
    private CommonInterfaceElements ifData;

    public ProducedTab(Icons icons, UIDataSource uiDataSource, CommonInterfaceElements ifData)
    {
        this.icons = icons;
        this.uiDataSource = uiDataSource;
        this.ifData = ifData;
    }

    public void OnOpen() { }

    public void Draw()
    {
        ImGui.SetNextItemWidth(100);
        ImGui.InputInt("Cycle", ref ifData.mCycle);
        ImGui.SameLine();
        var cycle = UIUtils.FixValue(ref ifData.mCycle, 1, 7) - 1;
        var startGroove = (cycle == 0) ? new Groove() : uiDataSource.DataSource.ProducedItems.GrooveAtEndOfCycle[cycle - 1];
        ImGui.Text(string.Format("Groove: {0} -> {1}", startGroove, uiDataSource.DataSource.ProducedItems.GrooveAtEndOfCycle[cycle]));
        if (uiDataSource.Dirty)
        {
            ImGui.SameLine();
            if (UIUtils.ImageButton(icons.SaveData, "Save Data"))
            {
                uiDataSource.Save();
            }
        }
        ImGui.Spacing();
        DrawProducedTable(cycle, startGroove);
        ImGui.Spacing();
        DrawMaterialsTable(cycle);
    }

    private void DrawProducedTable(int cycle, Groove startGroove)
    {
        if (!ImGui.BeginTable("Produced", 4)) { return; }

        ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 200);
        ImGui.TableSetupColumn("Workshop 1", ImGuiTableColumnFlags.WidthFixed, 200);
        ImGui.TableSetupColumn("Workshop 2", ImGuiTableColumnFlags.WidthFixed, 200);
        ImGui.TableSetupColumn("Workshop 3", ImGuiTableColumnFlags.WidthFixed, 200);
        ImGui.TableHeadersRow();

        var hours = new int[Constants.MaxWorkshops];
        var values = new double[Constants.MaxWorkshops];
        for (int step = 0; step < Constants.MaxSteps; step++)
        {
            ImGui.TableNextRow(ImGuiTableRowFlags.None, 27);
            ImGui.TableSetColumnIndex(0);
            ImGui.Text($"Step {step + 1}");
            for (int w = 0; w < Constants.MaxWorkshops; w++)
            {
                var lastId = -1;
                if (step > 0)
                {
                    lastId = uiDataSource.DataSource.ProducedItems[cycle, w, step - 1];
                    if (lastId < 0) { continue; }
                }

                if (hours[w] >= Constants.MaxHours) { continue; }

                var thisId = uiDataSource.DataSource.ProducedItems[cycle, w, step];
                Item? thisItem = (thisId >= 0) ? uiDataSource.ItemCache[ItemStaticData.Get(thisId)] : null;

                ImGui.TableSetColumnIndex(w + 1);
                ImGui.SetNextItemWidth(200);

                if (ImGui.BeginCombo($"{w} {step}", thisItem?.Name ?? ""))
                {
                    if (ImGui.Selectable(""))
                    {
                        uiDataSource.DataSource.ProducedItems[cycle, w, step] = -1;
                        uiDataSource.DataChanged(cycle);
                    }

                    if (step == 0)
                    {
                        for (uint i = 0; i < Constants.MaxItems; i++)
                        {
                            var item = ItemStaticData.Get(i);
                            if (!item.IsValid()) { continue; }

                            if (ImGui.Selectable(item.Name))
                            {
                                uiDataSource.DataSource.ProducedItems[cycle, w, step] = (int)item.Id;
                                for (int s = step + 1; s < Constants.MaxSteps; s++)
                                {
                                    uiDataSource.DataSource.ProducedItems[cycle, w, s] = -1;
                                }
                                uiDataSource.DataChanged(cycle);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in CategoryMap.GetEfficientItems(ItemStaticData.Get((uint)uiDataSource.DataSource.ProducedItems[cycle, w, step - 1]).Categories))
                        {
                            if ((hours[w] + item.Hours) > Constants.MaxHours)
                            {
                                continue;
                            }
                            if (ImGui.Selectable(item.Name))
                            {
                                uiDataSource.DataSource.ProducedItems[cycle, w, step] = (int)item.Id;
                                for (int s = step + 1; s < Constants.MaxSteps; s++)
                                {
                                    uiDataSource.DataSource.ProducedItems[cycle, w, s] = -1;
                                }
                                uiDataSource.DataChanged(cycle);
                            }
                        }
                    }
                    ImGui.EndCombo();
                }

                if (thisItem != null)
                {
                    hours[w] += thisItem.Hours;
                    values[w] += thisItem.EffectiveValue(cycle) * ItemSet.ItemsPerStep(step);
                }

            }
        }

        ImGui.TableNextRow(ImGuiTableRowFlags.Headers, 27);
        ImGui.TableSetColumnIndex(0);
        ImGui.SetNextItemWidth(200);
        ImGui.Text("Hours");
        for (int w = 0; w < Constants.MaxWorkshops; w++)
        {
            ImGui.TableSetColumnIndex(w + 1);
            ImGui.SetNextItemWidth(200);
            ImGui.Text($"{hours[w]}hs");
        }

        ImGui.TableNextRow(ImGuiTableRowFlags.Headers, 27);
        ImGui.TableSetColumnIndex(0);
        ImGui.SetNextItemWidth(200);
        ImGui.Text("Value");
        for (int w = 0; w < Constants.MaxWorkshops; w++)
        {
            ImGui.TableSetColumnIndex(w + 1);
            ImGui.SetNextItemWidth(200);
            ImGui.Text($"{values[w]:F2}");
        }

        var totalValue = 0.0;
        ImGui.TableNextRow(ImGuiTableRowFlags.Headers, 27);
        ImGui.TableSetColumnIndex(0);
        ImGui.SetNextItemWidth(200);
        ImGui.Text("Grooved Value");
        for (int w = 0; w < Constants.MaxWorkshops; w++)
        {
            ImGui.TableSetColumnIndex(w + 1);
            ImGui.SetNextItemWidth(200);
            var gValue = values[w] * startGroove.Multiplier();
            ImGui.Text($"{gValue:F2}");
            totalValue += gValue;
        }

        ImGui.TableNextRow(ImGuiTableRowFlags.Headers, 27);
        ImGui.TableSetColumnIndex(0);
        ImGui.SetNextItemWidth(200);
        ImGui.Text("Total Value");
        ImGui.TableNextColumn();
        ImGui.Text($"{totalValue:F2}");

        ImGui.EndTable();
    }

    private void DrawMaterialsTable(int cycle)
    {
        if (!ImGui.BeginTable("Materials", 6)) { return; }

        ImGui.TableSetupColumn("Gatherable Material", ImGuiTableColumnFlags.WidthFixed, 200);
        ImGui.TableSetupColumn("Amount", ImGuiTableColumnFlags.WidthFixed, 100);
        ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 50);
        ImGui.TableSetupColumn("Rare Material", ImGuiTableColumnFlags.WidthFixed, 200);
        ImGui.TableSetupColumn("Source", ImGuiTableColumnFlags.WidthFixed, 150);
        ImGui.TableSetupColumn("Amount", ImGuiTableColumnFlags.WidthFixed, 100);
        ImGui.TableHeadersRow();

        var gatherableMaterials = new Dictionary<Material, int>();
        var rareMaterials = new Dictionary<Material, int>();
        for (int step = 0; step < Constants.MaxSteps; step++)
        {
            for (int w = 0; w < Constants.MaxWorkshops; w++)
            {
                var id = uiDataSource.DataSource.ProducedItems[cycle, w, step];
                if (id < 0) { continue; }

                var item = uiDataSource.ItemCache[ItemStaticData.Get(id)];
                foreach (var m in item.Materials)
                {
                    var mats = (m.Material.Source == MaterialSource.Gatherable) ? gatherableMaterials : rareMaterials;
                    if (!mats.TryAdd(m.Material, m.Count))
                    {
                        mats[m.Material] += m.Count;
                    }
                }
            }
        }

        var sortedGatherableMaterials = gatherableMaterials.OrderBy(x => x.Key.Name).ToArray();
        var sortedRareMaterials = rareMaterials .OrderBy(x => x.Key.Name).ToArray();
        for (int i = 0; i < sortedGatherableMaterials.Length || i < sortedRareMaterials.Length; i++)
        {
            ImGui.TableNextRow();
            if (i < sortedGatherableMaterials.Length)
            {
                var it = sortedGatherableMaterials[i];
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(it.Key.Name);
                ImGui.TableNextColumn();
                ImGui.Text(it.Value.ToString());
            }
            if (i < sortedRareMaterials.Length)
            {
                var it = sortedRareMaterials[i];
                ImGui.TableSetColumnIndex(3);
                ImGui.Text(it.Key.Name);
                ImGui.TableNextColumn();
                ImGui.Text(it.Key.Source.ToString());
                ImGui.TableNextColumn();
                ImGui.Text(it.Value.ToString());
            }
        }

        ImGui.EndTable();
    }
}
