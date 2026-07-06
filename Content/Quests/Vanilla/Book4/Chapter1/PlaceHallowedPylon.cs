using QuestBooks.Core.Quests;
using Terraria.GameContent;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter1;

public class PlaceHallowedPylon : VanillaQuest
{
    public override bool CheckCompletion() => Main.PylonSystem.HasPylonOfType(TeleportPylonType.Hallow);
}