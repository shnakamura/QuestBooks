using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Events;

public class MartianInvasionDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMartians;
}