using Terraria.Audio;

namespace QuestBooks.Common.UI.Elements;

public readonly record struct ElementSoundSettings
{
    /// <summary>
    ///     The default click sound style of elements.
    /// </summary>
    public static readonly SoundStyle DEFAULT_CLICK_SOUND = SoundID.MenuOpen;
    
    /// <summary>
    ///     The default hover sound style of elements.
    /// </summary>
    public static readonly SoundStyle DEFAULT_HOVER_SOUND = SoundID.MenuTick;

    public static ElementSoundSettings Default { get; } = new(in DEFAULT_CLICK_SOUND, in DEFAULT_HOVER_SOUND);
    
    /// <summary>
    ///     Gets the sound style played when the cursor clicks the element.
    /// </summary>
    public SoundStyle Click { get; }
    
    /// <summary>
    ///     Gets the sound style played when the cursor hovers over the element.
    /// </summary>
    public SoundStyle Hover { get; }
    
    /// <summary>
    ///     Gets a value indicating whether element sounds are enabled.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if element sounds are enabled; otherwise, <see langword="false"/>.
    /// </value>
    public bool Enabled { get; init; } = true;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ElementSoundSettings"/> struct with the specified click and hover sound styles.
    /// </summary>
    /// <param name="click">
    ///     The sound style played when the cursor clicks the element.
    /// </param>
    /// <param name="hover">
    ///     The sound style played when the cursor hovers over the element.
    /// </param>
    private ElementSoundSettings(in SoundStyle click, in SoundStyle hover)
    {
        Click = click;
        Hover = hover;
        
        Enabled = true;
    }

    public static ElementSoundSettings FromSounds(in SoundStyle click, in SoundStyle hover) => new(click, hover);
}

public static class ElementSoundSettingsExtensions
{
    public static TElement WithSounds<TElement>(this TElement element, ElementSoundSettings settings) where TElement : Element
    {
        element.Sounds = settings;
        
        return element;
    }
    
    public static TElement WithDefaultSounds<TElement>(this TElement element) where TElement : Element => element.WithSounds(ElementSoundSettings.Default);
}