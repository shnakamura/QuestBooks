using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter1;

public class EnterGraveyard : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneGraveyard;
}