using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter0;

public class CraftHardmodeFurnace : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftHardmodeFurnaceCheck() : CraftItemHook<CraftHardmodeFurnace>(ItemID.AdamantiteForge, ItemID.TitaniumForge);
}