using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter2;

public class CraftStarinaBottle : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftStarinaBottleCheck() : CraftItemHook<CraftStarinaBottle>(ItemID.StarinaBottle);
}