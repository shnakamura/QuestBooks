namespace QuestBooks.Common.UI;

public sealed class ElementListScrollbar : Element
{
    /// <summary>
    ///     Gets an empty scrollbar with full dimensions.
    /// </summary>
    public static ElementListScrollbar Full => new ElementListScrollbar().WithFullDimensions();
    
    /// <summary>
    ///     Gets an empty scrollbar.
    /// </summary>
    public static ElementListScrollbar Empty => new();
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="ElementListScrollbar"/> class.
    /// </summary>
    private ElementListScrollbar() { }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        
    }
}

public static class ElementListScrollbarUtilities
{
    private static SpriteBatch Batch => Main.spriteBatch;

    public static void Draw(Texture2D texture, Rectangle dimensions, Rectangle frame, in Color color)
    {
        Batch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6), new Rectangle(0, 0, texture.Width, 6), color);
        Batch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), new Rectangle(0, 6, texture.Width, 4), color);
        Batch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6), new Rectangle(0, texture.Height - 6, texture.Width, 6), color);
    }
}