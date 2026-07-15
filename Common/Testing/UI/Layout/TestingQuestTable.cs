using QuestBooks.Common.Testing.UI.Reload;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Layout;

[TestingInterfaceReload]
public sealed class TestingQuestTable : UIElement
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        var background = new UIPanel(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/PanelBackground"), ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/PanelBorder"))
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };

        Append(background);
    }
}