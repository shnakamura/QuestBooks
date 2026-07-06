using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter1;

public class BuyBugNet : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuyNetCheck() : BuyItemHook<BuyBugNet>(ItemID.BugNet);
}