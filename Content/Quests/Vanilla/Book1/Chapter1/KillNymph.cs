using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class KillNymph : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class KillNymphCheck() : KillNPCHook<KillNymph>(NPCID.Nymph);
}