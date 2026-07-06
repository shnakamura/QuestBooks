using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class SubbiomeAether : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneShimmer;
}