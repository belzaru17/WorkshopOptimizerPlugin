using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using WorkshopOptimizerPlugin.Utils;

namespace WorkshopOptimizerPlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;

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

    public void Dispose() { }

    public override void Draw()
    {
        bool saveConfig = false;

        int defaultRestCycle1 = configuration.DefaultRestCycle1 + 1;
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

        if (saveConfig)
        {
            configuration.Save();
        }
    }
}
