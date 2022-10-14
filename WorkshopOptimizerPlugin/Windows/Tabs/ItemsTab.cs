using Dalamud.Plugin.Ipc.Exceptions;
using ImGuiNET;
using System;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Utils;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class ItemsTab : ITab
{
    private UIDataSource uiDataSource;
    private CommonInterfaceElements ifData;
    private Icons icons;

    private int mSetWhen;
    private int[] mWhenOveerides;
    public bool hasWhenOverrides;

    public ItemsTab(Icons icons, UIDataSource uiDataSource, CommonInterfaceElements ifData)
    {
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
        ifData.DrawBasicControls(uiDataSource);
        var cycle = ifData.Cycle;
        ImGui.SameLine();
        DrawActionsBar();
        ImGui.Spacing();

        if (ImGui.BeginTable("Data", 9, ImGuiTableFlags.ScrollY | ImGuiTableFlags.RowBg))
        {
            ImGui.TableSetupColumn("Item", ImGuiTableColumnFlags.WidthFixed, 150);
            ImGui.TableSetupColumn("Hours", ImGuiTableColumnFlags.WidthFixed, 50);
            ImGui.TableSetupColumn("Popularity", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Supply", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Demand", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Cycles", ImGuiTableColumnFlags.WidthFixed, 350);
            ImGui.TableSetupColumn("Base Value", ImGuiTableColumnFlags.WidthFixed, 80);
            ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.WidthFixed, 80);
            ImGui.TableSetupColumn("When to Use", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableHeadersRow();

            var itemCache = ifData.IsCurrentSeason() ? uiDataSource.CurrentItemCache : uiDataSource.PreviousItemCache;
            var disabled = ifData.IsPreviousSeason();
            for (uint i = 0; i < Constants.MaxItems; i++)
            {
                var staticData = ItemStaticData.Get(i);
                if (!staticData.IsValid()) { continue; }

                var item = itemCache[staticData];
                var patterns = item.FindPatterns(cycle);
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(item.Name);
                ImGui.TableNextColumn();
                ImGui.Text($"{item.Hours}");
                ImGui.TableNextColumn();
                ImGui.Text($"{item.Popularity}");
                ImGui.TableNextColumn();
                if (item.Supply[cycle] != Supply.Unknown || patterns.Count != 1)
                {
                    ImGui.Text($"{item.Supply[cycle]}");
                }
                else
                {
                    ImGui.Text($"{patterns[0].SupplyPattern[cycle]}*");
                }
                ImGui.TableNextColumn();
                if (item.Demand[cycle] != Demand.Unknown || patterns.Count != 1)
                {
                    ImGui.Text($"{item.Demand[cycle]}");
                } else
                {
                    ImGui.Text($"{patterns[0].DemandPattern[cycle]}*");
                }
                ImGui.TableNextColumn();
                var spatterns = string.Join("/", patterns.ConvertAll(p => p.Name));
                ImGui.Text(spatterns);
                ImGui.TableNextColumn();
                ImGui.Text(item.Value.ToString());
                ImGui.TableNextColumn();
                ImGui.Text($"{item.EffectiveValue(cycle):F2}");
                ImGui.TableNextColumn();
                if (disabled) { ImGui.BeginDisabled(); }
                if (ImGui.Combo($"###Item {i}", ref mWhenOveerides[i], WhenUtils.WhenAsStrings, WhenUtils.WhenAsStrings.Length))
                {
                    uiDataSource.WhenOverrides[i] = (When)mWhenOveerides[i];
                    uiDataSource.OptimizationParameterChanged();
                    hasWhenOverrides = true;
                }
                if (disabled) { ImGui.EndDisabled(); }
            }
            ImGui.EndTable();
        }
    }

    private void DrawActionsBar()
    {
        var indent = 780;
        ImGui.Indent(indent);
        ImGui.Text("Set ");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(100);
        ImGui.Combo("###SET_TYPE", ref mSetWhen, new string[]{"All", "Common", "Gatherables"}, 3);
        Func<MaterialSource, bool>[] checkMaterials = {
            s => true,
            s => s == MaterialSource.Gatherable || s == MaterialSource.Crop || s == MaterialSource.Dropping,
            s => s == MaterialSource.Gatherable,
        };
        ImGui.SameLine();
        ImGui.Text(" to ");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(100);
        if (ImGui.BeginCombo("###SET_WHEN", ""))
        {
            foreach (When w in new When[] { When.Never, When.Weak, When.Strong, When.Either, When.Always })
            {
                if (ImGui.Selectable(w.ToString()))
                {
                    for (uint i = 0; i < Constants.MaxItems; i++)
                    {
                        var itemData = ItemStaticData.Get(i);
                        if (!itemData.IsValid()) continue;

                        bool matches = true;
                        foreach (var materials in itemData.Materials)
                        {
                            matches &= checkMaterials[mSetWhen](materials.Material.Source);
                        }
                        if (matches)
                        {
                            uiDataSource.WhenOverrides[i] = w;
                            mWhenOveerides[i] = (int)w;
                        }
                    }
                    uiDataSource.OptimizationParameterChanged();
                    hasWhenOverrides = true;
                }
            }
            ImGui.EndCombo();
        }
        ImGui.SameLine();
        if (UIUtils.ImageButton(icons.ResetToDefaults, "Reset to defaults", hasWhenOverrides))
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
