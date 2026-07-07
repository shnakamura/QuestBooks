using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class LunaticCultistDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedAncientCultist;
}