using QuestBooks.Common.UI.Components;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class PanelButton : UIElement
{
    private readonly SettingsPanel background;

    private readonly Text label;
    
    /// <summary>
    ///     Gets a value indicating whether to highlight the button while the cursor is hovering over it.
    /// </summary>
    public bool Highlight { get; init; } = true;

    /// <summary>
    ///     Gets or sets the scale of the button's label.
    /// </summary>
    public float Scale
    {
        get => label.Scale;
        set => label.Scale = value;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="PanelButton"/> <see langword="class"/>.
    /// </summary>
    /// <param name="text">
    ///     The text of the button.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     <paramref name="text"/> is <see langword="null"/> or empty.
    /// </exception>
    public PanelButton(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);

        background = new SettingsPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        Append(background);
        
        label = new Text(text)
        {
            HAlign = 0.5f,
            VAlign = 0.5f
        };

        Append(label);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PanelButton"/> <see langword="class"/>.
    /// </summary>
    /// <param name="text">
    ///     The text of the button.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    public PanelButton(LocalizedText text) : this(text.Value) => ArgumentNullException.ThrowIfNull(text);
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!Highlight)
        {
            return;
        }
        
        background.BorderColor = IsMouseHovering ? UICommon.DefaultUIBorderMouseOver : UICommon.DefaultUIBorder;
    }
}