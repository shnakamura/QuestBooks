using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book3.Chapter0;

public class CraftMoltenPickaxe : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftMoltenPickaxeCheck() : CraftItemHook<CraftMoltenPickaxe>(ItemID.MoltenPickaxe);
}