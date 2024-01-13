using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Numerics;
using WorkshopOptimizerPlugin.Utils;

namespace WorkshopOptimizerPlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    public ConfigWindow(Plugin plugin) : base(
        "Workshop Optimizer Configuration",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.configuration = plugin.Configuration;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public override void Draw()
    {
        var saveConfig = false;

        var defaultRestCycle1 = configuration.DefaultRestCycle1 + 1;
        ImGui.Text("Default Rest Cycle ");
        ImGui.SetNextItemWidth(100);
        if (ImGui.InputInt("1st", ref defaultRestCycle1))
        {
            configuration.DefaultRestCycle1 = UIUtils.FixValue(ref defaultRestCycle1, 1, 7) - 1;
            saveConfig = true;
        }
        ImGui.SameLine();
        var defaultRestCycle2 = configuration.DefaultRestCycle2 + 1;
        ImGui.SetNextItemWidth(100);
        if (ImGui.InputInt("2nd", ref defaultRestCycle2, 100))
        {
            configuration.DefaultRestCycle2 = UIUtils.FixValue(ref defaultRestCycle2, 1, 7) - 1;
            saveConfig = true;
        }

        ImGui.Spacing();
        var defaultTopValues = configuration.DefaultTopValues;
        ImGui.SetNextItemWidth(100);
        if (ImGui.InputInt("Default Top Values", ref defaultTopValues, 5))
        {
            configuration.DefaultTopValues = UIUtils.FixValue(ref defaultTopValues, 1, 2000);
            saveConfig = true;
        }

        var itemGenerationCutoff = configuration.ItemGenerationCutoff;
        ImGui.SetNextItemWidth(100);
        if (ImGui.InputInt("Item Generation Cutoff", ref itemGenerationCutoff, 5))
        {
            configuration.ItemGenerationCutoff = UIUtils.FixValue(ref itemGenerationCutoff, 1, 100);
            saveConfig = true;
        }

        ImGui.Spacing();
        var defaultMultiCycleLimit = configuration.DefaultMultiCycleLimit;
        ImGui.SetNextItemWidth(100);
        if (ImGui.InputInt("Default Multi-Cycle Limit", ref defaultMultiCycleLimit))
        {
            configuration.DefaultMultiCycleLimit = UIUtils.FixValue(ref defaultMultiCycleLimit, 0, 999);
            saveConfig = true;
        }

        ImGui.Spacing();
        var defaultAllowMissingCommonMaterials = configuration.DefaultAllowMissingCommonMaterials;
        if (ImGui.Checkbox("Default Allow Missing Common Materials", ref defaultAllowMissingCommonMaterials))
        {
            configuration.DefaultAllowMissingCommonMaterials = defaultAllowMissingCommonMaterials;
            saveConfig = true;
        }

        if (saveConfig)
        {
            configuration.Save();
        }
    }
}
