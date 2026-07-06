using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter3;

public class GetVortexSet : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAllItems(ItemID.VortexHelmet, ItemID.VortexBreastplate, ItemID.VortexLeggings);
}