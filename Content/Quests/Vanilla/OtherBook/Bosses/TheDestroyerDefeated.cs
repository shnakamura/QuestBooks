using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class TheDestroyerDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMechBoss1;
}