using System.Collections.Generic;
using Terraria.UI;

namespace QuestBooks.Common.UI.Layout;

public sealed class JustifiedVerticalStack : UIElement
{
    private List<UIElement> layout = new();
    
    /// <summary>
    ///     Gets a read-only list of all elements in the stack's layout.
    /// </summary>
    public IReadOnlyList<UIElement> Layout => layout;
    
    /// <summary>
    ///     Adds the specified element to the stack's layout.
    /// </summary>
    /// <param name="element">
    ///     The element to add.
    /// </param>
    /// <typeparam name="TElement">
    ///     The type of the element to add.
    /// </typeparam>
    public void Add<TElement>(TElement element) where TElement : UIElement
    {
        Append(element);
        
        layout.Add(element);
    }
    
    public override void Recalculate()
    {
        base.Recalculate();

        var count = Layout.Count;

        for (var i = 0; i < count; i++)
        {
            var child = Layout[i];
            
            child.VAlign = i / (float)(count - 1);
            child.Recalculate();
        }
    }
}