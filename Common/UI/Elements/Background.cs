using Terraria.ModLoader.UI;

namespace QuestBooks.Common.UI;

public sealed class Background : Element
{
    public static Background Full => new Background().WithFullDimensions();

    public static Background Empty => new();

    private float _opacity = 1f;
    
    /// <summary>
    ///     Gets or sets the color of the background.
    /// </summary>
    public Color Color { get; set; } = UICommon.DefaultUIBlue;
    
    /// <summary>
    ///     Gets or sets the opacity of the background.
    /// </summary>
    /// <value>
    ///     A value in the range of <c>[0f - 1f]</c>, where <c>0f</c> represents fully transparent and <c>1f</c> represents fully opaque.
    /// </value>
    public float Opacity
    {
        get => _opacity;
        set => _opacity = Math.Clamp(value, 0f, 1f);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Background"/> class.
    /// </summary>
    private Background() { }

    protected override void Draw(in ElementDrawContext context)
    {
        base.Draw(in context);

        if (!context.Self)
        {
            return;
        }
        
        var dimensions = GetDimensions();
        var position = dimensions.Position();

        Utils.DrawSettingsPanel(context.Batch, position, dimensions.Width, Color * Opacity);
    }
}

/// <summary>
///     Provides <see cref="Background"/> extensions.
/// </summary>
public static class BackgroundExtensions
{
    public static Background WithColor(this Background background, in Color color)
    {
        background.Color = color;
        
        return background;
    }

    public static Background WithOpacity(this Background background, float opacity)
    {
        background.Opacity = opacity;

        return background;
    }
}