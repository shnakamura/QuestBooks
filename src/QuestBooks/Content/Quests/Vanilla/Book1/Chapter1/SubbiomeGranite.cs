using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class SubbiomeGranite : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneGranite;
}