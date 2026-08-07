using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;

namespace QuestBooks.Common.UI.Components;

public sealed class BackgroundPanel : UIPanel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BackgroundPanel"/> <see langword="class"/>.
    /// </summary>
    public BackgroundPanel() => BackgroundColor = UICommon.MainPanelBackground;
}