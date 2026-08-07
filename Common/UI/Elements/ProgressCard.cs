using QuestBooks.Common.UI.Components;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class ProgressCard : UIElement
{
    private readonly ProgressBar progressBar;
    
    private readonly Text percentage;

    /// <summary>
    ///     Gets or sets the tooltip of the progress card.
    /// </summary>
    public virtual string Tooltip { get; set; }

    /// <summary>
    ///     Gets or sets the progress of the progress card.
    /// </summary>
    public float Progress
    {
        get => progressBar.Progress;
        set => progressBar.Progress = value;
    }
    
    /// <summary>
    ///     Gets or sets the color of the progress card.
    /// </summary>
    public Color Color
    {
        get => progressBar.Color;
        set => progressBar.Color = value;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProgressCard"/> <see langword="class"/>.
    /// </summary>
    /// <param name="text">
    ///     The text of the progress card.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     <paramref name="text"/> is <see langword="null"/> or empty.
    /// </exception>
    public ProgressCard(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);
        
        Append(new SettingsPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        });

        Append(new Text(text)
        {
            Top = StyleDimension.FromPixels(8f),
            Left = StyleDimension.FromPixels(8f),
            Scale = 0.8f
        });
        
        progressBar = new ProgressBar
        {
            Top = StyleDimension.FromPixels(-8f),
            HAlign = 0.5f,
            VAlign = 1f,
            Width = StyleDimension.FromPixelsAndPercent(-8f * 2f, 1f),
            Height = StyleDimension.FromPixels(12f)
        };

        Append(progressBar);
        
        percentage = new Text
        {
            Top = StyleDimension.FromPixels(8f),
            Left = StyleDimension.FromPixels(-8f),
            HAlign = 1f,
            VAlign = 0f,
            Scale = 0.8f
        };

        Append(percentage);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProgressCard"/> <see langword="class"/>.
    /// </summary>
    /// <param name="text">
    ///     The localized text of the progress card.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    public ProgressCard(LocalizedText text) : this(text.Value) => ArgumentNullException.ThrowIfNull(text);

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        percentage.Contents = $"{Progress * 100f:F2}%";
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        if (!IsMouseHovering || string.IsNullOrEmpty(Tooltip))
        {
            return;
        }
        
        Main.instance.MouseText(Tooltip);
    }
}