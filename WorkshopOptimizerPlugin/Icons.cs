using Dalamud.Interface.Internal;
using Dalamud.Plugin;
using ImGuiScene;
using System.IO;

namespace WorkshopOptimizerPlugin;

public class Icons
{
    public readonly IDalamudTextureWrap Settings;
    public readonly IDalamudTextureWrap ResetToDefaults;
    public readonly IDalamudTextureWrap ExportData;
    public readonly IDalamudTextureWrap OptimizerSettings;

    public Icons(DalamudPluginInterface pluginInterface)
    {
        IDalamudTextureWrap LoadIcon(string basename)
        {
            return pluginInterface.UiBuilder.LoadImage(Path.Join(pluginInterface.AssemblyLocation.DirectoryName, "Resources\\Icons", basename));
        }

        Settings = LoadIcon("settings_FILL1_wght400_GRAD0_opsz48.png");
        ResetToDefaults = LoadIcon("refresh_FILL1_wght400_GRAD0_opsz48.png");
        ExportData = LoadIcon("content_copy_FILL1_wght400_GRAD0_opsz48.png");
        OptimizerSettings = LoadIcon("tips_and_updates_FILL1_wght400_GRAD0_opsz48.png");
    }

    public void Dispose()
    {
        Settings.Dispose();
        ResetToDefaults.Dispose();
        ExportData.Dispose();
        OptimizerSettings.Dispose();
    }
}
