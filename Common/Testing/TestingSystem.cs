using Terraria.ModLoader.IO;

namespace QuestBooks.Common.Testing;

public sealed class TestingSystem : ModSystem
{
    private const string Tag = "TestingEnabled";

    private static bool enabled;

    /// <summary>
    ///     Gets a value indicating whether testing is enabled.
    /// </summary>
    public static bool Enabled
    {
        get => enabled;
        internal set
        {
            enabled = value;
            
            NetMessage.SendData(MessageID.WorldData);
        }
    }

    public override void SaveWorldData(TagCompound tag) => tag[Tag] = Enabled;

    public override void LoadWorldData(TagCompound tag) => Enabled = tag.ContainsKey(Tag) && tag.GetBool(Tag);
}