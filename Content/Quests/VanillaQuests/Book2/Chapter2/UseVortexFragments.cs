using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class UseVortexFragments : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class UseVortexFragmentsCheck() : CraftItemHook(OnCraft)
    {
        private static void OnCraft(Item item, RecipeItemCreationContext context)
        {
            if (!context.Recipe.HasTile(TileID.LunarCraftingStation) || !context.Recipe.HasIngredient(ItemID.FragmentVortex))
            {
                return;
            }

            QuestBooksMod.MarkComplete<UseVortexFragments>();
        }
    }
}