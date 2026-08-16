using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public static class UserInterfaceListExtensions
{
    public static TList WithScrollbar<TList, TScrollbar>(this TList list, TScrollbar scrollbar) where TList : UIList where TScrollbar : UIScrollbar
    {
        list.SetScrollbar(scrollbar);
        
        return list;
    }
    
    public static TList WithSort<TList>(this TList list, Action<List<UIElement>> callback) where TList : UIList
    {
        list.ManualSortMethod = callback;
        
        return list;
    }

    public static TList WithHiddenOverflow<TList>(this TList list, bool value) where TList : UIList
    {
        list.OverflowHidden = value;
        
        return list;
    }
}

public static class UserInterfaceInputExtensions
{
    public static TElement WithRightClickEvent<TElement>(this TElement element, UIElement.MouseEvent callback) where TElement : UIElement
    {
        element.OnRightClick += callback;
        
        return element;
    }
    
    public static TElement WithRightClickEvent<TElement>(this TElement element, Action callback) where TElement : UIElement => element.WithRightClickEvent((_, _) => callback());
    
    public static TElement WithLeftClickEvent<TElement>(this TElement element, UIElement.MouseEvent callback) where TElement : UIElement
    {
        element.OnLeftClick += callback;
        
        return element;
    }
    
    public static TElement WithLeftClickEvent<TElement>(this TElement element, Action callback) where TElement : UIElement => element.WithLeftClickEvent((_, _) => callback());
}

public static class UserInterfaceAlignmentExtensions
{
    /// <summary>
    ///     Sets the horizontal alignment of the element, in percent.
    /// </summary>
    /// <param name="element">
    ///     The element to set the horizontal alignment of.
    /// </param>
    /// <param name="percent">
    ///     The horizontal alignment of the element, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified horizontal alignment.
    /// </returns>
    public static TElement WithHorizontalAlignment<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.HAlign = percent;

        return element;
    }
    
    /// <summary>
    ///     Sets the vertical alignment of the element, in percent.
    /// </summary>
    /// <param name="element">
    ///     The element to set the vertical alignment of.
    /// </param>
    /// <param name="percent">
    ///     The vertical alignment of the element, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified vertical alignment.
    /// </returns>
    public static TElement WithVerticalAlignment<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.VAlign = percent;

        return element;
    }
    
    /// <summary>
    ///     Sets the horizontal and vertical alignment of the element, in percent.
    /// </summary>
    /// <param name="element">
    ///     The element to set the horizontal and vertical alignment of.
    /// </param>
    /// <param name="horizontal">
    ///     The horizontal alignment of the element, in percent.
    /// </param>
    /// <param name="vertical">
    ///     The vertical alignment of the element, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified horizontal and vertical alignment.
    /// </returns>
    public static TElement WithAlignment<TElement>(this TElement element, float horizontal, float vertical) where TElement : UIElement
    {
        element.HAlign = horizontal;
        element.VAlign = vertical;
        
        return element;
    }

    /// <summary>
    ///     Sets the horizontal and vertical alignment of the element, in percent.
    /// </summary>
    /// <param name="element">
    ///     The element to set the horizontal and vertical alignment of.
    /// </param>
    /// <param name="percent">
    ///     The horizontal and vertical alignment of the element, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified horizontal and vertical alignment.
    /// </returns>
    public static TElement WithAlignment<TElement>(this TElement element, float percent) where TElement : UIElement => element.WithAlignment(percent, percent);
}

public static class UserInterfaceFillExtensions
{
    /// <summary>
    ///     Sets the width of the element, in percent.
    /// </summary>
    /// <param name="element">
    ///     The element to set the width of.
    /// </param>
    /// <param name="percent">
    ///     The width of the element, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified width.
    /// </returns>
    public static TElement WithHorizontalFill<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.Width.Percent = percent;
        
        return element;
    }
    
    /// <summary>
    ///     Sets the height of the element, in percent.
    /// </summary>
    /// <param name="element">
    ///     The element to set the height of.
    /// </param>
    /// <param name="percent">
    ///     The height of the element, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified height.
    /// </returns>
    public static TElement WithVerticalFill<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.Height.Percent = percent;
        
        return element;
    }
    
    /// <summary>
    ///     Sets the width and height of the element, in percent.
    /// </summary>
    /// <param name="element">
    ///     The element to set the width and height of.
    /// </param>
    /// <param name="horizontal">
    ///     The width of the element, in percent.
    /// </param>
    /// <param name="vertical">
    ///     The height of the element, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified width and height.
    /// </returns>
    public static TElement WithFill<TElement>(this TElement element, float horizontal, float vertical) where TElement : UIElement
    {
        element.Width.Percent = horizontal;
        element.Height.Percent = vertical;
        
        return element;
    }
    
    /// <summary>
    ///     Sets the width and height of the element, in percent.
    /// </summary>
    /// <param name="element">
    ///     The element to set the width and height of.
    /// </param>
    /// <param name="percent">
    ///     The width and height of the element, in percent.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified width and height.
    /// </returns>
    public static TElement WithFill<TElement>(this TElement element, float percent) where TElement : UIElement => element.WithFill(percent, percent);
}

public static class UserInterfaceExtensions
{
    /// <summary>
    ///     Appends a child to the specified element.
    /// </summary>
    /// <param name="element">
    ///     The element to append the child to.
    /// </param>
    /// <param name="child">
    ///     The child to append to the specified element.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <typeparam name="TChild">
    ///     The type of the child element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified child.
    /// </returns>
    public static TElement WithElement<TElement, TChild>(this TElement element, TChild child) where TElement : UIElement where TChild : UIElement
    {
        element.Append(child);
        
        return element;
    }

    /// <summary>
    ///     Sets the width of the element, in pixels.
    /// </summary>
    /// <param name="element">
    ///     The element to set the width of.
    /// </param>
    /// <param name="pixels">
    ///     The width of the element, in pixels.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified width.
    /// </returns>
    public static TElement WithWidth<TElement>(this TElement element, float pixels) where TElement : UIElement
    {
        element.Width.Pixels = pixels;
        
        return element;
    }
    
    /// <summary>
    ///     Sets the height of the element, in pixels.
    /// </summary>
    /// <param name="element">
    ///     The element to set the height of.
    /// </param>
    /// <param name="pixels">
    ///     The height of the element, in pixels.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element.
    /// </typeparam>
    /// <returns>
    ///     The element with the specified height.
    /// </returns>
    public static TElement WithHeight<TElement>(this TElement element, float pixels) where TElement : UIElement
    {
        element.Height.Pixels = pixels;

        return element;
    }
}

/// <summary>
///     Provides <see cref="CalculatedStyle"/> extensions.
/// </summary>
public static class CalculatedStyleExtensions
{
    /// <summary>
    ///     Gets the size of the calculated style.
    /// </summary>
    /// <param name="style">
    ///     The calculated style to get the size of.
    /// </param>
    /// <returns>
    ///     A <see cref="Vector2"/> representing the size of the calculated style.
    /// </returns>
    public static Vector2 Size(this CalculatedStyle style) => new(style.Width, style.Height);
}