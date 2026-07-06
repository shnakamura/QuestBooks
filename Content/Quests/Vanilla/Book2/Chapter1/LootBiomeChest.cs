using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter1;

public class LootBiomeChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootBiomeChestCheck() : LootChestHook<LootBiomeChest>
    (
        TileID.Containers,
        ChestFrames.DungeonCorruption,
        ChestFrames.DungeonCrimson,
        ChestFrames.DungeonHallow,
        ChestFrames.DungeonJungle,
        ChestFrames.DungeonTundra
    );
}