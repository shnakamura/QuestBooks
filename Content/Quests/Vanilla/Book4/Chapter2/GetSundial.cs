using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter2;

public class GetSundial : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.Sundial);
}