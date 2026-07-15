using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public sealed class SearchBar : UIElement
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

        var searchIcon = new Image(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/SearchIcon"))
        {
            Snap = true,
            HAlign = 0f,
            VAlign = 0.5f,
            Left = StyleDimension.FromPixels(8f)
        };

        Append(searchIcon);

        var inputField = new TextInputField
        {
            Placeholder = Language.GetTextValue("Mods.QuestBooks.UI.Common.SearchBars.Placeholder"),
            Tags = false,
            Scale = 0.8f,
            Capacity = 20,
            HAlign = 0.5f,
            VAlign = 0.5f,
            Width = StyleDimension.FromPercent(0.75f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        Append(inputField);

        var clearIcon = new Image(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/SearchClearIcon"))
        { 
            Snap = true,
            HAlign = 1f,
            VAlign = 0.5f,
            Left = StyleDimension.FromPixels(-8f)
        };

        clearIcon.OnMouseOver += (_, _) => SoundEngine.PlaySound(in SoundID.MenuTick); 
        clearIcon.OnMouseOut += (_, _) => SoundEngine.PlaySound(in SoundID.MenuTick);

        clearIcon.OnLeftClick += (_, _) => inputField.StopWriting(true);
        
        Append(clearIcon);
    }
}