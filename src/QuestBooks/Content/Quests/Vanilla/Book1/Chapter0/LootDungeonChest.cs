using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter0;

public class LootDungeonChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootDungeonChestCheck() : LootChestHook<LootDungeonChest>(TileID.Containers, ChestFrames.LockedGold);
}