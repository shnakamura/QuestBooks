using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter1;

public class HouseStylist : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Stylist);
}