using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class ProgressBar : UIElement
{
    /// <summary>
    ///     Represents the method that is called when the progress of a <see cref="ProgressBar"/> changes.
    /// </summary>
    /// <param name="progress">
    ///     The new progress value.
    /// </param>
    public delegate void ProgressBarChangeCallback(float progress);
    
    /// <summary>
    ///     Occurs when the progress of the progress bar changes.
    /// </summary>
    public event ProgressBarChangeCallback OnChangeProgress;
    
    private UIPanel background;

    private StretchedImage fill;

    /// <summary>
    ///     Gets the progress of this progress bar.
    /// </summary>
    public float Progress { get; protected set; }

    /// <summary>
    ///     Gets the color of this progress bar.
    /// </summary>
    public Color Color { get; init; } = Color.White;

    /// <summary>
    ///     Gets a value indicating whether a progress tooltip will be displayed when hovering over this progress bar.
    /// </summary>
    public bool Hover { get; init; } = true;

    public override void Recalculate()
    {
        base.Recalculate();
        
        const float speed = 0.33f;

        var pixels = MathF.Max(MathHelper.SmoothStep(fill.Width.Pixels, background.Width.Pixels * Progress - 4f, speed), 0f);
        var percent = MathF.Max(MathHelper.SmoothStep(fill.Width.Percent, background.Width.Percent * Progress, speed), 0f);
        
        fill.Width.Set(pixels, percent);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        
        background = new UIPanel(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/ProgressBarBackground"), ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/ProgressBarBorder"))
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        Append(background);

        fill = new StretchedImage(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/ProgressBarFill"))
        {
            Color = Color,
            HAlign = 0f,
            VAlign = 0.5f,
            Left = StyleDimension.FromPixels(2f),
            Height = StyleDimension.FromPixelsAndPercent(-4f, 1f)
        };
        
        Append(fill);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        Recalculate();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        if (!IsMouseHovering)
        {
            return;
        }
        
        UICommon.TooltipMouseText($"{Progress * 100f:F2}%");
    }

    /// <summary>
    ///     Sets the progress of this progress bar.
    /// </summary>
    /// <param name="progress">
    ///     The progress to set.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="progress"/> is negative.
    /// </exception>
    /// <remarks>
    ///     Clamped in the range of <c>[0f - 1f]</c>, where <c>0f</c> is empty and <c>1f</c> is full.
    /// </remarks>
    public virtual void SetProgress(float progress)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(progress);
        
        Progress = Math.Clamp(progress, 0f, 1f);
        
        OnChangeProgress?.Invoke(Progress);
    }

    /// <summary>
    ///     Sets the color of this progress bar.
    /// </summary>
    /// <param name="color">
    ///     The color to set.
    /// </param>
    public virtual void SetColor(in Color color) => fill.Color = color;
}