using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter3;

public class GetLuminiteBars : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasItem(ItemID.LunarBar);
}