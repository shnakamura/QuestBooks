using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter2;

public class KillVortexPillar : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedTowerVortex;
}