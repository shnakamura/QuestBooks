using ReLogic.Content;

namespace QuestBooks.Common.UI.Elements;

public sealed class SettingsPanel() : Panel(BACKGROUND_TEXTURE, BORDER_TEXTURE)
{
    /// <summary>
    ///     The texture asset of the background of the settings panel.
    /// </summary>
    public static readonly Asset<Texture2D> BACKGROUND_TEXTURE = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/SettingsPanelBackground");

    /// <summary>
    ///     The texture asset of the border of the settings panel.
    /// </summary>
    public static readonly Asset<Texture2D> BORDER_TEXTURE = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/SettingsPanelBorder");
}