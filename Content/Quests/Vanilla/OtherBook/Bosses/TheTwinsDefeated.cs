using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class TheTwinsDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMechBoss2;
}