using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;
using Terraria.DataStructures;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter2;

public class UseSolarFragments : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class UseSolarFragmentsCheck() : CraftItemHook(OnCraft)
    {
        private static void OnCraft(Item item, RecipeItemCreationContext context)
        {
            if (!context.Recipe.HasTile(TileID.LunarCraftingStation) || !context.Recipe.HasIngredient(ItemID.FragmentSolar))
            {
                return;
            }

            QuestBooksMod.MarkComplete<UseSolarFragments>();
        }
    }
}