using Terraria.Localization;

namespace QuestBooks.Common.UI.Elements;

public readonly record struct ElementTooltipSettings
{
    /// <summary>
    ///     Gets the text of the element tooltip.
    /// </summary>
    public readonly string Text { get; }
    
    /// <summary>
    ///     Gets a value indicating whether the element tooltip is enabled.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the element tooltip is enabled; otherwise, <see langword="false"/>.
    /// </value>
    public readonly bool Enabled { get; }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="ElementTooltipSettings"/> struct with the specified text.
    /// </summary>
    /// <param name="text">
    ///     The text of the element tooltip.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     <paramref name="text"/> is <see langword="null"/> or empty.
    /// </exception>
    private ElementTooltipSettings(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);

        Text = text;
        Enabled = true;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ElementTooltipSettings"/> struct with the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized text of the element tooltip.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    private ElementTooltipSettings(LocalizedText text) : this(text.Value) => ArgumentNullException.ThrowIfNull(text);

    /// <summary>
    ///     Creates a new instance of the <see cref="ElementTooltipSettings"/> class from the specified contents.
    /// </summary>
    /// <param name="text">
    ///     The contents of the tooltip.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="ElementTooltipSettings"/> class with the specified contents.
    /// </returns>
    public static ElementTooltipSettings FromLiteral(string text) => new(text);

    /// <summary>
    ///     Creates a new instance of the <see cref="ElementTooltipSettings"/> class from the specified localization key.
    /// </summary>
    /// <param name="key">
    ///     The localization key of the tooltip.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="ElementTooltipSettings"/> class with the specified localization key.
    /// </returns>
    public static ElementTooltipSettings FromKey(string key) => new(Language.GetText(key));

    /// <summary>
    ///     Creates a new instance of the <see cref="ElementTooltipSettings"/> class from the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized text of the tooltip.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="ElementTooltipSettings"/> class with the specified localized text.
    /// </returns>
    public static ElementTooltipSettings FromLocalization(LocalizedText text) => new(text);
}

public static class ElementTooltipSettingsExtensions
{
    public static TElement WithTooltip<TElement>(this TElement element, ElementTooltipSettings settings) where TElement : Element
    {
        element.Tooltip = settings;
        
        return element;
    }
}