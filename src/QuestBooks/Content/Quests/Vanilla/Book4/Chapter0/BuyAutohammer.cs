using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter0;

public class BuyAutohammer : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuyAutohammerCheck() : BuyItemHook<BuyAutohammer>(ItemID.Autohammer);
}