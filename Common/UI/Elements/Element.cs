using System.Collections.Generic;
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

public static class ElementExtensions
{
    public static TElement WithUpdateCallback<TElement>(this TElement element, Action callback) where TElement : UIElement
    {
        element.OnUpdate += _ => callback.Invoke();

        return element;
    }

    public static TElement WithUpdateCallback<TElement>(this TElement element, Action<TElement> callback) where TElement : UIElement => element.WithUpdateCallback(() => callback.Invoke(element));

    public static TElement WithLeftClickCallback<TElement>(this TElement element, Action callback) where TElement : UIElement
    {
        element.OnLeftClick += (_, _) => callback.Invoke();

        return element;
    }

    public static TElement WithLeftClickCallback<TElement>(this TElement element, Action<TElement> callback) where TElement : UIElement => element.WithLeftClickCallback(() => callback.Invoke(element));

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
    ///     Sets the width of the specified element.
    /// </summary>
    /// <remarks>
    ///     <paramref name="percent" /> is a value in the range of <c>[0f - 1f]</c>, where <c>0f</c>
    ///     represents left alignment and <c>1f</c> represents right alignment.
    /// </remarks>
    /// <param name="element">
    ///     The element to set the width of.
    /// </param>
    /// <param name="pixels">
    ///     The width to set, in pixels.
    /// </param>
    /// <param name="percent">
    ///     The width to set, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the width of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithWidth<TElement>(this TElement element, float pixels, float percent) where TElement : UIElement => element.WithWidth(StyleDimension.FromPixelsAndPercent(pixels, percent));

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
    ///     Sets the height of the specified element.
    /// </summary>
    /// <remarks>
    ///     <paramref name="percent" /> is a value in the range of <c>[0f - 1f]</c>, where <c>0f</c>
    ///     represents top alignment and <c>1f</c> represents bottom alignment.
    /// </remarks>
    /// <param name="element">
    ///     The element to set the height of.
    /// </param>
    /// <param name="pixels">
    ///     The height to set, in pixels.
    /// </param>
    /// <param name="percent">
    ///     The height to set, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the height of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithHeight<TElement>(this TElement element, float pixels, float percent) where TElement : UIElement => element.WithHeight(StyleDimension.FromPixelsAndPercent(pixels, percent));

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
    ///     Sets the top position of the specified element.
    /// </summary>
    /// <remarks>
    ///     <paramref name="percent" /> is a value in the range of <c>[0f - 1f]</c>, where <c>0f</c>
    ///     represents top alignment and <c>1f</c> represents bottom alignment.
    /// </remarks>
    /// <param name="element">
    ///     The element to set the top position of.
    /// </param>
    /// <param name="pixels">
    ///     The top position to set, in pixels.
    /// </param>
    /// <param name="percent">
    ///     The top position to set, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the top position of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithTop<TElement>(this TElement element, float pixels, float percent) where TElement : UIElement => element.WithTop(StyleDimension.FromPixelsAndPercent(pixels, percent));

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
    ///     Sets the left position of the specified element.
    /// </summary>
    /// <remarks>
    ///     <paramref name="percent" /> is a value in the range of <c>[0f - 1f]</c>, where <c>0f</c>
    ///     represents left alignment and <c>1f</c> represents right alignment.
    /// </remarks>
    /// <param name="element">
    ///     The element to set the left position of.
    /// </param>
    /// <param name="pixels">
    ///     The left position to set, in pixels.
    /// </param>
    /// <param name="percent">
    ///     The left position to set, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the left position of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithLeft<TElement>(this TElement element, float pixels, float percent) where TElement : UIElement => element.WithLeft(StyleDimension.FromPixelsAndPercent(pixels, percent));

    /// <summary>
    ///     Sets the horizontal alignment of the specified element.
    /// </summary>
    /// <remarks>
    ///     <paramref name="percent" /> is a value in the range of <c>[0f - 1f]</c>, where <c>0f</c>
    ///     represents left alignment and <c>1f</c> represents right alignment.
    /// </remarks>
    /// <param name="element">
    ///     The element to set the horizontal alignment of.
    /// </param>
    /// <param name="percent">
    ///     The horizontal alignment to set, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the horizontal alignment of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithHAlign<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.HAlign = percent;

        return element;
    }

    /// <summary>
    ///     Sets the vertical alignment of the specified element.
    /// </summary>
    /// <remarks>
    ///     <paramref name="percent" /> is a value in the range of <c>[0f - 1f]</c>, where <c>0f</c>
    ///     represents top alignment and <c>1f</c> represents bottom alignment.
    /// </remarks>
    /// <param name="element">
    ///     The element to set the vertical alignment of.
    /// </param>
    /// <param name="percent">
    ///     The vertical alignment to set, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the vertical alignment of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithVAlign<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.VAlign = percent;

        return element;
    }

    /// <summary>
    ///     Sets the padding of the specified element.
    /// </summary>
    /// <param name="element">
    ///     The element to set the padding of.
    /// </param>
    /// <param name="top">
    ///     The top padding to set, in pixels.
    /// </param>
    /// <param name="left">
    ///     The left padding to set, in pixels.
    /// </param>
    /// <param name="bottom">
    ///     The bottom padding to set, in pixels.
    /// </param>
    /// <param name="right">
    ///     The right padding to set, in pixels.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to set the padding of.
    /// </typeparam>
    /// <returns>
    ///     The specified element.
    /// </returns>
    public static TElement WithPadding<TElement>(this TElement element, float top, float left, float bottom, float right) where TElement : UIElement
    {
        element.PaddingTop = top;
        element.PaddingLeft = left;
        element.PaddingBottom = bottom;
        element.PaddingRight = right;

        return element;
    }

    public static TElement WithElement<TElement, TChild>(this TElement element, TChild child) where TElement : UIElement where TChild : UIElement
    {
        element.Append(child);

        return element;
    }

    public static TElement WithContent<TElement, TChild>(this TElement element, TChild child) where TElement : UIElement where TChild : UIElement
    {
        element.RemoveAllChildren();
        element.Append(child);

        child.Activate();

        return element;
    }
}