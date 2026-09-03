using Terraria.ModLoader.UI;

namespace QuestBooks.Common.UI;

public sealed class Button : IElementStyle
{
    public static void Apply<TElement>(TElement element) where TElement : Element
    {
        switch (element)
        {
            case Image image:
                image.Highlight = UICommon.DefaultUIBorderMouseOver;
                
                // TODO: Tweens.
                image.WithLeftClickCallback(i => i.Scale /= 2f);
                image.WithUpdateCallback(i => i.Scale = MathHelper.SmoothStep(i.Scale, i.IsMouseHovering ? 1.25f : 1f, 0.33f));
                break;
            case Panel panel:
                panel.Highlight = UICommon.DefaultUIBorderMouseOver;
                break;
        }
        
        element.Attach(new InterfaceMouse());
        element.Attach(InterfaceSounds.FromSounds(in SoundID.MenuTick, in SoundID.MenuOpen));
    }
}