using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter2;

public class GetSharpeningStation : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.SharpeningStation);
}