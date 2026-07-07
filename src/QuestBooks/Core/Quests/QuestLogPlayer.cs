using QuestBooks.Common.Input;
using Terraria.Audio;
using Terraria.GameInput;

namespace QuestBooks.Core.Quests;

internal sealed class QuestLogPlayer : ModPlayer
{
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (!KeybindSystem.ToggleQuestLog.JustPressed)
        {
            return;
        }
        
        QuestLogDrawer.Toggle();
    }

    public override void SetControls()
    {
        if (!QuestLogDrawer.DisplayLog || (!QuestLogDrawer.TargetDisplayLog && QuestLogDrawer.AnimationInProgress))
        {
            return;
        }

        if (Main.LocalPlayer.controlInv)
        {
            QuestLogDrawer.Toggle(false);
            
            Main.LocalPlayer.releaseInventory = false;
        }
        else if (Main.LocalPlayer.controlCreativeMenu && Main.LocalPlayer.difficulty == PlayerDifficultyID.Creative)
        {
            QuestLogDrawer.Toggle(false);
            
            Main.LocalPlayer.releaseCreativeMenu = false;
        }
    }

    public override void PostUpdate()
    {
        if (!Main.LocalPlayer.dead || !QuestLogDrawer.DisplayLog)
        {
            return;
        }
        
        QuestLogDrawer.Toggle(false);
    }

    public override void OnEnterWorld()
    {
        QuestLogDrawer.Toggle(false, true);

        QuestLogDrawer.ActiveStyle.SelectedBook = null;
        QuestLogDrawer.ActiveStyle.SelectedChapter = null;
        QuestLogDrawer.ActiveStyle.SelectedElement = null;

        QuestLogDrawer.ActiveStyle.OnEnterWorld();
    }
}