using QuestBooks.Common.Testing.UI.Reload;
using QuestBooks.Common.UI.Elements;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Components;

[TestingInterfaceReload]
public sealed class ModProgressOverview : UIElement
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
        
        var label = new UIText(Language.GetText("Mods.QuestBooks.UI.Testing.Labels.ModProgressOverview"), 0.9f)
        {
            Left = StyleDimension.FromPixels(8f),
            Top = StyleDimension.FromPixels(8f)
        };
        
        Append(label);

        var list = new UIList
        {
            Top = StyleDimension.FromPixels(label.Top.Pixels + label.Height.Pixels + 16f),
            HAlign = 0.5f,
            VAlign = 0f,
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPixelsAndPercent(-32f, 1f)
        };

        list.ListPadding = 0f;

        list.SetScrollbar(new UIScrollbar());

        // TODO: Only list mods that have at least one quest.
        foreach (var mod in ModLoader.Mods)
        {
            var card = new ProgressCard(mod.DisplayNameClean)
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPixels(64f),
                PaddingLeft = 8f,
                PaddingRight = 8f,
                PaddingTop = 8f,
                PaddingBottom = 8f
            };
            
            list.Add(card);
        }
        
        Append(list);
    }
}