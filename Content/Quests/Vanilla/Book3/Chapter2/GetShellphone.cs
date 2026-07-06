using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter2;

public class GetShellphone : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.Shellphone);
}