using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class EmpressOfLightDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedEmpressOfLight;
}