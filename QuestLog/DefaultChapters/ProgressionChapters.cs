using QuestBooks.Content.Quests.Vanilla.Book0.Chapter0;
using QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;
using QuestBooks.Core.Quests;

namespace QuestBooks.QuestLog.DefaultChapters;

public class PostIntroChapter : ScrollChapter
{
    public override bool IsUnlocked() => QuestManager.GetQuest<BasicsCompleteInfo>().Completed;
}

public class HardmodeChapter : ScrollChapter
{
    public override bool IsUnlocked() => QuestManager.GetQuest<WallOfFleshDefeated>().Completed;
}

public class EndgameChapter : ScrollChapter
{
    public override bool IsUnlocked() => QuestManager.GetQuest<LunaticCultistDefeated>().Completed;
}