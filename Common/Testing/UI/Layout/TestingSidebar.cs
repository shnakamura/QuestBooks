using QuestBooks.Common.Testing.UI.Components;
using QuestBooks.Common.Testing.UI.Reload;
using QuestBooks.Common.UI.Elements;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Layout;

[TestingInterfaceReload]
public sealed class TestingSidebar : UIElement
{
    public override void OnInitialize()
    {
        base.OnInitialize();

        var searchBar = new SearchBar
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPixels(48f)
        };
        
        searchBar.SetPadding(8f);
        
        Append(searchBar);
        
        var modProgressOverview = new ModProgressOverview
        {
            Top = StyleDimension.FromPixels(searchBar.Top.Pixels + searchBar.Height.Pixels),
            Width = StyleDimension.FromPixels(256f),
            Height = StyleDimension.FromPixels(372f)
        };
        
        modProgressOverview.SetPadding(8f);

        Append(modProgressOverview);
        
        var totalProgressOverview = new TotalProgressOverview
        {
            Top = StyleDimension.FromPixels(modProgressOverview.Top.Pixels + modProgressOverview.Height.Pixels),
            Width = StyleDimension.FromPixels(256f),
            Height = StyleDimension.FromPixels(172f)
        };
        
        totalProgressOverview.SetPadding(8f);
        
        Append(totalProgressOverview);
        
        var closeButton = new CloseButton
        {
            HAlign = 0f,
            VAlign = 1f,
            Width = StyleDimension.FromPixels(256f),
            Height = StyleDimension.FromPixels(64f)
        };
        
        closeButton.SetPadding(8f);
        
        Append(closeButton);
    }
}