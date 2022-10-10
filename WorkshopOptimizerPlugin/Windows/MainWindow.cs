using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.Game;
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
            MinimumSize = new Vector2(375, 600),
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
            new Tuple<string, ITab>("Produced", new ProducedTab(uiDataSource, commonInterfaceElements)),
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
        ShowStatus();
        ImGui.SameLine();
        DrawButtons();
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

    private unsafe void DrawButtons()
    {
        var manager = ManagerProvider.GetManager();
        var cycle = SeasonUtils.GetCycle();
        var hasCycleData = IsSameSeason() && (uiDataSource.DataSource.DataCollectionTime[cycle] != null);
        var isPreviousSeason = SeasonUtils.IsPreviousSeason(uiDataSource.DataSource.SeasonStart);

        var indent = ImGui.GetWindowWidth() - 175;
        ImGui.Indent(indent);
        if (UIUtils.ImageButton((IsSameSeason() || isPreviousSeason) ? plugin.Icons.PopulateData : plugin.Icons.ResetData, "Populate Data", !hasCycleData && manager != null))
        {
            if (!IsSameSeason())
            {
                if (isPreviousSeason)
                {
                    uiDataSource.NextWeek();
                }
                else
                {
                    uiDataSource.Reset();
                }
            }
            PopulateJsonData(manager);
        }
        ImGui.SameLine();
        if (UIUtils.ImageButton(plugin.Icons.SaveData, "Save Data", uiDataSource.Dirty))
        {
            uiDataSource.Save();
        }
        ImGui.SameLine();
#if DEBUG
        var enable_reload = true;
#else
        var enable_reload = uiDataSource.Dirty;
#endif
        if (UIUtils.ImageButton(plugin.Icons.ReloadData, "Reload Data", enable_reload))
        {
            uiDataSource.Reload();
        }
        ImGui.SameLine();
        if (UIUtils.ImageButton(plugin.Icons.ExportData, "Export Data", hasCycleData))
        {
            ExportData();
        }
        ImGui.SameLine();
        if (UIUtils.ImageButton(plugin.Icons.Settings, "Settings"))
        {
            plugin.DrawConfigUI();
        }
        ImGui.Unindent(indent);

#if DEBUG
        ImGui.SameLine();
        indent = ImGui.GetWindowWidth() - 210;
        ImGui.Indent(indent);
        if (UIUtils.ImageButton(plugin.Icons.ResetData, "Reset Data"))
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
        ImGui.Unindent(indent);
#endif
    }

    unsafe private void ShowStatus()
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
