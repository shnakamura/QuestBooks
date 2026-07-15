using QuestBooks.Common.Testing.UI.Layout;
using QuestBooks.Common.Testing.UI.Reload;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI;

[TestingInterfaceReload]
public sealed class TestingState : UIState
{
    public override void OnInitialize()
    {
        base.OnInitialize();

        var container = new UIElement
        {
            HAlign = 0.5f,
            VAlign = 0.5f,
            Width = StyleDimension.FromPixels(1280f),
            Height = StyleDimension.FromPixels(720f)
        };
        
        Append(container);

        var background = new UIPanel(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/PanelBackground"), ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/PanelBorder"))
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f),
        };
        
        container.Append(background);
        
        var header = new TestingHeader
        {
            Width = StyleDimension.FromPixels(256f),
            Height = StyleDimension.FromPixels(64f)
        };
        
        header.SetPadding(8f);
        
        container.Append(header);

        var sidebar = new TestingSidebar
        {
            Width = StyleDimension.FromPixels(256f),
            Height = StyleDimension.FromPixelsAndPercent(-header.Height.Pixels, 1f),
            Top = StyleDimension.FromPixels(header.Top.Pixels + header.Height.Pixels)
        };
        
        container.Append(sidebar);

        var table = new TestingQuestTable
        {
            Left = StyleDimension.FromPixels(sidebar.Left.Pixels + sidebar.Width.Pixels),
            HAlign = 0f,
            VAlign = 0.5f,
            Width = StyleDimension.FromPixels(720f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        table.SetPadding(8f);
        
        container.Append(table);
    }
}
