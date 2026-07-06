using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter2;

public class CraftCelestialSigil : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftCelestialSigilCheck() : CraftItemHook<CraftCelestialSigil>(ItemID.CelestialSigil);
}