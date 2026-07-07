using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class KillUndeadMiner : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class KillUndeadMinerCheck() : KillNPCHook<KillUndeadMiner>(NPCID.UndeadMiner);
}