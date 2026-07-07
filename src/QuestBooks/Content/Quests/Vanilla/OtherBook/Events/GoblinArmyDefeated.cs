using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Events;

public class GoblinArmyDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedGoblins;
}