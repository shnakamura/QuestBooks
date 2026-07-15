using Terraria.ModLoader.IO;

namespace QuestBooks.Common.Testing;

[Autoload(Side = ModSide.Client)]
public sealed class TestingSystem : ModSystem
{
    private const string Tag = "TestingEnabled";
    
    /// <summary>
    ///     Gets or sets a value indicating whether testing is enabled.
    /// </summary>
    public static bool Enabled { get; internal set; }

    public override void SaveWorldData(TagCompound tag) => tag[Tag] = Enabled;

    public override void LoadWorldData(TagCompound tag) => Enabled = tag.ContainsKey(Tag) && tag.GetBool(Tag);
}