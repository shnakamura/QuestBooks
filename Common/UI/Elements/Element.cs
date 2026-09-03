using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public abstract class Element : UIElement
{
    private readonly Dictionary<Type, ElementComponent> componentsByType = [];
    private readonly List<ElementComponent> components = [];

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

    /// <summary>
    ///     Applies a style to the element.
    /// </summary>
    /// <typeparam name="TStyle">
    ///     The type of the style to apply.
    /// </typeparam>
    public void Style<TStyle>() where TStyle : IElementStyle => TStyle.Apply(this);
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
