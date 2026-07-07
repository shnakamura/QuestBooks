using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter0;

public class CraftAnvil : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftAnvilCheck() : CraftItemHook<CraftAnvil>(ItemID.IronAnvil);
}