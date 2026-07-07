using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter2;

public class BuyCrystalBall : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BuyCrystalBallCheck() : BuyItemHook<BuyCrystalBall>(ItemID.CrystalBall);
}