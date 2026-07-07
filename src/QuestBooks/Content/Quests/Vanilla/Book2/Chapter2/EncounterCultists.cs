using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter2;

public class EncounterCultists : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyoneNearCultists();
}