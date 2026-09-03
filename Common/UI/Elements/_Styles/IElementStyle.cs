namespace QuestBooks.Common.UI;

public interface IElementStyle
{
    static abstract void Apply<TElement>(TElement element) where TElement : Element;
}