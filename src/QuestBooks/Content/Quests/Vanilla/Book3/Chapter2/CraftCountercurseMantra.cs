using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter2;

public class CraftCountercurseMantra : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftCountercurseMantraCheck() : CraftItemHook<CraftCountercurseMantra>(ItemID.CountercurseMantra);
}