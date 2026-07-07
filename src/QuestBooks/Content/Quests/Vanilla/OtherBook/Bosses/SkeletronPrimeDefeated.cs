using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class SkeletronPrimeDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMechBoss3;
}