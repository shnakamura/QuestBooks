using Terraria.Audio;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements; 

public class Element : UIElement
{
    /// <summary>
    ///     Gets a value indicating whether the element is being hovered over.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the element is being hovered over; otherwise, <see langword="false"/>.
    /// </value>
    public bool Hovered => ContainsPoint(Main.MouseScreen);
    
    /// <summary>
    ///     Gets the local player instance.
    /// </summary>
    public Player Player => Main.LocalPlayer;
    
    /// <summary>
    ///     Gets or sets a value indicating whether the mouse should be captured while hovering over the state.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the mouse should be captured while hovering over the state; otherwise, <see langword="false"/>.
    /// </value>
    public bool Focus { get; set; } = true;
    
    /// <summary>
    ///     Gets or sets the sound settings of the element.
    /// </summary>
    public ElementSoundSettings Sounds { get; set; }
    
    /// <summary>
    ///     Gets or sets the tooltip settings of the element.
    /// </summary>
    public ElementTooltipSettings Tooltip { get; set; }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);

        if (!Sounds.Enabled)
        {
            return;
        }

        SoundEngine.PlaySound(Sounds.Hover);
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);

        if (!Sounds.Enabled)
        {
            return;
        }
        
        SoundEngine.PlaySound(Sounds.Click);
    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Player.mouseInterface |= Focus && Hovered;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        
        if (Tooltip.Enabled && IsMouseHovering)
        {
            Main.instance.MouseText(Tooltip.Text);
        }
    }
}

public static class ElementExtensions
{
    public static TElement WithFocus<TElement>(this TElement element) where TElement : Element
    {
        element.Focus = true;
        
        return element;
    }
}