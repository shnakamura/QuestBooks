using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book0.Chapter0;

public class SurviveFirstNight : VanillaQuest
{
    public bool CachedNight { get; set; }

    public override bool CheckCompletion() => false;

    public override void Update()
    {
        var day = Main.IsItDay();

        if (day && CachedNight)
        {
            QuestBooksMod.CompleteQuest<SurviveFirstNight>();
        }

        CachedNight = !day;
    }
}