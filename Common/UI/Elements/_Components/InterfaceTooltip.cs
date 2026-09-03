using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public sealed class InterfaceTooltip : ElementComponent
{
    /// <summary>
    ///     Gets the callback used to retrieve the tooltip contents.
    /// </summary>
    public Func<string> Callback { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InterfaceTooltip"/> class with the specified localized text callback.
    /// </summary>
    /// <param name="callback">
    ///     A callback that returns the localized tooltip text.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    private InterfaceTooltip(Func<LocalizedText> callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Callback = () => callback.Invoke().Value;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="InterfaceTooltip"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     A callback that returns the tooltip contents.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    private InterfaceTooltip(Func<string> callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Callback = callback;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="InterfaceTooltip"/> class with the specified contents.
    /// </summary>
    /// <param name="contents">
    ///     The tooltip contents.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     <paramref name="contents"/> is <see langword="null"/> or empty.
    /// </exception>
    private InterfaceTooltip(string contents)
    {
        ArgumentException.ThrowIfNullOrEmpty(contents);

        Callback = () => contents;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InterfaceTooltip"/> class with the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized tooltip text.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    private InterfaceTooltip(LocalizedText text) : this(text.Value) => ArgumentNullException.ThrowIfNull(text);

    /// <inheritdoc/>
    public override void Attach(Element element)
    {
        base.Attach(element);   
        
        element.OnDraw += Draw;
    }

    /// <inheritdoc/>
    public override void Detach(Element element)
    {
        base.Detach(element);
        
        element.OnDraw -= Draw;
    }
    
    private void Draw(UIElement element)
    {
        if (!element.IsMouseHovering)
        {
            return;
        }
        
        Main.instance.MouseText(Callback.Invoke());
    }
    
    /// <summary>
    ///     Creates an instance of the <see cref="InterfaceTooltip"/> class with the specified localized text callback.
    /// </summary>
    /// <param name="callback">
    ///     A callback that returns the localized tooltip text.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="InterfaceTooltip"/> with the specified callback.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public static InterfaceTooltip FromCallback(Func<LocalizedText> callback) => new(callback);

    /// <summary>
    ///     Creates an instance of the <see cref="InterfaceTooltip"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     A callback that returns the tooltip contents.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="InterfaceTooltip"/> class with the specified callback.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public static InterfaceTooltip FromCallback(Func<string> callback) => new(callback);

    /// <summary>
    ///     Creates an instance of the <see cref="InterfaceTooltip"/> class with the specified contents.
    /// </summary>
    /// <param name="contents">
    ///     The contents of the tooltip.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="InterfaceTooltip"/> class with the specified contents.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     <paramref name="contents"/> is <see langword="null"/> or empty.
    /// </exception>
    public static InterfaceTooltip FromLiteral(string contents) => new(contents);

    /// <summary>
    ///     Creates an instance of the <see cref="InterfaceTooltip"/> class with the specified localization key.
    /// </summary>
    /// <param name="key">
    ///     The localization key of the tooltip.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="InterfaceTooltip"/> class with the localized text associated with the specified key.
    /// </returns>
    public static InterfaceTooltip FromKey(string key) => new(Language.GetTextValue(key));

    /// <summary>
    ///     Creates an instance of the <see cref="InterfaceTooltip"/> class with the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized text of the tooltip.
    /// </param>
    /// <returns>
    ///     An instance of the <see cref="InterfaceTooltip"/> class with the specified localized text.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    public static InterfaceTooltip FromLocalization(LocalizedText text) => new(text);
}