using Dalamud.Interface.Internal;
using Dalamud.Interface.Textures;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System.IO;

namespace WorkshopOptimizerPlugin;

public class Icons
{
    public readonly ISharedImmediateTexture Settings;
    public readonly ISharedImmediateTexture ResetToDefaults;
    public readonly ISharedImmediateTexture ExportData;
    public readonly ISharedImmediateTexture OptimizerSettings;

    public Icons(IDalamudPluginInterface pluginInterface, ITextureProvider textureProvider)
    {
        ISharedImmediateTexture LoadIcon(string basename)
        {
            return textureProvider.GetFromFile(Path.Join(pluginInterface.AssemblyLocation.DirectoryName, "Resources\\Icons", basename));
        }

        Settings = LoadIcon("settings_FILL1_wght400_GRAD0_opsz48.png");
        ResetToDefaults = LoadIcon("refresh_FILL1_wght400_GRAD0_opsz48.png");
        ExportData = LoadIcon("content_copy_FILL1_wght400_GRAD0_opsz48.png");
        OptimizerSettings = LoadIcon("tips_and_updates_FILL1_wght400_GRAD0_opsz48.png");
    }

    public void Dispose()
    {
    }
}
