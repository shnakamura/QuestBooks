using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class SkeletronDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedBoss3;
}