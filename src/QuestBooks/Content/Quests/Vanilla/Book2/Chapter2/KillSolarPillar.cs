using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter2;

public class KillSolarPillar : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedTowerSolar;
}