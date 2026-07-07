using Terraria.Audio;

namespace QuestBooks.Common.Commands;

internal sealed class QuickDesignerCommand : ModCommand
{
    public override string Command => "questdesigner";

    public override CommandType Type => CommandType.Chat;

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        QuestBooksMod.DesignerEnabled = !QuestBooksMod.DesignerEnabled;

        if (QuestBooksMod.DesignerEnabled)
        {
            SoundEngine.PlaySound(SoundID.AchievementComplete);
        }

        else
        {
            SoundEngine.PlaySound(SoundID.Unlock);
        }
    }
}