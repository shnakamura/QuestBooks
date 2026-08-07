using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class ProgressBar : UIElement
{
    private sealed class ProgressBarPanel() : UIPanel(BackgroundTexture, BorderTexture)
    {
        private static readonly Asset<Texture2D> BackgroundTexture = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/ProgressBarBackground");
        
        private static readonly Asset<Texture2D> BorderTexture = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/ProgressBarBorder");
    }
    
    private sealed class ProgressBarFill() : StretchedImage(FillTexture)
    {
        private static readonly Asset<Texture2D> FillTexture = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/ProgressBarFill");
    }

    public delegate void ProgressBarChangeCallback(float progress);

    /// <summary>
    ///     Raised when the progress of the progress bar is changed.
    /// </summary>
    public event ProgressBarChangeCallback OnChangeProgress;
    
    /// <summary>
    ///     The background panel of the progress bar.
    /// </summary>
    private readonly UIPanel background;

    /// <summary>
    ///     The fill image of the progress bar.
    /// </summary>
    private readonly StretchedImage fill;

    private float progress;
    
    /// <summary>
    ///     Gets or sets the tooltip of the progress bar.
    /// </summary>
    public virtual string Tooltip { get; set; }
    
    /// <summary>
    ///     Gets the progress of the progress bar.
    /// </summary>
    public float Progress
    {
        get => progress;
        set
        {
            progress = Math.Clamp(value, 0f, 1f);
            
            OnChangeProgress?.Invoke(progress);
        }
    }
    
    /// <summary>
    ///     Gets or sets the color of the progress bar.
    /// </summary>
    public Color Color
    {
        get => fill.Color;
        set => fill.Color = value;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProgressBar"/> <see langword="class"/>.
    /// </summary>
    public ProgressBar()
    {
        background = new ProgressBarPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        Append(background);

        fill = new ProgressBarFill
        {
            Left = StyleDimension.FromPixels(2f),
            VAlign = 0.5f,
            Height = StyleDimension.FromPixelsAndPercent(-4f, 1f)
        };
        
        Append(fill);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        var pixels = MathHelper.SmoothStep(0f, background.Width.Pixels - 4f, Progress);
        var percent = MathHelper.SmoothStep(0f, background.Width.Percent, Progress);
        
        fill.Width.Set(pixels, percent);
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