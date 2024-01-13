using ImGuiNET;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Utils;

namespace WorkshopOptimizerPlugin.Windows.Tabs;

internal class MaterialsTab : ITab
{
    public void OnOpen() { }

    public void Draw()
    {
        if (ImGui.BeginTable("Materials", 2, ImGuiTableFlags.RowBg | ImGuiTableFlags.ScrollY))
        {
            ImGui.TableSetupColumn("Id");
            ImGui.TableSetupColumn("Count");
            ImGui.TableHeadersRow();

            for (var i = 0; i < Material.MaxMaterials; i++)
            {
                var material = Material.GetMaterialByIndex(i);
                if (material == null) continue;

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text($"{material.Name}");
                ImGui.TableNextColumn();
                ImGui.Text($"{InventoryProvider.GetItemCount(material)}");
            }
            ImGui.EndTable();
        }
    }
}
