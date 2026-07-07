using QuestBooks.Core.Quests;

namespace QuestBooks.Content.Quests.Vanilla.OtherBook.Bosses;

public class MoonLordDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMoonlord;
}