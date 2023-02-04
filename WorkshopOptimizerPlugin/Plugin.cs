using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using WorkshopOptimizerPlugin.Windows;
using Dalamud.Game.Gui;

namespace WorkshopOptimizerPlugin;

public sealed class Plugin : IDalamudPlugin
{
    public string Name => "Workshop Optimizer Plugin";

    public ChatGui ChatGui { get; init; }
    public Configuration Configuration { get; init; }
    public WindowSystem WindowSystem = new("WorkshopOptimizerPlugin");
    public readonly Icons Icons;

    private const string CommandName = "/wso";

    private DalamudPluginInterface PluginInterface { get; init; }
    private CommandManager CommandManager { get; init; }

    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }

    public Plugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] CommandManager commandManager,
        [RequiredVersion("1.0")] ChatGui chatGui)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;
        ChatGui = chatGui;

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);

        Icons = new Icons(PluginInterface);

        this.ConfigWindow = new ConfigWindow(this);
        this.MainWindow = new MainWindow(this);

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Optimize the workshop"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();
        CommandManager.RemoveHandler(CommandName);
        ConfigWindow.Dispose();
        MainWindow.Dispose();
        Icons.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        MainWindow.IsOpen = true;
    }

    private void DrawUI()
    {
        this.WindowSystem.Draw();
    }

    public void DrawConfigUI()
    {
        ConfigWindow.IsOpen = true;
    }
}
