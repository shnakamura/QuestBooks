using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter0;

public class DefeatGoblinSummoner : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillGoblinSummonerCheck() : KillNPCHook<DefeatGoblinSummoner>(NPCID.GoblinSummoner);
}