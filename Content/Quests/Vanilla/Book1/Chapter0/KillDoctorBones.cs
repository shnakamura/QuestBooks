using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter0;

public class KillDoctorBones : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class KillDoctorBonesCheck() : KillNPCHook<KillDoctorBones>(NPCID.DoctorBones);
}