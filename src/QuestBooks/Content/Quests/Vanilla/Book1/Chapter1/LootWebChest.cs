using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class LootWebChest : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class LootWebChestCheck() : LootChestHook<LootWebChest>(TileID.Containers, ChestFrames.Spider);
}