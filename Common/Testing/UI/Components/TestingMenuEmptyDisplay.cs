using QuestBooks.Common.UI.Components;
using QuestBooks.Common.UI.Elements;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Components;

public sealed class TestingMenuEmptyDisplay : UIElement
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        SetPadding(8f);

        Append(new SettingsPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        });
        
        Append(new Text(Language.GetText("Mods.QuestBooks.UI.Testing.Displays.Empty"))
        {
            Font = FontAssets.DeathText,
            PaddingTop = 32f,
            PaddingLeft = 32f,
            PaddingBottom = 32f,
            PaddingRight = 32f,
            HAlign = 0.5f,
            VAlign = 0.5f
        });
    }
}