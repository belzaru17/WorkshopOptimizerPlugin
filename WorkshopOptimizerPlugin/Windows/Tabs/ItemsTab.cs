using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Utils;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class ItemsTab : ITab
{
    private Configuration configuration;
    private UIDataSource uiDataSource;
    private CommonInterfaceElements ifData;
    private Icons icons;

    private int[] mWhenOveerides;
    public bool hasWhenOverrides;

    public ItemsTab(Configuration configuration, Icons icons, UIDataSource uiDataSource, CommonInterfaceElements ifData)
    {
        this.configuration = configuration;
        this.uiDataSource = uiDataSource;
        this.ifData = ifData;
        this.icons = icons;

        mWhenOveerides = new int[Constants.MaxItems];
        for (var i = 0; i < Constants.MaxItems; i++)
        {
            mWhenOveerides[i] = (int)uiDataSource.WhenOverrides[(uint)i];
        }
        hasWhenOverrides = false;
    }

    public void OnOpen() { }

    public void Draw()
    {
        ImGui.SetNextItemWidth(100);
        ImGui.InputInt("Cycle", ref ifData.mCycle);
        ImGui.SameLine();
        var cycle = UIUtils.FixValue(ref ifData.mCycle, 1, 7) - 1;
        ImGui.Text(string.Format("Groove: {0} -> {1}", (cycle == 0) ? 0 : uiDataSource.DataSource.ProducedItems.GrooveAtEndOfCycle[cycle - 1], uiDataSource.DataSource.ProducedItems.GrooveAtEndOfCycle[cycle])); ;
        ImGui.SameLine();
        DrawButtons();
        ImGui.Spacing();

        if (ImGui.BeginTable("Data", 8, ImGuiTableFlags.ScrollY))
        {
            ImGui.TableSetupColumn("Item");
            ImGui.TableSetupColumn("Popularity");
            ImGui.TableSetupColumn("Supply");
            ImGui.TableSetupColumn("Demand");
            ImGui.TableSetupColumn("Cycles");
            ImGui.TableSetupColumn("Base Value");
            ImGui.TableSetupColumn("Value");
            ImGui.TableSetupColumn("When to Use");
            ImGui.TableHeadersRow();

            var items = new List<Item>();
            for (uint i = 0; i < Constants.MaxItems; i++)
            {
                var staticData = ItemStaticData.Get(i);
                if (!staticData.IsValid()) { continue; }

                var item = uiDataSource.ItemCache[staticData];
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(item.Name);
                ImGui.TableNextColumn();
                ImGui.Text($"{item.Popularity}");
                ImGui.TableNextColumn();
                ImGui.Text($"{item.Supply[cycle]}");
                ImGui.TableNextColumn();
                ImGui.Text($"{item.Demand[cycle]}");
                var patterns = string.Join("/", item.FindPatterns(cycle).ConvertAll(p => p.Name));
                ImGui.TableNextColumn();
                ImGui.Text(patterns);
                ImGui.TableNextColumn();
                ImGui.Text(item.Value.ToString());
                ImGui.TableNextColumn();
                ImGui.Text($"{item.EffectiveValue(cycle):F2}");
                ImGui.TableNextColumn();
                if (ImGui.Combo($"###Item {i}", ref mWhenOveerides[i], WhenUtils.WhenAsStrings, WhenUtils.WhenAsStrings.Length))
                {
                    uiDataSource.WhenOverrides[i] = (When)mWhenOveerides[i];
                    uiDataSource.OptimizationParameterChanged();
                    hasWhenOverrides = true;
                }
            }
            ImGui.EndTable();
        }
    }

    unsafe private void DrawButtons()
    {
        ImGui.Indent(Constants.UIButtonIndent);
        if (IsSameSeason())
        {
            var cycle = SeasonUtils.GetCycle();
            if (uiDataSource.DataSource.DataCollectionTime[cycle] == null)
            {
                MJIManager* manager = ManagerProvider.GetManager();
                if (manager != null && UIUtils.ImageButton(icons.PopulateData, "Populate Data"))
                {
                    PopulateJsonData(manager);
                }
            } else if (UIUtils.ImageButton(icons.ExportData, "Export Data"))
            {
                ExportData();
            }
        }
        else
        {
            if (UIUtils.ImageButton(icons.ResetData, "Reset Data"))
            {
                ImGui.OpenPopup("Confirm Reset");
            }
            if (ImGui.BeginPopup("Confirm Reset"))
            {
                ImGui.Text("Confirm reset data?");
                if (ImGui.Button("Cancel"))
                {
                    ImGui.CloseCurrentPopup();
                }
                ImGui.SameLine();
                if (ImGui.Button("Reset"))
                {
                    uiDataSource.Reset();
                    ImGui.CloseCurrentPopup();
                }
                ImGui.EndPopup();
            }
        }
        if (configuration.Debug) {
            ImGui.SameLine();
            if (UIUtils.ImageButton(icons.ReloadData, "Reload Data"))
            {
                uiDataSource.Reload();
            }
        }
        ImGui.Unindent(Constants.UIButtonIndent);
        if (hasWhenOverrides)
        {
            ImGui.SameLine();
            var indent = ImGui.GetWindowWidth() - 80;
            ImGui.Indent(indent);
            if (UIUtils.ImageButton(icons.ResetToDefaults, "Reset to defaults"))
            {
                uiDataSource.WhenOverrides.Reset();
                for (uint i = 0; i < Constants.MaxItems; i++)
                {
                    mWhenOveerides[i] = (int)uiDataSource.WhenOverrides[i];
                }
                uiDataSource.OptimizationParameterChanged();
                hasWhenOverrides = false;
            }
            ImGui.Unindent(indent);
        }
    }

    unsafe private void PopulateJsonData(MJIManager* manager)
    {
        byte popIndex = manager->CurrentPopularity;
        byte nextPopIndex = manager->NextPopularity;
        var cycle = SeasonUtils.GetCycle();
        for (uint i = 0; i < Constants.MaxItems; i++)
        {
            var pop = PopularityTable.GetItemPopularity(popIndex, i);
            if (pop == Popularity.Unknown) continue;

            var item = uiDataSource.DataSource.DynamicData[(int)i];
            item.Popularity = pop;
            item.NextPopularity = PopularityTable.GetItemPopularity(nextPopIndex, i);
            item.Supply[cycle] = SupplyUtils.FromFFXIV(manager->GetSupplyForCraftwork(i));
            item.Demand[cycle] = DemandUtils.FromFFXIV(manager->GetDemandShiftForCraftwork(i));
        }
        uiDataSource.DataSource.DataCollectionTime[cycle] = DateTime.UtcNow;
        uiDataSource.DataChanged(cycle);
        uiDataSource.Save();
    }

    unsafe private void ExportData()
    {
        var s = "# item,popularity,supply1,demand1,supply2,demand2,supply3,demand3,supply4,demand4,supply4,demand4,supply6,demand6,supply7,demand7\n";
        for (uint i = 0; i < Constants.MaxItems; i++)
        {
            var staticData = ItemStaticData.Get(i);
            if (!staticData.IsValid()) continue;

            var item = uiDataSource.ItemCache[staticData];
            if (item.Popularity == Popularity.Unknown) continue;

            s += $"{item.Name},{item.Popularity}";
            for (int c = 0; c < Constants.MaxCycles; c++)
            {
                s += $",{item.Supply[c]},{item.Demand[c]}";
            }
            s += "\n";
        }
        Clipboard.CopyTextToClipboard(s);
    }

    private bool IsSameSeason()
    {
        return SeasonUtils.IsSameSeason(uiDataSource.DataSource.SeasonStart);
    }
}
