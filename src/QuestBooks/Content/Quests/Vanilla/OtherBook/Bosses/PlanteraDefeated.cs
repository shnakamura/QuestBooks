using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class PlanteraDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedPlantBoss;
}