using System.Runtime.CompilerServices;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public static class StyleExtensions
{
    /// <summary>
    ///     Gets the size of the calculated style.
    /// </summary>
    /// <param name="style">
    ///     The calculated style to get the size of.
    /// </param>
    /// <returns>
    ///     A <see cref="Vector2"/> representing the size of the calculated style.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Size(this CalculatedStyle style) => new(style.Width, style.Height);
}