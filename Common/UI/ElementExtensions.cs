using System.Runtime.CompilerServices;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public static class ElementExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement Padding<TElement>(this TElement element, float padding) where TElement : UIElement
    {
        element.PaddingTop = padding;
        element.PaddingLeft = padding;
        element.PaddingBottom = padding;
        element.PaddingRight = padding;
        
        element.Recalculate();
        
        return element;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement Allign<TElement>(this TElement element, float horizontal, float vertical) where TElement : UIElement
    {
        element.HAlign = horizontal;
        element.VAlign = vertical;
        
        element.Recalculate();
        
        return element;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement Allign<TElement>(this TElement element) where TElement : UIElement => element.Allign(0.5f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement Allign<TElement>(this TElement element, float percent) where TElement : UIElement => element.Allign(percent, percent);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement Fill<TElement>(this TElement element, float horizontal, float vertical) where TElement : UIElement
    {
        element.Width.Percent = horizontal;
        element.Height.Percent = vertical;
        
        element.Recalculate();
        
        return element;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement Fill<TElement>(this TElement element) where TElement : UIElement => element.Fill(1f);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement Fill<TElement>(this TElement element, float percent) where TElement : UIElement => element.Fill(percent, percent);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement Pixels<TElement>(this TElement element, float width, float height) where TElement : UIElement
    {
        element.Width.Pixels = width;
        element.Height.Pixels = height;
        
        return element;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TElement Pixels<TElement>(this TElement element, float pixels) where TElement : UIElement => element.Pixels(pixels, pixels);
}