using ImGuiNET;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Utils;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class PatternsTab : ITab
{
    private UIDataSource uiDataSource;
    private CommonInterfaceElements ifData;

    public PatternsTab(UIDataSource uiDataSource, CommonInterfaceElements ifData)
    {
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
        ImGui.Text(string.Format("Groove: {0} -> {1}", startGroove, uiDataSource.DataSource.ProducedItems.GrooveAtEndOfCycle[cycle])); ;
        ImGui.Spacing();

        if (ImGui.BeginTable("Patterns", 6, ImGuiTableFlags.ScrollY))
        {
            ImGui.TableSetupColumn("Item", ImGuiTableColumnFlags.WidthFixed, 200);
            ImGui.TableSetupColumn("Cycle", ImGuiTableColumnFlags.WidthFixed, 80);
            ImGui.TableSetupColumn("Expected Supply", ImGuiTableColumnFlags.WidthFixed, 200);
            ImGui.TableSetupColumn("Popularity", ImGuiTableColumnFlags.WidthFixed, 100);
            ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.WidthFixed, 150);
            ImGui.TableSetupColumn("Grooved Value", ImGuiTableColumnFlags.WidthFixed, 150);
            ImGui.TableHeadersRow();

            var items = new List<Item>();
            for (int i = 0; i < Constants.MaxItems; i++)
            {
                var staticData = ItemStaticData.Get(i);
                if (!staticData.IsValid()) { continue; }

                var item = uiDataSource.ItemCache[staticData];
                var pattern = item.FindPattern(cycle);

                if (pattern != null)
                {
                    items.Add(item);
                }
            }

            foreach (var item in items.OrderByDescending(o => o.EffectiveValue(cycle)).ToList())
            {
                var pattern = item.FindPattern(cycle);
                if (pattern == null) { continue; }

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(item.Name);
                ImGui.TableNextColumn();
                ImGui.Text(pattern.Name);
                ImGui.TableNextColumn();
                var expectedSupply = pattern.SupplyPattern[cycle]; ;
                var actualSupply = item.Supply[cycle];
                if (actualSupply == Supply.Unknown || actualSupply == expectedSupply)
                {
                    ImGui.Text(expectedSupply.ToString());
                }
                else
                {
                    ImGui.TextColored(new Vector4(0.75f, 0.5f, 0, 1), $"{expectedSupply} (Actual: {actualSupply})");
                }
                ImGui.TableNextColumn();
                ImGui.Text(item.Popularity.ToString());
                ImGui.TableNextColumn();
                var effValue = item.EffectiveValue(cycle);
                ImGui.Text(string.Format("{0:F2}", effValue));
                ImGui.TableNextColumn();
                ImGui.Text(string.Format("{0:F2}", effValue * startGroove.Multiplier()));
            }
            ImGui.EndTable();
        }
    }
}
