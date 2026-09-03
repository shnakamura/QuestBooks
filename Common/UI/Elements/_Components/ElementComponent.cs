namespace QuestBooks.Common.UI;

public abstract class ElementComponent
{
    /// <summary>
    ///     Occurs when the component is attached to an element.
    /// </summary>
    /// <param name="element">
    ///     The element to which the component is attached.
    /// </param>
    public virtual void Attach(Element element) { }
    
    /// <summary>
    ///     Occurs when the component is detached from an element.
    /// </summary>
    /// <param name="element">
    ///     The element from which the component is detached.
    /// </param>
    public virtual void Detach(Element element) { }
}

/// <summary>
///     Provides <see cref="ElementComponent"/> extensions.
/// </summary>
public static class ElementComponentExtensions
{
    /// <summary>
    ///     Attaches a component to the specified element.
    /// </summary>
    /// <param name="element">
    ///     The element to attach the component to.
    /// </param>
    /// <param name="component">
    ///     The component to attach.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to attach the component to.
    /// </typeparam>
    /// <typeparam name="TComponent">
    ///     The type of the component to attach.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithComponent<TElement, TComponent>(this TElement element, TComponent component) where TElement : Element where TComponent : ElementComponent
    {
        element.Attach(component);

        return element;
    }
}