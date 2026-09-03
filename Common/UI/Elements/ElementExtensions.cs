using Terraria.UI;

namespace QuestBooks.Common.UI;

public static class ElementExtensions
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
    
    /// <summary>
    ///     Sets the horizontal alignment of the specified element, in percent.
    /// </summary>
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
    ///     Sets the vertical alignment of the specified element, in percent.
    /// </summary>
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