using System.Collections.Generic;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public sealed class VerticalStack : Element
{
    private List<UIElement> layout = new();
    
    /// <summary>
    ///     Gets a read-only list of all elements in the stack's layout.
    /// </summary>
    public IReadOnlyList<UIElement> Layout => layout;

    /// <summary>
    ///     Gets the mode of the stack.
    /// </summary>
    public StackMode Mode { get; init; } = StackMode.Offset;
    
    /// <summary>
    ///     Gets the vertical gap between each element in the stack, in pixels.
    /// </summary>
    public float Gap { get; init; }

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

        switch (Mode)
        {
            case StackMode.Offset:
                StackOffset();
                break;
            case StackMode.Evenly:
                StackEvenly();
                break;
        }
    }

    private void StackOffset()
    {
        var offset = 0f;
        
        foreach (var element in Layout)
        {
            element.Top.Set(offset, 0f);

            offset += element.GetOuterDimensions().Height;
            offset += Gap;
            
            element.Recalculate();
        }
    }

    private void StackEvenly()
    {
        var count = Layout.Count;

        for (var i = 0; i < count; i++)
        {
            var element = Layout[i];
            
            element.VAlign = i / (float)(count - 1);
            element.Recalculate();
        }
    }
}