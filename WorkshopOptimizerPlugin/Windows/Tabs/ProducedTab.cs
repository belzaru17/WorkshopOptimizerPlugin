using Dalamud.Bindings.ImGui;
using System.Collections.Generic;
using System.Linq;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class ProducedTab : ITab
{
    private readonly UIDataSource uiDataSource;
    private readonly CommonInterfaceElements ifData;

    public ProducedTab(UIDataSource uiDataSource, CommonInterfaceElements ifData)
    {
        this.uiDataSource = uiDataSource;
        this.ifData = ifData;
    }

    public void OnOpen() { }

    public void Draw()
    {
        ifData.DrawBasicControls();
        var cycle = ifData.Cycle;
        var startGroove = ifData.GetStartGroove();
        ImGui.SameLine();
        ifData.DrawRestCycleCheckbox(cycle);
        ImGui.Spacing();
        DrawProducedTable(cycle, startGroove);
        ImGui.Spacing();
        DrawMaterialsTable(cycle);
    }

    private void DrawProducedTable(int cycle, Groove startGroove)
    {
        if (!ImGui.BeginTable("Produced", 1 + Constants.MaxWorkshops)) { return; }

        ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 200);
        for (var i = 0; i < Constants.MaxWorkshops; i++)
        {
            ImGui.TableSetupColumn($"Workshop {i+1}", ImGuiTableColumnFlags.WidthFixed, 280);
        }
        ImGui.TableHeadersRow();

        var itemCache = ifData.IsCurrentSeason() ? uiDataSource.CurrentItemCache : uiDataSource.PreviousItemCache;
        var producedItems = ifData.IsCurrentSeason() ? uiDataSource.DataSource.CurrentProducedItems : uiDataSource.DataSource.PreviousProducedItems;
        var disabled = ifData.IsPreviousSeason() || ifData.RestCycles[cycle];
        var hours = new int[Constants.MaxWorkshops];
        var items = new List<Item>[Constants.MaxWorkshops];
        for (var w = 0; w < Constants.MaxWorkshops; w++)
        {
            items[w] = new List<Item>();
        }
        for (var step = 0; step < Constants.MaxSteps; step++)
        {
            ImGui.TableNextRow(ImGuiTableRowFlags.None, 27);
            ImGui.TableSetColumnIndex(0);
            ImGui.Text($"Step {step + 1}");
            for (var w = 0; w < Constants.MaxWorkshops; w++)
            {
                if ((step > 0) && (producedItems[cycle, w, step - 1] < 0)) { continue; }

                if (hours[w] >= Constants.MaxHours) { continue; }

                var thisId = producedItems[cycle, w, step];
                var thisItem = (thisId >= 0) ? itemCache[ItemStaticData.Get(thisId)] : null;

                ImGui.TableSetColumnIndex(w + 1);
                ImGui.SetNextItemWidth(200);

                if (disabled) { ImGui.BeginDisabled(); }
                if (ImGui.BeginCombo($"###{w} {step}", thisItem?.Name ?? ""))
                {
                    if (ImGui.Selectable(""))
                    {
                        producedItems[cycle, w, step] = -1;
                        for (var s = step + 1; s < Constants.MaxSteps; s++)
                        {
                            producedItems[cycle, w, s] = -1;
                        }
                        uiDataSource.DataChanged(cycle);
                    }

                    if (step == 0)
                    {
                        foreach (var item in ItemStaticData.GetAllItems().Where(i => i.IsValid()).OrderBy(i => i.Name))
                        {
                            if (ImGui.Selectable(item.Name))
                            {
                                producedItems[cycle, w, step] = (int)item.Id;
                                for (var s = step + 1; s < Constants.MaxSteps; s++)
                                {
                                    producedItems[cycle, w, s] = -1;
                                }
                                uiDataSource.DataChanged(cycle);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in CategoryMap.GetEfficientItems(ItemStaticData.Get((uint)producedItems[cycle, w, step - 1]).Categories).OrderBy(x => x.Name))
                        {
                            if ((hours[w] + item.Hours) > Constants.MaxHours)
                            {
                                continue;
                            }
                            if (ImGui.Selectable(item.Name))
                            {
                                producedItems[cycle, w, step] = (int)item.Id;
                                for (var s = step + 1; s < Constants.MaxSteps; s++)
                                {
                                    producedItems[cycle, w, s] = -1;
                                }
                                uiDataSource.DataChanged(cycle);
                            }
                        }
                    }
                    ImGui.EndCombo();
                }
                if (disabled) { ImGui.EndDisabled(); }

                if (thisItem != null)
                {
                    items[w].Add(thisItem);
                    hours[w] += thisItem.Hours;

                    ImGui.SameLine();
                    ImGui.Text($"{thisItem.Hours}hs ");

                    var (pattern, some) = thisItem.FindPattern(cycle);
                    ImGui.SameLine();
                    ImGui.Text(some ? pattern?.Name ?? " * " : " ? ");
                }
            }
        }


        ImGui.TableNextRow(ImGuiTableRowFlags.Headers, 27);
        ImGui.TableSetColumnIndex(0);
        ImGui.SetNextItemWidth(200);
        ImGui.Text("Hours");
        for (var w = 0; w < Constants.MaxWorkshops; w++)
        {
            ImGui.TableSetColumnIndex(w + 1);
            ImGui.SetNextItemWidth(200);
            ImGui.Text($"{hours[w]}hs");
        }

        var itemSets = new ItemSet[Constants.MaxWorkshops];
        for (var w = 0; w < Constants.MaxWorkshops; w++)
        {
            itemSets[w] = new ItemSet(items[w].ToArray());
        }

        ImGui.TableNextRow(ImGuiTableRowFlags.Headers, 27);
        ImGui.TableSetColumnIndex(0);
        ImGui.SetNextItemWidth(200);
        ImGui.Text("Base Value");
        for (var w = 0; w < Constants.MaxWorkshops; w++)
        {
            ImGui.TableSetColumnIndex(w + 1);
            ImGui.SetNextItemWidth(200);
            ImGui.Text($"{itemSets[w].Value}");
        }

        ImGui.TableNextRow(ImGuiTableRowFlags.Headers, 27);
        ImGui.TableSetColumnIndex(0);
        ImGui.SetNextItemWidth(200);
        ImGui.Text("Value");
        for (var w = 0; w < Constants.MaxWorkshops; w++)
        {
            ImGui.TableSetColumnIndex(w + 1);
            ImGui.SetNextItemWidth(200);
            ImGui.Text($"{itemSets[w].EffectiveValue(cycle) * startGroove.Multiplier():F2}");
        }

        var workshopItemSets = new WorkshopsItemSets(itemSets, cycle, startGroove);
        ImGui.TableNextRow(ImGuiTableRowFlags.Headers, 27);
        ImGui.TableSetColumnIndex(0);
        ImGui.SetNextItemWidth(200);
        ImGui.Text("Total Value");
        ImGui.TableNextColumn();
        ImGui.Text($"{workshopItemSets.EffectiveValue:F2}");

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

        var itemCache = ifData.IsCurrentSeason() ? uiDataSource.CurrentItemCache : uiDataSource.PreviousItemCache;
        var producedItems = ifData.IsCurrentSeason() ? uiDataSource.DataSource.CurrentProducedItems : uiDataSource.DataSource.PreviousProducedItems;
        var gatherableMaterials = new Dictionary<Material, int>();
        var rareMaterials = new Dictionary<Material, int>();
        for (var step = 0; step < Constants.MaxSteps; step++)
        {
            for (var w = 0; w < Constants.MaxWorkshops; w++)
            {
                var id = producedItems[cycle, w, step];
                if (id < 0) { continue; }

                var item = itemCache[ItemStaticData.Get(id)];
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
        for (var i = 0; i < sortedGatherableMaterials.Length || i < sortedRareMaterials.Length; i++)
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
