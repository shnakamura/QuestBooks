using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter0;

public class KillPinky : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    //public class KillPinkyCheck() : KillNPCHook<KillPinky>(NPCID.Pinky);
}