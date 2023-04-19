using Dalamud.Configuration;
using Dalamud.Plugin;
using WorkshopOptimizerPlugin.Data;
using System;

namespace WorkshopOptimizerPlugin;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 3;

    public int DefaultRestCycle1 = 0;
    public int DefaultRestCycle2 = 4;
    public int DefaultTopValues { get; set; } = 10;

    public int ItemGenerationCutoff { get; set; } = 20;

    public int DefaultMultiCycleLimit { get; set; } = Constants.DefaultMultiCycleLimit;

    // the below exist just to make saving less cumbersome
    [NonSerialized]
    private DalamudPluginInterface? pluginInterface;

    public void Initialize(DalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;
    }

    public void Save()
    {
        this.pluginInterface!.SavePluginConfig(this);
    }
}
