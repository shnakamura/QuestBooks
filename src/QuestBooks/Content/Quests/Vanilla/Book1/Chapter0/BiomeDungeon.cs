using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter0;

public class BiomeDungeon : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneDungeon;
}