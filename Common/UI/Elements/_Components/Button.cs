namespace QuestBooks.Common.UI;

public sealed class Button : Element
{
    /// <summary>
    ///     Gets an empty button with full dimensions.
    /// </summary>
    public static Button Full = new Button().WithFullDimensions();
    
    /// <summary>
    ///     Gets an empty button.
    /// </summary>
    public static Button Empty => new();
    
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Attach(InterfaceSounds.FromSounds(in SoundID.MenuTick, in SoundID.MenuClose));
    }
}