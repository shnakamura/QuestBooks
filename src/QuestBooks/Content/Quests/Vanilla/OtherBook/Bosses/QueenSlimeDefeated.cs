using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class QueenSlimeDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedQueenSlime;
}