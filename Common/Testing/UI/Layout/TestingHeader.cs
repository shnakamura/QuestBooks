using QuestBooks.Common.Testing.UI.Reload;
using QuestBooks.Common.UI.Elements;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Layout;

[TestingInterfaceReload]
public sealed class TestingHeader : UIElement
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
        
        var icon = new Image(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/Testing/HeaderIcon"))
        {
            HAlign = 0f,
            VAlign = 0.5f,
            Left = StyleDimension.FromPixels(8f)
        };
        
        Append(icon);

        var label = new UIText(Language.GetText("Mods.QuestBooks.UI.Testing.Header"))
        {
            HAlign = 0f,
            VAlign = 0.5f,
            Left = StyleDimension.FromPixels(icon.Left.Pixels + icon.Width.Pixels + 8f)
        };
        
        Append(label);
    }
}