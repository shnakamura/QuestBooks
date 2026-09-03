using Terraria.ModLoader.IO;

namespace QuestBooks.Common.Testing;

[Autoload(Side = ModSide.Client)]
public sealed class TestingSystem : ModSystem
{
    /// <summary>
    ///     Gets a value indicating whether testing is enabled.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if testing is enabled; otherwise, <see langword="false"/>.
    /// </value>
#if DEBUG
    public static bool Enabled { get; internal set; } = true;
#else
    public static bool Enabled { get; internal set; }
#endif
}