using QuestBooks.Common.UI.Elements;
using Terraria.ModLoader.UI;

namespace QuestBooks.Common.UI.Components;

public sealed class BackgroundPanel : Panel
{
    /// <summary>
    ///     The background color of the background panel.
    /// </summary>
    public static readonly Color BACKGROUND_COLOR = UICommon.MainPanelBackground;

    /// <summary>
    ///     The border color of the background panel.
    /// </summary>
    public static readonly Color BORDER_COLOR = Color.Black;
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="BackgroundPanel"/> class.
    /// </summary>
    public BackgroundPanel() => Colors = new PanelColorSettings(BACKGROUND_COLOR, BORDER_COLOR);
}