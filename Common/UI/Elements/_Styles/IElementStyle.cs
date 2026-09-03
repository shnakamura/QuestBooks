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