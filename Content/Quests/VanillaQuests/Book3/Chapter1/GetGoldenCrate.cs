using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class GetGoldenCrate : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GetGoldenCrateCheck() : CatchFishHook<GetGoldenCrate>(ItemID.GoldenCrate);
}