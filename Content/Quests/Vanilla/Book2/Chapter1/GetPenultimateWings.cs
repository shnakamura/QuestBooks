using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter1;

public class GetPenultimateWings : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem(ItemID.FishronWings, ItemID.BetsyWings, ItemID.RainbowWings);
}