using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter3;

public class PurifyWorld : VanillaQuest
{
    public override bool CheckCompletion() => WorldGen.totalEvil == 0;
}