using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter1;

public class SubbiomeTemple : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneLihzhardTemple;
}