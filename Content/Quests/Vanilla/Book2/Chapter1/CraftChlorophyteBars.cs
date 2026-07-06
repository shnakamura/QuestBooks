using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter1;

public class CraftChlorophyteBars : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftChlorophyteBarsCheck() : CraftItemHook<CraftChlorophyteBars>(ItemID.ChlorophyteBar);
}