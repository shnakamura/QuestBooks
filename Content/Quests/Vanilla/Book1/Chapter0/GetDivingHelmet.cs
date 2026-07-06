using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter0;

public class GetDivingHelmet : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.DivingHelmet);
}