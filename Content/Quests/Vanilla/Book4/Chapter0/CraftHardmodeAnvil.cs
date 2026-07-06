using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter0;

public class CraftHardmodeAnvil : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class CraftHardmodeAnvilCheck() : CraftItemHook<CraftHardmodeAnvil>(ItemID.MythrilAnvil, ItemID.OrichalcumAnvil);
}