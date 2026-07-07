using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter0;

public class BuyPiggyBank : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuyPiggyBankCheck() : BuyItemHook<BuyPiggyBank>(ItemID.PiggyBank);
}