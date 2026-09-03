namespace QuestBooks.Common.UI;

public enum FlexDirection : byte
{
    /// <summary>
    ///     Elements will be aligned horizontally.
    /// </summary>
    Row,
    
    /// <summary>
    ///     Elements will be aligned vertically.
    /// </summary>
    Column
}

public enum FlexAlignment : byte
{
    /// <summary>
    ///     Elements will be aligned from the start of the flex.
    /// </summary>
    Start,
    
    /// <summary>
    ///     Elements will be aligned from the center of the flex.
    /// </summary>
    Center
}

public sealed class Flex : Element
{
    /// <summary>
    ///     Gets the direction of the flex.
    /// </summary>
    public FlexDirection Direction { get; init; }
    
    /// <summary>
    ///     Gets the alignment of the flex.
    /// </summary>
    public FlexAlignment Alignment { get; init; }
    
    private float _gap;
    
    /// <summary>
    ///     Gets or sets the gap between each element in the flex, in pixels.
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
    ///     Initializes a new instance of the <see cref="Flex"/> class with the specified direction and alignment.
    /// </summary>
    /// <param name="direction">
    ///     The direction of the flex.
    /// </param>
    /// <param name="alignment">
    ///     The alignment of the flex.
    /// </param>
    private Flex(FlexDirection direction, FlexAlignment alignment)
    {
        Direction = direction;
        Alignment = alignment;
    }

    /// <inheritdoc/>
    public override void Recalculate()
    {
        base.Recalculate();
        
        switch (Direction)
        {
            case FlexDirection.Row:
                RecalculateHorizontal();
                break;
            case FlexDirection.Column:
                RecalculateVertical();
                break;
        }
    }

    private void RecalculateHorizontal()
    {
        switch (Alignment)
        {
            case FlexAlignment.Start:
                RecalculateHorizontalFromStart();
                break;
            case FlexAlignment.Center:
                RecalculateHorizontalFromCenter();
                break;
        }
    }

    private void RecalculateHorizontalFromStart()
    {
        var offset = 0f;
        
        foreach (var element in Elements)
        {
            element.Left.Pixels = offset;

            offset += element.GetOuterDimensions().Width + Gap;
        }
    }

    private void RecalculateHorizontalFromCenter()
    {
        var count = Elements.Count;
        
        for (var i = 0; i < count; i++)
        {
            var element = Elements[i];
            var value = i / (float)(count - 1);
            
            if (float.IsNaN(value))
            {
                value = 0.5f;
            }

            element.HAlign = value;
        }
    }

    private void RecalculateVertical()
    {
        switch (Alignment)
        {
            case FlexAlignment.Start:
                RecalculateVerticalFromStart();
                break;
            case FlexAlignment.Center:
                RecalculateVerticalFromCenter();
                break;
        }
    }

    private void RecalculateVerticalFromStart()
    {
        var offset = 0f;
        
        foreach (var element in Elements)
        {
            element.Top.Pixels = offset;

            offset += element.GetOuterDimensions().Height + Gap;
        }
    }

    private void RecalculateVerticalFromCenter()
    {
        var count = Elements.Count;

        for (var i = 0; i < count; i++)
        {
            var element = Elements[i];
            var value = i / (float)(count - 1);

            if (float.IsNaN(value))
            {
                value = 0.5f;
            }
            
            element.VAlign = value;
        }
    }

    /// <summary>
    ///     Returns a new horizontal <see cref="Flex"/> with the specified alignment.
    /// </summary>
    /// <param name="alignment">
    ///     The alignment of the flex.
    /// </param>
    /// <returns>
    ///     A new horizontal <see cref="Flex"/> with the specified alignment.
    /// </returns>
    public static Flex FromHorizontal(FlexAlignment alignment) => new(FlexDirection.Row, alignment);

    /// <summary>
    ///     Returns a new vertical <see cref="Flex"/> with the specified alignment.
    /// </summary>
    /// <param name="alignment">
    ///     The alignment of the flex.
    /// </param>
    /// <returns>
    ///     A new vertical <see cref="Flex"/> with the specified alignment.
    /// </returns>
    public static Flex FromVertical(FlexAlignment alignment) => new(FlexDirection.Column, alignment);
}

/// <summary>
///     Provides <see cref="Flex"/> extensions.
/// </summary>
public static class FlexExtensions
{
    public static Flex WithGap(this Flex flex, float gap)
    {
        flex.Gap = gap;
        
        return flex;
    }
}