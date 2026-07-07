using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class KingSlimeDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedSlimeKing;
}