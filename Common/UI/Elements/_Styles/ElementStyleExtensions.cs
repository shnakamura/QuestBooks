namespace QuestBooks.Common.UI;

public static class ElementStyleExtensions
{
    public static TElement WithStyle<TElement, TStyle>(this TElement element) where TElement : Element where TStyle : IElementStyle
    {
        element.Style<TStyle>();
        
        return element;
    }
}