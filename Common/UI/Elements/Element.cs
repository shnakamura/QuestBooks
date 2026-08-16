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
    ///     Sets the padding for all sides of the element, in pixels.
    /// </summary>
    public float Padding
    {
        set => this.WithPadding(value);
    }

    public (float Width, float Height) Fill
    {
        set => this.WithFill(value.Width, value.Height);
    }

    public (float Horizontal, float Vertical) Align
    {
        set => this.WithAlignment(value.Horizontal, value.Vertical);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Player.mouseInterface |= Focus && Hovered;
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