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
    private readonly Plugin plugin;

    private readonly ItemSetsCache[] itemSetsCaches;
    private readonly UIDataSource uiDataSource;
    private readonly CommonInterfaceElements commonInterfaceElements;

    private readonly List<Tuple<string, ITab>> tabs;

    public MainWindow(Plugin plugin) : base(
        "Workshop Optimizer", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 600),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;

        itemSetsCaches = new ItemSetsCache[Constants.MaxSeasons] { new ItemSetsCache(), new ItemSetsCache() };
        uiDataSource = UIDataSource.Load(plugin.Configuration);
        for (var i = 0; i < Constants.MaxSeasons; i++)
        {
            uiDataSource.AddListener(itemSetsCaches[i]);
        }
        commonInterfaceElements = new CommonInterfaceElements(plugin.Icons, plugin.Configuration, uiDataSource);

        tabs = new()
        {
            new Tuple<string, ITab>("Workshops", new WorkshopsTab(uiDataSource, commonInterfaceElements, itemSetsCaches)),
            new Tuple<string, ITab>("Produced", new ProducedTab(uiDataSource, commonInterfaceElements)),
            new Tuple<string, ITab>("Next Week", new NextWeekTab(uiDataSource)),
            new Tuple<string, ITab>("Items", new ItemsTab(plugin.Icons, uiDataSource, commonInterfaceElements)),
            new Tuple<string, ITab>("Patterns", new PatternsTab(uiDataSource, commonInterfaceElements)),
            new Tuple<string, ITab>("Combinations", new CombinationsTab(uiDataSource, commonInterfaceElements, itemSetsCaches)),
            new Tuple<string, ITab>("Felicitous Favors", new FavorsTab(uiDataSource)),
#if DEBUG
            new Tuple<string, ITab>("Materials", new MaterialsTab()),
#endif
        };
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        foreach (var tab in tabs)
        {
            tab.Item2.OnOpen();
        }
    }

    public override void OnClose()
    {
        base.OnClose();
        if (uiDataSource.Dirty)
        {
            uiDataSource.Save();
        }
    }

    public override void Draw()
    {
        if (!IsSameSeason())
        {
            if (SeasonUtils.IsPreviousSeason(uiDataSource.DataSource.SeasonStart))
            {
                uiDataSource.NextWeek();
            }
            else
            {
                uiDataSource.Reset(plugin.Configuration);
            }
        }

        var cycle = SeasonUtils.GetCycle();
        var rank = IslandProvider.GetIslandRank();
        var str_rank = rank > 0 ? rank.ToString() : "unknown";
        ImGui.Text($"Season: {uiDataSource.DataSource.SeasonStart:yyyy-MM-dd} - {uiDataSource.DataSource.SeasonStart.AddDays(Constants.MaxCycles):yyyy-MM-dd}. Cycle={cycle + 1}. Island Rank={str_rank}.");
        ImGui.SameLine();
        PopulateDataIfPossible();
#if DEBUG
        ImGui.SameLine();
        if (ImGui.Button("Reset"))
        {
            uiDataSource.Reset(plugin.Configuration);
        }
#endif
        ImGui.SameLine();
        var indent = ImGui.GetWindowWidth() - 45;
        ImGui.Indent(indent);
        if (UIUtils.ImageButton(plugin.Icons.Settings, "Settings"))
        {
            plugin.OpenConfigUI();
        }
        ImGui.Unindent(indent);
        ImGui.Spacing();

        if (rank == 0)
        {
            ImGui.TextColored(new Vector4(0.75f, 0, 0, 1), "Need to visit your island!");
            return;
        }

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

    private void PopulateDataIfPossible()
    {
        var cycle = SeasonUtils.GetCycle();
        if (uiDataSource.DataSource.DataCollectionTime[cycle] == null)
        {
            var manager = ManagerProvider.GetManager();
            if (!manager.IsValid)
            {
                ImGui.TextColored(new Vector4(0.75f, 0, 0, 1), "Need to visit your island and open Demand & Supply!");
                return;
            }

            PopulateJsonData(manager);

            if (commonInterfaceElements.Cycle == cycle)
            {
                commonInterfaceElements.NextCycle();
            }
        }
        ImGui.Text("");
    }

    private void PopulateJsonData(Manager manager)
    {
        var popIndex = manager.CurrentPopularityIndex;
        var nextPopIndex = manager.NextPopularityIndex;
        var cycle = SeasonUtils.GetCycle();
        for (uint i = 0; i < Constants.MaxItems; i++)
        {
            var pop = PopularityTable.GetItemPopularity(popIndex, i);
            if (pop == Popularity.Unknown) continue;

            var item = uiDataSource.DataSource.CurrentDynamicData[(int)i];
            item.Popularity = pop;
            item.NextPopularity = PopularityTable.GetItemPopularity(nextPopIndex, i);
            item.Supply[cycle] = manager.GetSupplyForCraftwork(i);
            item.Demand[cycle] = manager.GetDemandShiftForCraftwork(i);
        }
        uiDataSource.DataSource.DataCollectionTime[cycle] = DateTime.UtcNow;
        uiDataSource.DataChanged(cycle);
    }

    private bool IsSameSeason()
    {
        return SeasonUtils.IsSameSeason(uiDataSource.DataSource.SeasonStart);
    }

}
