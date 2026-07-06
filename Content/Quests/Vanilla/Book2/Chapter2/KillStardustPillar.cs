using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter2;

public class KillStardustPillar : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedTowerStardust;
}