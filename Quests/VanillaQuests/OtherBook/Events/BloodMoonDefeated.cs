namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class BloodMoonDefeated : VanillaQuest
{
    public override bool CheckCompletion() => false;

    public class BloodMoonCheck : ModSystem
    {
        private bool cachedBloodMoon;

        public override void PostUpdateTime()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            if (!Main.bloodMoon && cachedBloodMoon)
            {
                QuestBooksMod.CompleteQuest<BloodMoonDefeated>();
            }

            cachedBloodMoon = Main.bloodMoon;
        }
    }
}