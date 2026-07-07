using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter0;

public class SmashEvilAltar : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class SmashEvilAltarCheck() : KillTileHook<SmashEvilAltar>(TileID.DemonAltar);
}