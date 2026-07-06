using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter1;

public class HouseDemolitionist : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Demolitionist);
}