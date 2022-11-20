using Dalamud.Plugin;
using ImGuiScene;
using System.IO;

namespace WorkshopOptimizerPlugin;

public class Icons
{
    public readonly TextureWrap Settings;
    public readonly TextureWrap ResetToDefaults;
    public readonly TextureWrap ExportData;

    private DalamudPluginInterface pluginInterface;

    public Icons(DalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;

        Settings = LoadIcon("settings_FILL1_wght400_GRAD0_opsz48.png");
        ResetToDefaults = LoadIcon("refresh_FILL1_wght400_GRAD0_opsz48.png");
        ExportData = LoadIcon("content_copy_FILL1_wght400_GRAD0_opsz48.png");
    }

    public void Dispose()
    {
        Settings.Dispose();
        ResetToDefaults.Dispose();
        ExportData.Dispose();
    }

    private TextureWrap LoadIcon(string basename)
    {
        return pluginInterface.UiBuilder.LoadImage(Path.Join(pluginInterface.AssemblyLocation.DirectoryName, "Resources\\Icons", basename));
    }
}
