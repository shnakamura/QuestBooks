using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public static class UserInterfaceListExtensions
{
    public static TList WithSort<TList>(this TList list, Action<List<UIElement>> callback) where TList : UIList
    {
        list.ManualSortMethod = callback;
        
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
    public static TElement WithHorizontalAlignment<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.HAlign = percent;

        return element;
    }
    
    public static TElement WithVerticalAlignment<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.VAlign = percent;

        return element;
    }
    
    public static TElement WithAlignment<TElement>(this TElement element, float horizontal, float vertical) where TElement : UIElement
    {
        element.HAlign = horizontal;
        element.VAlign = vertical;
        
        return element;
    }

    public static TElement WithAlignment<TElement>(this TElement element, float percent) where TElement : UIElement => element.WithAlignment(percent, percent);
}

public static class UserInterfaceFillExtensions
{
    public static TElement WithHorizontalFill<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.Width.Percent = percent;
        
        return element;
    }
    
    public static TElement WithVerticalFill<TElement>(this TElement element, float percent) where TElement : UIElement
    {
        element.Height.Percent = percent;
        
        return element;
    }
    
    public static TElement WithFill<TElement>(this TElement element, float horizontal, float vertical) where TElement : UIElement
    {
        element.Width.Percent = horizontal;
        element.Height.Percent = vertical;
        
        return element;
    }
    
    public static TElement WithFill<TElement>(this TElement element, float percent) where TElement : UIElement => element.WithFill(percent, percent);
}

public static class UserInterfaceExtensions
{
    public static TElement WithElement<TElement, TChild>(this TElement element, TChild child) where TElement : UIElement where TChild : UIElement
    {
        element.Append(child);
        
        return element;
    }

    public static TElement WithPadding<TElement>(this TElement element, float padding) where TElement : UIElement
    {
        element.PaddingTop = padding;
        element.PaddingLeft = padding;
        element.PaddingBottom = padding;
        element.PaddingRight = padding;
        
        return element;
    }

    public static TElement WithWidth<TElement>(this TElement element, float pixels) where TElement : UIElement
    {
        element.Width.Pixels = pixels;
        
        return element;
    }
    
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