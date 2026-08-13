using System.Linq;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public static class ElementExtensions
{
    public static bool TryInitialize<TElement>(this TElement element, bool recalculate = true) where TElement : UIElement
    {
        if (element.Children.Any())
        {
            return false;
        }
        
        element.OnInitialize();

        if (recalculate)
        {
            element.Recalculate();
        }

        return true;
    }
    
    public static bool TryClear<TElement>(this TElement element, bool recalculate = true) where  TElement : UIElement
    {
        if (!element.Children.Any())
        {
            return false;
        }
        
        element.RemoveAllChildren();

        if (recalculate)
        {
            element.Recalculate();
        }
        
        return true;
    }
}