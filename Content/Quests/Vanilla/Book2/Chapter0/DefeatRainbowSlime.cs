using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter0;

public class DefeatRainbowSlime : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillRainbowSlimeCheck() : KillNPCHook<DefeatRainbowSlime>(NPCID.RainbowSlime);
}