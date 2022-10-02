using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class NextWeekTab : ITab
{
    private UIDataSource data;

    public NextWeekTab(UIDataSource uiDataSource)
    {
        this.data = uiDataSource;
    }

    public void OnOpen() { }

    public void Draw()
    {
        ImGui.Spacing();
        if (ImGui.BeginTable("Next Week's Items", 4))
        {
            ImGui.TableSetupColumn("Item");
            ImGui.TableSetupColumn("Next Week Popularity");
            ImGui.TableSetupColumn("Granary Item");
            ImGui.TableSetupColumn("Granary Mission");
            ImGui.TableHeadersRow();

            var gItems = new List<Tuple<GranaryItemData, Item>>();
            foreach (var gItem in GranaryItemData.Items)
            {
                gItems.Add(new Tuple<GranaryItemData, Item>(gItem, data.ItemCache[ItemStaticData.Get(gItem.Id)]));
            }

            foreach (var i in gItems.OrderBy(o => o.Item2.NextPopularity))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(i.Item2.Name);
                ImGui.TableNextColumn();
                ImGui.Text(i.Item2.NextPopularity.ToString());
                ImGui.TableNextColumn();
                ImGui.Text(i.Item1.Component);
                ImGui.TableNextColumn();
                ImGui.Text(i.Item1.Mission);
            }
            ImGui.EndTable();
        }
    }

}
