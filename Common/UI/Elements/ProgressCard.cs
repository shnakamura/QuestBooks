using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public sealed class ProgressCard : UIElement
{
    /// <summary>
    ///     Represents the method that is called when the progress of a <see cref="ProgressCard"/> changes.
    /// </summary>
    /// <param name="progress">
    ///     The new progress value.
    /// </param>
    public delegate void ProgressCardChangeCallback(float progress);
    
    /// <summary>
    ///     Occurs when the progress of the progress card changes.
    /// </summary>
    public event ProgressCardChangeCallback OnChangeProgress;
    
    private readonly string text;

    private ProgressBar progressBar;
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProgressCard"/> class with the specified text.
    /// </summary>
    /// <param name="text">
    ///     The text displayed by the progress card.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     <paramref name="text"/> is <see langword="null"/> or empty.
    /// </exception>
    public ProgressCard(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);

        this.text = text;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProgressCard"/> class with the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized text displayed by the progress card.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    public ProgressCard(LocalizedText text)
    {
        ArgumentNullException.ThrowIfNull(text);

        this.text = text.Value;
    }
    
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        var background = new UIPanel(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/PanelBackground"), ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/PanelBorder"))
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };

        Append(background);

        var label = new UIText(text, 0.8f)
        {
            HAlign = 0f,
            VAlign = 0f,
            Top = StyleDimension.FromPixels(8f),
            Left = StyleDimension.FromPixels(8f)
        };
        
        Append(label);
        
        progressBar = new ProgressBar
        {
            Color = Color.Green,
            HAlign = 0.5f,
            VAlign = 1f,
            Width = StyleDimension.FromPixelsAndPercent(-8f * 2f, 1f),
            Height = StyleDimension.FromPixels(12f),
            Top = StyleDimension.FromPixels(-8f)
        };
        
        Append(progressBar);
        
        var percentage = new UIText($"{progressBar.Progress * 100f:F2}%", 0.8f)
        {
            HAlign = 1f,
            VAlign = 0f,
            Top = StyleDimension.FromPixels(8f),
            Left = StyleDimension.FromPixels(-8f)
        };

        percentage.OnUpdate += _ => percentage.SetText($"{progressBar.Progress * 100f:F2}%");
        
        Append(percentage);
    }
    
    /// <summary>
    ///     Sets the progress of this progress card.
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
    public void SetProgress(float progress)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(progress);
        
        progressBar.SetProgress(progress);
        
        OnChangeProgress?.Invoke(progress);
    }

    /// <summary>
    ///     Sets the progress bar color of this progress card.
    /// </summary>
    /// <param name="color">
    ///     The color to set.
    /// </param>
    public void SetColor(in Color color) => progressBar.SetColor(in color);
}