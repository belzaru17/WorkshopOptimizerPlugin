using Dalamud.Plugin;
using ImGuiScene;
using System.IO;

namespace WorkshopOptimizerPlugin;

public class Icons
{
    public readonly TextureWrap Settings;
    public readonly TextureWrap ResetToDefaults;
    public readonly TextureWrap ExportData;
    public readonly TextureWrap OptimizerSettings;

    public Icons(DalamudPluginInterface pluginInterface)
    {
        TextureWrap LoadIcon(string basename)
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
