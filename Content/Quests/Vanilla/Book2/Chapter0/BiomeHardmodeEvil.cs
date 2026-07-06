using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.Book2.Chapter0;

public class BiomeHardmodeEvil : VanillaQuest
{
    public override bool CheckCompletion() => Main.hardMode && (Main.LocalPlayer.ZoneCrimson || Main.LocalPlayer.ZoneCorrupt);
}