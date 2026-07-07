using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter1;

public class Bestiary25 : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.GetBestiaryProgressReport().CompletionPercent >= 0.25f;
}