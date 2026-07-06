using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter2;

public class CraftAsphalt : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftAsphaltCheck() : CraftItemHook<CraftAsphalt>(ItemID.AsphaltBlock);
}