using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;
using Terraria.DataStructures;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class CraftHellstoneGear : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class HellstoneGearItemCheck() : CraftItemHook(Complete)
    {
        private static void Complete(Item item, RecipeItemCreationContext context)
        {
            if (!context.Recipe.HasIngredient(ItemID.HellstoneBar))
            {
                return;
            }

            QuestBooksMod.MarkComplete<CraftHellstoneGear>();
        }
    }
}