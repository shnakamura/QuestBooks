using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class LootGoldenChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootGoldenChestCheck() : LootChestHook<LootGoldenChest>(TileID.Containers, ChestFrames.Gold);
}