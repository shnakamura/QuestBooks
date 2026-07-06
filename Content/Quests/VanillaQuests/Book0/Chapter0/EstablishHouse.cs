using System.Linq;
using QuestBooks.Core.Quests;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class EstablishHouse : VanillaQuest
{
    public override bool CheckCompletion() => Main.npc.Any(n => n.active && n.townNPC && WorldGen.TownManager.HasRoomQuick(n.type));
}