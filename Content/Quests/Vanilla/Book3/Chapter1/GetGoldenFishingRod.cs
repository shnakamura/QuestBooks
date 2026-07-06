using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter1;

public class GetGoldenFishingRod : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.GoldenFishingRod);
}