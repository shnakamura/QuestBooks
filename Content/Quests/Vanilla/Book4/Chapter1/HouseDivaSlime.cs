using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter1;

public class HouseDivaSlime : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimeRainbow);
}