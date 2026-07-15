using Terraria.Audio;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;

namespace QuestBooks.Common.UI.Elements;

public sealed class CloseButton : UIElement
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

        var label = new UIText(Language.GetText("Mods.QuestBooks.UI.Common.Buttons.Close"))
        {
            HAlign = 0.5f,
            VAlign = 0.5f
        };
        
        Append(label);
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        
        SoundEngine.PlaySound(in SoundID.MenuClose);
    }
}