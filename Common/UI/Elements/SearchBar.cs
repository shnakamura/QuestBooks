using Terraria.ModLoader.UI;
using Terraria.UI;

namespace QuestBooks.Common.UI;

public class SearchBar : Element
{
    public static SearchBar Full => new SearchBar().WithFullDimensions();

    public static SearchBar Empty => new();

    /// <summary>
    ///     Gets the text input field of the search bar.
    /// </summary>
    public TextInputField Input { get; }
    
    /// <summary>
    ///     Gets the search icon of the search bar.
    /// </summary>
    public Image Search { get; }
    
    /// <summary>
    ///     Gets the clear icon of the search bar.
    /// </summary>
    public Image Clear { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SearchBar"/> class.
    /// </summary>
    public SearchBar()
    {
        Input = TextInputField.Empty
            .WithWidth(StyleDimension.FromPixelsAndPercent(-(24f + 8f) * 2f, 1f))
            .WithHeight(StyleDimension.FromPercent(1f));

        Search = Image.FromPath("QuestBooks/Assets/Textures/UI/Search")
            .WithHAlign(0f)
            .WithVAlign(0.5f)
            .WithLeftClickCallback(Input.Begin)
            .WithHighlight(UICommon.DefaultUIBorderMouseOver)
            .WithComponent(InterfaceSounds.FromSounds(in SoundID.MenuTick, in SoundID.MenuOpen));

        Clear = Image.FromPath("QuestBooks/Assets/Textures/UI/SearchClear")
            .WithHAlign(0f)
            .WithVAlign(0.5f)
            .WithLeftClickCallback(Input.End)
            .WithLeftClickCallback(Input.Clear)
            .WithHighlight(UICommon.DefaultUIBorderMouseOver)
            .WithComponent(InterfaceSounds.FromSounds(in SoundID.MenuTick, in SoundID.MenuOpen));
    }
    
    public override void OnInitialize()
    {
        base.OnInitialize();

        Append(Panel.Full);
        Append
        (
            Flex.FromHorizontal(FlexAlignment.Center)
                .WithFullDimensions()
                .WithPadding(8f)
                .WithElement(Search)
                .WithElement(Input)
                .WithElement(Clear)
        );
    }
}

public static class SearchBarExtensions
{
    public static TSearchBar WithSearch<TSearchBar>(this TSearchBar search, TextInputFieldChangeCallback callback) where TSearchBar : SearchBar
    {
        search.Input.OnChangeContents += callback;
        
        return search;
    }
}