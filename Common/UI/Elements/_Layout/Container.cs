namespace QuestBooks.Common.UI;

public sealed class Container : Element
{
    /// <summary>
    ///     Gets an empty container with full dimensions.
    /// </summary>
    public static Container Full => new Container().WithFullDimensions();

    /// <summary>
    ///     Gets an empty container.
    /// </summary>
    public static Container Empty => new();
}