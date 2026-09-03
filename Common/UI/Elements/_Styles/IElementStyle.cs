namespace QuestBooks.Common.UI;

public interface IElementStyle
{
    /// <summary>
    ///     Applies the style to the specified element.
    /// </summary>
    /// <param name="element">
    ///     The element to apply the style to.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to apply the style to.
    /// </typeparam>
    static abstract void Apply<TElement>(TElement element) where TElement : Element;
}

public static class ElementStyleExtensions
{
    /// <summary>
    ///     Applies the specified style to the element.
    /// </summary>
    /// <param name="element">
    ///     The element to apply the style to.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <typeparam name="TStyle">
    ///     The type of the style to apply.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified style applied.
    /// </returns>
    public static TElement WithStyle<TElement, TStyle>(this TElement element) where TElement : Element where TStyle : IElementStyle
    {
        element.Style<TStyle>();

        return element;
    }
}
