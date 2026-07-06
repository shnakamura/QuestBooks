using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class WallOfFleshDefeated : VanillaQuest
{
    public override bool CheckCompletion() => Main.hardMode;
}