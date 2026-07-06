using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter1;

public class BuyRocketBoots : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuyRocketBootsCheck() : BuyItemHook<BuyRocketBoots>(ItemID.RocketBoots);
}