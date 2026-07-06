using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Events;

public class PiratesDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedPirates;
}