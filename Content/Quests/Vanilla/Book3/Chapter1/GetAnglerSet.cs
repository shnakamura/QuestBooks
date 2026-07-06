using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter1;

public class GetAnglerSet : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAllItems(ItemID.AnglerHat, ItemID.AnglerVest, ItemID.AnglerPants);
}