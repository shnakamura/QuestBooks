using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class DeerclopsDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedDeerclops;
}