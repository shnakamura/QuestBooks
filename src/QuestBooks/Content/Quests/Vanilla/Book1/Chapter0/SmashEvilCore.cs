using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter0;

public class SmashEvilCore : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class SmashEvilCoreCheck() : KillTileHook<SmashEvilCore>(TileID.ShadowOrbs);
}