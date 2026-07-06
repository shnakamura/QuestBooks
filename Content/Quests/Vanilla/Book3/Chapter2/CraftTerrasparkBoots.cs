using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter2;

public class CraftTerrasparkBoots : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftTerrasparkBootsCheck() : CraftItemHook<CraftTerrasparkBoots>(ItemID.TerrasparkBoots);
}