using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class GolemDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedGolemBoss;
}