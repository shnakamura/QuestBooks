using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter0;

public class DefeatWyvern : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillWyvernCheck() : KillNPCHook<DefeatWyvern>(NPCID.WyvernHead);
}