using System.Linq;
using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter1;

public class DefeatDarkMage : VanillaQuest
{
    // Not affected by other mods, does not need to be a set
    public static readonly int[] DarkMageTypes =
    [
        NPCID.DD2DarkMageT1,
        NPCID.DD2DarkMageT3
    ];

    public override bool CheckCompletion() => false;

    public sealed class KillDarkMageCheck() : KillNPCHook<DefeatDarkMage>(npc => DarkMageTypes.Contains(npc.type));
}