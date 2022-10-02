using ImGuiNET;
using ImGuiScene;
using System.Numerics;

namespace WorkshopOptimizerPlugin.Utils;

internal static class UIUtils
{
    public static int FixValue(ref int value, int min, int max)
    {
        value = (value < min) ? min : ((value > max) ? max : value);
        return value;
    }

    public static bool ImageButton(TextureWrap icon, string tooltip, int size = 17)
    {
        var pressed = ImGui.ImageButton(icon.ImGuiHandle, new Vector2(size, size));
        if (!pressed && ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.Text(tooltip);
            ImGui.EndTooltip();
        }
        return pressed;
    }
}
