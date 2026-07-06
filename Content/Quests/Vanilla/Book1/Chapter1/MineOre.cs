using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter1;

public class MineOre : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class MineOreCheck() : KillTileHook<MineOre>(TileID.Sets.Ore);
}