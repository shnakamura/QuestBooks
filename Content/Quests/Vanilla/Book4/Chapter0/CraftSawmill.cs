using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter0;

public class CraftSawmill : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftSawmillCheck() : CraftItemHook<CraftSawmill>(ItemID.Sawmill);
}