namespace QuestBooks.Common.Inventory;

public static class InventoryDimensions
{
    private static float Scale => 0.85f;

    /// <summary>
    ///     The padding of the inventory, in pixels.
    /// </summary>
    public const int PADDING = 20;

    /// <summary>
    ///     Gets the width of the inventory, in pixels.
    /// </summary>
    public static int Width => (int)(560f * Scale);

    /// <summary>
    ///     Gets the height of the inventory, in pixels.
    /// </summary>
    public static int Height => (int)(280f * Scale);

    /// <summary>
    ///     Gets the bounds of the inventory, in screen coordinates.
    /// </summary>
    public static Rectangle Bounds => new(PADDING, PADDING, Width, Height);
}