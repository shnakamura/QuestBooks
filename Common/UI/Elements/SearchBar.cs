using ReLogic.Content;
using Terraria.Localization;

namespace QuestBooks.Common.UI.Elements;

public class SearchBar : Element
{
    /// <summary>
    ///     The texture asset of the search icon.
    /// </summary>
    public static readonly Asset<Texture2D> SEARCH_ICON_TEXTURE = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/SearchIcon", AssetRequestMode.ImmediateLoad);
    
    /// <summary>
    ///     The texture asset of the clear icon.
    /// </summary>
    public static readonly Asset<Texture2D> CLEAR_ICON_TEXTURE = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/SearchClearIcon", AssetRequestMode.ImmediateLoad);
    
    private readonly TextInputField input = new TextInputField().WithFill(0.9f, 1f);
    
    public override void OnInitialize()
    {
        base.OnInitialize();

        Append(new SettingsPanel().WithFill(1f));
        Append
        (
            Flex.Horizontal(FlexAlignment.Evenly)
                .WithPadding(8f)
                .WithFill(1f)
                .WithElement
                (
                    Image.FromAsset(SEARCH_ICON_TEXTURE)
                        .WithAlignment(0f, 0.5f)
                        .WithLeftClickEvent(input.Begin)
                        .WithDefaultSoundSettings()
                        .WithDefaultHighlightSettings()
                        .WithTooltipSettings(new ImageTooltipSettings(Language.GetText("Mods.QuestBooks.UI.Common.Buttons.Search")))
                )
                .WithElement(input)
                .WithElement
                (
                    Image.FromAsset(CLEAR_ICON_TEXTURE)
                        .WithAlignment(0f, 0.5f)
                        .WithLeftClickEvent(input.Clear)
                        .WithLeftClickEvent(input.End)
                        .WithDefaultSoundSettings()
                        .WithDefaultHighlightSettings()
                        .WithTooltipSettings(new ImageTooltipSettings(Language.GetText("Mods.QuestBooks.UI.Common.Buttons.Clear")))
                )
        );
    }
}