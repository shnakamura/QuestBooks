using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter0;

public class DefeatAnyMech : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMechBossAny;
}