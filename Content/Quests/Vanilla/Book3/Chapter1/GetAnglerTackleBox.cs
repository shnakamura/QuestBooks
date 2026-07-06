using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter1;

public class GetAnglerTackleBox : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.AnglerTackleBag);
}