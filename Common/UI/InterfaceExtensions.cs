using Terraria.UI;

namespace QuestBooks.Common.UI;

/// <summary>
///     Provides <see cref="UIElement"/> extensions.
/// </summary>
public static class InterfaceExtensions
{
    public static TElement LeftClicked<TElement>(this TElement element, Action callback) where TElement : UIElement
    {
        element.OnLeftClick += (_, _) => callback.Invoke();
        
        return element;
    }

    public static TElement RightClicked<TElement>(this TElement element, Action callback) where TElement : UIElement
    {
        element.OnRightClick += (_, _) => callback.Invoke();
        
        return element;
    }
    
    public static TElement Padding<TElement>(this TElement element, float padding) where TElement : UIElement
    {
        element.PaddingTop = padding;
        element.PaddingLeft = padding;
        element.PaddingBottom = padding;
        element.PaddingRight = padding;
        
        return element;
    }
    
    public static TElement Center<TElement>(this TElement element) where TElement : UIElement => element.Align(0.5f);
    
    public static TElement Align<TElement>(this TElement element, float horizontal, float vertical) where TElement : UIElement
    {
        element.HAlign = horizontal;
        element.VAlign = vertical;
        
        return element;
    }

    public static TElement Align<TElement>(this TElement element, float percent) where TElement : UIElement => element.Align(percent, percent);
    
    public static TElement Fill<TElement>(this TElement element, float horizontal, float vertical) where TElement : UIElement
    {
        element.Width.Percent = horizontal;
        element.Height.Percent = vertical;
        
        return element;
    }
    
    public static TElement Fill<TElement>(this TElement element) where TElement : UIElement => element.Fill(1f);
    
    public static TElement Fill<TElement>(this TElement element, float percent) where TElement : UIElement => element.Fill(percent, percent);

    public static TElement Add<TElement, TChild>(this TElement element, TChild child) where TElement : UIElement where TChild : UIElement
    {
        element.Append(child);
        
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