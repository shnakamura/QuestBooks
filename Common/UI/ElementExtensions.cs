using System.Linq;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public static class ElementExtensions
{
    public static bool TryInitialize(this UIElement element, bool recalculate = true)
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
    
    public static bool TryClear(this UIElement element, bool recalculate = true)
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