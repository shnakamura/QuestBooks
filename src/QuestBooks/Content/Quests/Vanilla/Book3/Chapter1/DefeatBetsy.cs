using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter1;

public class DefeatBetsy : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillBetsyCheck() : KillNPCHook<DefeatBetsy>(NPCID.DD2Betsy);
}