using QuestBooks.Core.Quests;
using Terraria.DataStructures;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class ShimmerTransmute : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class ShimmerTransmuteCheck : GlobalItem
    {
        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (source is not EntitySource_Misc misc || misc.Context != "Shimmer")
            {
                return;
            }

            QuestBooksMod.MarkComplete<ShimmerTransmute>();
        }
    }
}