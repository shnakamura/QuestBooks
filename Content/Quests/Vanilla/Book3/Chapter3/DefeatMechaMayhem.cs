using QuestBooks.Core.Quests;
using Terraria.GameContent.Achievements;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter3;

public class DefeatMechaMayhem : VanillaQuest
{
    public override void Load() => AchievementsHelper.OnProgressionEvent += Check;

    public override bool CheckCompletion() => false;

    private static void Check(int eventId)
    {
        if (eventId != AchievementHelperID.Events.DefeatedMechaMayhem)
        {
            return;
        }

        QuestBooksMod.MarkComplete<DefeatMechaMayhem>();
    }
}