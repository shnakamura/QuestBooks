using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class EyeOfCthulhuDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedBoss1;
}