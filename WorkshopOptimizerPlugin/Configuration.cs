using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using WorkshopOptimizerPlugin.Data;

namespace WorkshopOptimizerPlugin;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 4;

    public int DefaultRestCycle1 = 0;
    public int DefaultRestCycle2 = 4;
    public int DefaultTopValues { get; set; } = 10;

    public int ItemGenerationCutoff { get; set; } = 20;

    public int DefaultMultiCycleLimit { get; set; } = Constants.DefaultMultiCycleLimit;

    public bool DefaultAllowMissingCommonMaterials { get; set; } = true;

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
