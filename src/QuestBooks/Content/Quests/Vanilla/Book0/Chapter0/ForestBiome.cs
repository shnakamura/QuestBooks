using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book0.Chapter0;

public class ForestBiome : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.ZoneForest;
}