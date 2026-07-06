using QuestBooks.Core.Quests;
using Terraria.GameContent.Events;

namespace QuestBooks.Content.Quests.Vanilla.Book4.Chapter1;

public class EventParty : VanillaQuest
{
    public override bool CheckCompletion() => BirthdayParty.PartyIsUp;
}