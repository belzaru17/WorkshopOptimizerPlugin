using Dalamud.Plugin;
using ImGuiScene;
using System.IO;

namespace WorkshopOptimizerPlugin;

public class Icons
{
    public readonly TextureWrap Settings;
    public readonly TextureWrap ResetToDefaults;
    public readonly TextureWrap ExportData;
    public readonly TextureWrap PopulateData;
    public readonly TextureWrap ResetData;
    public readonly TextureWrap SaveData;
    public readonly TextureWrap ReloadData;

    private DalamudPluginInterface pluginInterface;

    public Icons(DalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;

        Settings = LoadIcon("settings_FILL1_wght400_GRAD0_opsz48.png");
        ResetToDefaults = LoadIcon("undo_FILL1_wght400_GRAD0_opsz48.png");
        ExportData = LoadIcon("content_copy_FILL1_wght400_GRAD0_opsz48.png");
        PopulateData = LoadIcon("cloud_download_FILL1_wght400_GRAD0_opsz48.png");
        ResetData = LoadIcon("delete_forever_FILL1_wght400_GRAD0_opsz48.png");
        SaveData = LoadIcon("save_FILL1_wght400_GRAD0_opsz48.png");
        ReloadData = LoadIcon("refresh_FILL1_wght400_GRAD0_opsz48.png");
    }

    public void Dispose()
    {
        Settings.Dispose();
        ResetToDefaults.Dispose();
    }

    private TextureWrap LoadIcon(string basename)
    {
        return pluginInterface.UiBuilder.LoadImage(Path.Join(pluginInterface.AssemblyLocation.DirectoryName, "Resources\\Icons", basename));
    }
}
