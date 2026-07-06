using QuestBooks.Core.Quests;
using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Content.Quests.Vanilla.Book1.Chapter0;

[ReinitializeDuringResizeArrays]
public class CraftEvilBars : VanillaQuest
{
    static CraftEvilBars()
    {
        EvilBars = ItemID.Sets.Factory.CreateNamedSet("EvilBars")
            .Description("Any world evil bar")
            .RegisterBoolSet(ItemID.CrimtaneBar, ItemID.DemoniteBar);
    }

    public static readonly bool[] EvilBars;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class CraftEvilBarsCheck() : CraftItemHook<CraftEvilBars>(EvilBars);
}