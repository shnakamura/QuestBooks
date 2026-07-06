using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter0;

public class GetHardmodeWingMaterial : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HasAnyItem(ItemID.GiantHarpyFeather, ItemID.BoneFeather, ItemID.FireFeather, ItemID.IceFeather);
}