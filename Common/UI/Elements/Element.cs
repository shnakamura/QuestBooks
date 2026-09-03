using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public abstract class Element : UIElement
{
    private readonly Dictionary<Type, ElementComponent> componentsByType = [];
    private readonly List<ElementComponent> components = [];

    /// <summary>
    ///     Gets a read-only list of all components in the element.
    /// </summary>
    public IReadOnlyList<ElementComponent> Components => components;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Element" /> class.
    /// </summary>
    protected Element() { }

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        foreach (var component in components)
        {
            component.Detach(this);
        }

        components.Clear();
        componentsByType.Clear();
    }

    /// <summary>
    ///     Attaches a component to the element.
    /// </summary>
    /// <param name="component">
    ///     The component to attach.
    /// </param>
    /// <typeparam name="TComponent">
    ///     The type of the component to attach.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="component" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     A component of the same type already exists.
    /// </exception>
    public void Attach<TComponent>(TComponent component) where TComponent : ElementComponent
    {
        ArgumentNullException.ThrowIfNull(component);

        if (componentsByType.ContainsKey(typeof(TComponent)))
        {
            throw new InvalidOperationException();
        }

        component.Attach(this);

        components.Add(component);
        componentsByType.Add(typeof(TComponent), component);
    }

    /// <summary>
    ///     Detaches a component from the element.
    /// </summary>
    /// <param name="component">
    ///     The component to detach.
    /// </param>
    /// <typeparam name="TComponent">
    ///     The type of the component to detach.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="component" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     A component of the specified type does not exist.
    /// </exception>
    public void Detach<TComponent>(TComponent component) where TComponent : ElementComponent
    {
        ArgumentNullException.ThrowIfNull(component);

        if (!componentsByType.ContainsKey(typeof(TComponent)))
        {
            throw new InvalidOperationException();
        }

        component.Detach(this);

        components.Remove(component);
        componentsByType.Remove(typeof(TComponent));
    }
}

public static class ElementListExtensions
{
    public static TList WithScrollbar<TList, TScrollbar>(this TList list, TScrollbar scrollbar) where TList : UIList where TScrollbar : UIScrollbar
    {
        list.SetScrollbar(scrollbar);
        
        return list;
    }
    
    public static TList WithItem<TList, TItem>(this TList list, TItem item) where TList : UIList where TItem : UIElement
    {
        list.Add(item);
        
        return list;
    }
    
    public static TList WithSort<TList>(this TList list, Action<List<UIElement>> callback) where TList : UIList
    {
        list.ManualSortMethod = callback;
        
        return list;
    }

    public static TList WithGap<TList>(this TList list, float gap) where TList : UIList
    {
        list.ListPadding = gap;

        return list;
    }
}

public static class ElementCallbackExtensions
{
    public static TElement WithUpdateCallback<TElement>(this TElement element, Action callback) where TElement : UIElement
    {
        element.OnUpdate += (_) => callback.Invoke();

        return element;
    }

    public static TElement WithUpdateCallback<TElement>(this TElement element, Action<TElement> callback) where TElement : UIElement
    {
        element.OnUpdate += (_) => callback.Invoke(element);

        return element;
    }
    
    public static TElement WithLeftClickCallback<TElement>(this TElement element, Action callback) where TElement : UIElement
    {
        element.OnLeftClick += (_, _) => callback.Invoke();

        return element;
    }

    public static TElement WithLeftClickCallback<TElement>(this TElement element, Action<TElement> callback) where TElement : UIElement
    {
        element.OnLeftClick += (_, _) => callback.Invoke(element);
        
        return element;
    }
}

public static class ElementStyleExtensions
{
    public static TElement WithFullDimensions<TElement>(this TElement element) where TElement : UIElement
    {
        element.Width = StyleDimension.Fill;
        element.Height = StyleDimension.Fill;
        
        return element;
    }

    /// <summary>
    ///     Sets the width of the specified element.
    /// </summary>
    /// <param name="element">
    ///     The element to set the width of.
    /// </param>
    /// <param name="width">
    ///     The width to set.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the width of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithWidth<TElement>(this TElement element, StyleDimension width) where TElement : UIElement
    {
        element.Width = width;
        
        return element;
    }

    /// <summary>
    ///     Sets the height of the specified element.
    /// </summary>
    /// <param name="element">
    ///     The element to set the height of.
    /// </param>
    /// <param name="height">
    ///     The height to set.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the height of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithHeight<TElement>(this TElement element, StyleDimension height) where TElement : UIElement
    {
        element.Height = height;

        return element;
    }
    
    /// <summary>
    ///     Sets the top position of the specified element.
    /// </summary>
    /// <param name="element">
    ///     The element to set the top position of.
    /// </param>
    /// <param name="top">
    ///     The top position to set.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the top position of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithTop<TElement>(this TElement element, StyleDimension top) where TElement : UIElement
    {
        element.Top = top;

        return element;
    }

    /// <summary>
    ///     Sets the left position of the specified element.
    /// </summary>
    /// <param name="element">
    ///     The element to set the left position of.
    /// </param>
    /// <param name="left">
    ///     The left position to set.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the left position of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithLeft<TElement>(this TElement element, StyleDimension left) where TElement : UIElement
    {
        element.Left = left;

        return element;
    }

    /// <summary>
    ///     Sets the left padding of the specified element, in pixels.
    /// </summary>
    /// <param name="element">
    ///     The element to set the left padding of.
    /// </param>
    /// <param name="padding">
    ///     The left padding to set, in pixels.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the left padding of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithLeftPadding<TElement>(this TElement element, float padding) where TElement : UIElement
    {
        element.PaddingLeft = padding;

        return element;
    }
    
    /// <summary>
    ///     Sets the right padding of the specified element, in pixels.
    /// </summary>
    /// <param name="element">
    ///     The element to set the right padding of.
    /// </param>
    /// <param name="padding">
    ///     The right padding to set, in pixels.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the right padding of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithRightPadding<TElement>(this TElement element, float padding) where TElement : UIElement
    {
        element.PaddingRight = padding;

        return element;
    }
    
    /// <summary>
    ///     Sets the top padding of the specified element, in pixels.
    /// </summary>
    /// <param name="element">
    ///     The element to set the top padding of.
    /// </param>
    /// <param name="padding">
    ///     The top padding to set, in pixels.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the top padding of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithTopPadding<TElement>(this TElement element, float padding) where TElement : UIElement
    {
        element.PaddingTop = padding;

        return element;
    }
    
    /// <summary>
    ///     Sets the bottom padding of the specified element, in pixels.
    /// </summary>
    /// <param name="element">
    ///     The element to set the bottom padding of.
    /// </param>
    /// <param name="padding">
    ///     The bottom padding to set, in pixels.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the bottom padding of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithBottomPadding<TElement>(this TElement element, float padding) where TElement : UIElement
    {
        element.PaddingBottom = padding;

        return element;
    }
    
    public static TElement WithHAlign<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.HAlign = percent;

        return element;
    }

    public static TElement WithVAlign<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.VAlign = percent;

        return element;
    }
}

public static class ElementExtensions
{
    public static TElement WithElement<TElement, TChild>(this TElement element, TChild child) where TElement : UIElement where TChild : UIElement
    {
        element.Append(child);
        
        return element;
    }
    
    public static TElement WithReplacement<TElement, TChild>(this TElement element, TChild child) where TElement : UIElement where TChild : UIElement
    {
        element.RemoveAllChildren();
        element.Append(child);
        
        child.Activate();

        return element;
    }
}