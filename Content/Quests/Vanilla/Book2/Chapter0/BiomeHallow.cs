using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter0;

public class BiomeHallow : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneHallow;
}