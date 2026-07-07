using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class EnterCaverns : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneRockLayerHeight;
}