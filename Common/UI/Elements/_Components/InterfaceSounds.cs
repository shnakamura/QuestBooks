using Terraria.Audio;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public sealed class InterfaceSounds : ElementComponent
{
    private bool hovered;

    /// <summary>
    ///     The sound style played when the cursor hovers over the element.
    /// </summary>
    public readonly SoundStyle Hover;

    /// <summary>
    ///     The sound style played when the cursor clicks the element.
    /// </summary>
    public readonly SoundStyle Click;
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="InterfaceSounds"/> class with the specified sounds.
    /// </summary>
    /// <param name="hover">
    ///     The sound style played when the cursor hovers over the element.
    /// </param>
    /// <param name="click">
    ///     The sound style played when the cursor clicks the element.
    /// </param>
    private InterfaceSounds(in SoundStyle hover, in SoundStyle click)
    {
        Hover = hover;
        Click = click;
    }
    
    /// <inheritdoc/> 
    public override void Attach(Element element)
    {
        base.Attach(element);

        element.OnUpdate += Update;
        
        element.OnLeftClick += LeftClick;
    }

    /// <inheritdoc/> 
    public override void Detach(Element element)
    {
        base.Detach(element);

        element.OnUpdate -= Update;
        
        element.OnLeftClick -= LeftClick;
    }
    
    // NOTE: UIElement::LeftClick() propagates between parent and child elements, requiring manual hover state tracking for accurate sound playback.
    private void Update(UIElement element)
    {
        if (hovered == element.IsMouseHovering)
        {
            return;
        }

        hovered = element.IsMouseHovering;

        if (!hovered)
        {
            return;
        }
        
        SoundEngine.PlaySound(in Hover);
    }

    private void LeftClick(UIMouseEvent evt, UIElement element) => SoundEngine.PlaySound(in Click);
    
    /// <summary>
    ///     Creates an instance of the <see cref="InterfaceSounds"/> class with the specified sounds.
    /// </summary>
    /// <param name="hover">
    ///     The sound style played when the cursor hovers over the element.
    /// </param>
    /// <param name="click">
    ///     The sound style played when the cursor clicks the element.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="InterfaceSounds"/> class with the specified sounds.
    /// </returns>
    public static InterfaceSounds FromSounds(in SoundStyle hover, in SoundStyle click) => new(hover, click);
}