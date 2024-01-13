using ImGuiNET;
using System.Linq;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class FavorsTab(UIDataSource uiDataSource) : ITab
{
    private readonly UIDataSource uiDataSource = uiDataSource;

    public void OnOpen() { }

    public void Draw()
    {
        var itemCache = uiDataSource.CurrentItemCache;
        var cycle = SeasonUtils.GetCycle() + 1;
        if (cycle > Constants.MaxCycles) cycle = Constants.MaxCycles;

        ImGui.Spacing();
        if (ImGui.BeginTable("Felicitous Favors Items", 7, ImGuiTableFlags.RowBg))
        {
            ImGui.TableSetupColumn("Item");
            ImGui.TableSetupColumn("Hours");
            ImGui.TableSetupColumn("Needed");
            ImGui.TableSetupColumn("Produced");
            ImGui.TableSetupColumn("Popularity");
            ImGui.TableSetupColumn("Cycles");
            ImGui.TableSetupColumn("Require");
            ImGui.TableHeadersRow();

            for (var i = 0; i < Constants.MaxFelicitousFavors; i++)
            {
                ItemStaticData? selectedItemStaticData = null;
                if (uiDataSource.DataSource.CurrentFelicitousFavors[i] is uint selectedIndex)
                {
                    selectedItemStaticData = ItemStaticData.Get(selectedIndex);
                }
                var selectedItem = itemCache.Get(selectedItemStaticData);

                var hours = 4 + (i * 2);
                var needed = i == 1 ? 6 : 8;
                var done = selectedItem?.AccumulatedProduced(cycle) ?? 0;

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                if (ImGui.BeginCombo($"##Item_{i}", selectedItem?.Name ?? ""))
                {
                    if (ImGui.Selectable(""))
                    {
                        if (selectedItem != null)
                        {
                            uiDataSource.WhenOverrides[selectedItem.Id] = ItemStaticData.Get(selectedItem.Id).When;
                            uiDataSource.OptimizationParameterChanged();
                        }
                        uiDataSource.DataSource.CurrentFelicitousFavors[i] = null;
                        uiDataSource.NeedsSave();
                    }
                    foreach (var item in ItemStaticData.GetAllItems().Where(i => i.IsValid() && i.Hours == hours).OrderBy(i => i.Name))
                    {
                        if (ImGui.Selectable(item.Name))
                        {
                            uiDataSource.DataSource.CurrentFelicitousFavors[i] = item.Id;
                            uiDataSource.NeedsSave();
                        }
                    }
                    ImGui.EndCombo();
                }
                ImGui.TableNextColumn();
                ImGui.Text($"{hours}");
                ImGui.TableNextColumn();
                ImGui.Text($"{needed}");
                ImGui.TableNextColumn();
                ImGui.Text($"{done}");
                ImGui.TableNextColumn();
                ImGui.Text($"{selectedItem?.Popularity.ToString() ?? ""}");
                ImGui.TableNextColumn();
                {
                    var patterns = selectedItem?.FindPatterns(cycle);
                    if (patterns != null)
                    {
                        var spatterns = string.Join("/", patterns.ConvertAll(p => p.Name));
                        ImGui.Text(spatterns);
                    }
                    else
                    {
                        ImGui.Text("");
                    }
                }
                ImGui.TableNextColumn();
                if (selectedItem != null)
                {
                    if (done >= needed)
                    {
                        ImGui.Text("Done");
                    }
                    else if (uiDataSource.WhenOverrides[selectedItem.Id] == When.Required)
                    {
                        if (ImGui.Button($"Defaults##Reset_{i}"))
                        {
                            uiDataSource.WhenOverrides[selectedItem.Id] = ItemStaticData.Get(selectedItem.Id).When;
                            uiDataSource.OptimizationParameterChanged();
                        }
                    }
                    else
                    {
                        if (ImGui.Button($"Require##Require_{i}"))
                        {
                            uiDataSource.WhenOverrides[selectedItem.Id] = When.Required;
                            uiDataSource.OptimizationParameterChanged();
                        }
                    }
                }
            }
            ImGui.EndTable();
        }
    }
}
