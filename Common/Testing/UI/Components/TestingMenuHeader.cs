using QuestBooks.Common.UI.Components;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.Layout;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Components;

public sealed class TestingMenuHeader : UIElement
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

        var stack = new HorizontalStack
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f),
            Gap = 4f
        };
        
        stack.SetPadding(8f);
        
        Append(stack);
        
        stack.Add(new Image(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/Testing/HeaderIcon"))
        {
            HAlign = 0f,
            VAlign = 0.5f
        });

        stack.Add(new Text(Language.GetText("Mods.QuestBooks.UI.Testing.Header"))
        {
            HAlign = 0f,
            VAlign = 0.5f
        });
    }
}