using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;
using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Utils;
using WorkshopOptimizerPlugin.Windows.Tabs;
using WorkshopOptimizerPlugin.Windows.Utils;

namespace WorkshopOptimizerPlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;

    private readonly ItemSetsCache itemSetsCache;
    private readonly UIDataSource uiDataSource;
    private CommonInterfaceElements commonInterfaceElements;

    private List<Tuple<string, ITab>> tabs;

    public MainWindow(Plugin plugin) : base(
        "Workshop Optimizer", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;

        itemSetsCache = new ItemSetsCache();
        uiDataSource = UIDataSource.Load();
        uiDataSource.AddListener(itemSetsCache);
        commonInterfaceElements = new CommonInterfaceElements(plugin.Configuration);

        tabs = new()
        {
            new Tuple<string, ITab>("Items", new ItemsTab(plugin.Configuration, plugin.Icons, uiDataSource, commonInterfaceElements)),
            new Tuple<string, ITab>("Patterns", new PatternsTab(uiDataSource, commonInterfaceElements)),
            new Tuple<string, ITab>("Combinations", new CombinationsTab(plugin.Configuration, plugin.Icons, uiDataSource, commonInterfaceElements, itemSetsCache)),
            new Tuple<string, ITab>("Workshops", new WorkshopsTab(plugin.Configuration, plugin.Icons, uiDataSource, commonInterfaceElements, itemSetsCache)),
            new Tuple<string, ITab>("Produced", new ProducedTab(plugin.Icons, uiDataSource, commonInterfaceElements)),
            new Tuple<string, ITab>("Next Week", new NextWeekTab(uiDataSource)),
        };
    }

    public void Dispose() { }

    public override void OnOpen()
    {
        base.OnOpen();
        foreach (var tab in tabs)
        {
            tab.Item2.OnOpen();
        }
    }

    public override void Draw()
    {
        int cycle = SeasonUtils.GetCycle();
        ImGui.Text($"Season: {uiDataSource.DataSource.SeasonStart:yyyy-MM-dd} - {uiDataSource.DataSource.SeasonStart.AddDays(Constants.MaxCycles):yyyy-MM-dd}. Cycle={cycle + 1}.");
        ImGui.SameLine();
        if (UIUtils.ImageButton(plugin.Icons.Settings, "Settings"))
        {
            plugin.DrawConfigUI();
        }
        ImGui.SameLine();
        showStatus();
        ImGui.Spacing();

        if (ImGui.BeginTabBar("Tabs"))
        {
            foreach (var tab in tabs)
            {
                if (ImGui.BeginTabItem(tab.Item1))
                {
                    tab.Item2.Draw();
                    ImGui.EndTabItem();
                }
            }
            ImGui.EndTabBar();
        }
    }

    unsafe private void showStatus()
    {
        if (ManagerProvider.GetManager() == null)
        {
            ImGui.TextColored(new Vector4(0.75f, 0, 0, 1), "Need to visit your island and open Demand & Supply!");
        } else if (uiDataSource.DataSource.DataCollectionTime[SeasonUtils.GetCycle()] == null)
        {
            ImGui.TextColored(new Vector4(0.75f, 0.5f, 0, 1), "Data not collected for this cycle!");
        } else
        {
            ImGui.Text("");
        }
    }
}
