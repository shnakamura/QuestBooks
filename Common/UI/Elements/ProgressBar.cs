using Terraria.UI;

namespace QuestBooks.Common.UI;

public sealed class ProgressBar : Element
{
    public static ProgressBar Full => new ProgressBar().WithFullDimensions();
    
    public static ProgressBar Empty => new();
    
    private float _progress;
    
    /// <summary>
    ///     Gets the progress of the progress bar.
    /// </summary>
    /// <value>
    ///     A value in the range of <c>[0f, 1f]</c>, where <c>0f</c> represents no progress and <c>1f</c> represents full progress.
    /// </value>
    public float Progress
    {
        get => _progress;
        set => _progress = Math.Clamp(value, 0f, 1f);
    }

    /// <summary>
    ///     Gets the background panel of the progress bar.
    /// </summary>
    public Panel Background { get; }

    /// <summary>
    ///     Gets the fill panel of the progress bar.
    /// </summary>
    public Panel Fill { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProgressBar"/> class.
    /// </summary>
    private ProgressBar()
    {
        Background = Panel.FromPath("QuestBooks/Assets/Textures/UI/DarkPanel").WithFullDimensions();
        Fill = Panel.FromPath("QuestBooks/Assets/Textures/UI/Panel").WithHeight(StyleDimension.FromPercent(1f));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProgressBar"/> class with the specified progress.
    /// </summary>
    /// <param name="progress">
    ///     The progress of the progress bar.
    /// </param>
    private ProgressBar(float progress) : this() => Progress = progress;

    /// <inheritdoc/>
    public override void OnInitialize()
    {
        base.OnInitialize();

        Append(Background);
        Append(Fill);
    }

    /// <inheritdoc/>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        Fill.Width = StyleDimension.FromPercent(Progress);
    }

    /// <summary>
    ///     Returns a new <see cref="ProgressBar"/> with the specified progress.
    /// </summary>
    /// <param name="progress">
    ///     The progress of the progress bar.
    /// </param>
    /// <returns>
    ///     A new <see cref="ProgressBar"/> with the specified progress.
    /// </returns>
    public static ProgressBar FromProgress(float progress) => new(progress);
}

public static class ProgressBarExtensions
{
    public static ProgressBar WithColor(this ProgressBar bar, in Color color)
    {
        bar.Fill.WithBackgroundColor(color);

        return bar;
    }
}
