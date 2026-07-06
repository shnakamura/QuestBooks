using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;
using Terraria.DataStructures;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter0;

public class CraftCactusGear : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftCactusGearCheck() : CraftItemHook(OnCraft)
    {
        private static void OnCraft(Item item, RecipeItemCreationContext context)
        {
            if (!context.Recipe.HasIngredient(ItemID.Cactus))
            {
                return;
            }

            QuestBooksMod.MarkComplete<CraftCactusGear>();
        }
    }
}