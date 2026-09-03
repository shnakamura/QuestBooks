using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameInput;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public sealed class ElementList<TElement> : Element, IEnumerable<TElement> where TElement : UIElement
{
    /// <summary>
    ///     Gets an empty element list with full dimensions.
    /// </summary>
    public static ElementList<TElement> Full => new ElementList<TElement>().WithFullDimensions();

    /// <summary>
    ///     Gets an empty element list.
    /// </summary>
    public static ElementList<TElement> Empty => new();
    
    private IComparer<TElement> _comparer = Comparer<TElement>.Default;
    
    private float scroll;
    
    private float _gap;
    
    private readonly List<TElement> items = [];

    /// <summary>
    ///     Gets or sets the comparer used to sort the elements in the list.
    /// </summary>
    public IComparer<TElement> Comparer
    {
        get => _comparer;
        set
        {
            _comparer = value;
            
            Sort();
        }
    }

    /// <summary>
    ///     Gets or sets the scroll offset of the list, in pixels.
    /// </summary>
    public float Scroll
    {
        get => scroll;
        set
        {
            scroll = MathHelper.Clamp(value, 0f, Overflow);
            
            Recalculate();
        }
    }
    
    /// <summary>
    ///     Gets or sets the gap between each element in the list, in pixels.
    /// </summary>
    public float Gap 
    {
        get => _gap;
        set
        {
            _gap = value;
            
            Recalculate();
        }
    }

    /// <summary>
    ///     Gets the number of elements in the list.
    /// </summary>
    public int Count => items.Count;
    
    /// <summary>
    ///     Gets the amount of overflow in the list, in pixels.
    /// </summary>
    public float Overflow => MathF.Max(0f, items.Sum(item => item.GetOuterDimensions().Height) + MathF.Max(0f, items.Count - 1f) * Gap - GetInnerDimensions().Height);
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="ElementList{TElement}"/> class.
    /// </summary>
    private ElementList() => OverflowHidden = true;

    /// <summary>
    ///     Gets the element at the specified index.
    /// </summary>
    /// <param name="index">
    ///     The index of the element to get.
    /// </param>
    public TElement this[int index] => items[index];
    
    /// <inheritdoc/>
    public override void Recalculate()
    {
        base.Recalculate();
        
        var offset = 0f;
        
        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var gap = i < items.Count - 1 ? Gap : 0f;
            
            item.Top.Set(offset - Scroll, 0f);
            
            offset += item.GetOuterDimensions().Height + gap;
        }
    }

    /// <inheritdoc/>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!IsMouseHovering)
        {
            return;
        }
        
        PlayerInput.LockVanillaMouseScroll("ModLoader/UIList");
    }

    /// <inheritdoc/>
    public override void ScrollWheel(UIScrollWheelEvent evt)
    {
        base.ScrollWheel(evt);

        Scroll -= evt.ScrollWheelValue;
    }
    
    /// <summary>
    ///     Adds an element to the list.
    /// </summary>
    /// <param name="element">
    ///     The element to add.
    /// </param>
    public void Add(TElement element)
    {
        items.Add(element);
        
        Sort();
        
        Append(element);
    }
    
    /// <summary>
    ///     Removes an element from the list.
    /// </summary>
    /// <param name="element">
    ///     The element to remove.
    /// </param>
    public void Remove(TElement element)
    {
        items.Remove(element);

        Sort();
        
        RemoveChild(element);
    }

    /// <summary>
    ///     Removes all elements from the list.
    /// </summary>
    public void Clear()
    {
        items.Clear();
        
        RemoveAllChildren();
    }

    /// <summary>
    ///     Sorts the elements in the list.
    /// </summary>
    public void Sort() => items.Sort(Comparer);

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    
    /// <inheritdoc/>
    public IEnumerator<TElement> GetEnumerator() => items.GetEnumerator();
}

public static class BExtensions
{
    public static ElementList<TElement> WithComparer<TElement>(this ElementList<TElement> list, IComparer<TElement> comparer) where TElement : Element
    {
        list.Comparer = comparer;
        
        return list;
    }
    
    public static ElementList<TElement> WithItem<TElement>(this ElementList<TElement> list, TElement element) where TElement : Element
    {
        list.Add(element);
        
        return list;
    }

    public static ElementList<TElement> WithGap<TElement>(this ElementList<TElement> list, float gap) where TElement : Element
    {
        list.Gap = gap;
        
        return list;
    }
}