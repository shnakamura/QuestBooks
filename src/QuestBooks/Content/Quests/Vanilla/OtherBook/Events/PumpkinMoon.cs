using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Events;

public class PumpkinMoonDefeated : VanillaQuest
{
    public override bool CheckCompletion() => Main.pumpkinMoon && NPC.waveNumber >= 15;
}

public class PumpkingDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class PumpkingCheck() : KillNPCHook<PumpkingDefeated>(NPCID.Pumpking);
}

public class MourningWoodDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class MourningWoodCheck() : KillNPCHook<MourningWoodDefeated>(NPCID.MourningWood);
}