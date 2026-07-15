using QuestBooks.Common.Testing.UI.Reload;
using QuestBooks.Common.UI.Elements;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Components;

[TestingInterfaceReload]
public sealed class TotalProgressOverview : UIElement
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        var background = new UIPanel(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/PanelBackground"), ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/PanelBorder"))
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };

        Append(background);
        
        var label = new UIText(Language.GetText("Mods.QuestBooks.UI.Testing.Labels.TotalProgressOverview"), 0.9f)
        {
            Left = StyleDimension.FromPixels(8f),
            Top = StyleDimension.FromPixels(8f)
        };
        
        Append(label);

        var questsProgressCard = new ProgressCard(Language.GetText("Mods.QuestBooks.UI.Testing.ProgressBars.Quests"))
        {
            Top = StyleDimension.FromPixels(label.Top.Pixels + label.Height.Pixels + 20f),
            HAlign = 0.5f,
            VAlign = 0f,
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPixels(64f)
        };
        
        questsProgressCard.SetPadding(8f);
        
        Append(questsProgressCard);
        
        var modsProgressCard = new ProgressCard(Language.GetText("Mods.QuestBooks.UI.Testing.ProgressBars.Mods"))
        {
            Top = StyleDimension.FromPixels(questsProgressCard.Top.Pixels + questsProgressCard.Height.Pixels),
            HAlign = 0.5f,
            VAlign = 0f,
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPixels(64f)
        };
        
        modsProgressCard.SetPadding(8f);
        
        Append(modsProgressCard);
    }
}