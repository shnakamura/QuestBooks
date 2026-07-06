using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter0;

public class LootSurfaceChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootSurfaceChestCheck() : LootChestHook<LootSurfaceChest>(TileID.Containers, ChestFrames.Wood, ChestFrames.Gold);
}