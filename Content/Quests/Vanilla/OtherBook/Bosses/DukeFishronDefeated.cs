using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class DukeFishronDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedFishron;
}