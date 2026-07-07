using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class QueenBeeDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedQueenBee;
}