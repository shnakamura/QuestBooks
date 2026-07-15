using ReLogic.Content;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class StretchedImage : UIElement
{
    private float opacity = 1f;

    /// <summary>
    ///     Gets the texture of this image.
    /// </summary>
    public Asset<Texture2D> Texture { get; protected set; }

    /// <summary>
    ///     Gets the frame of this image's texture.
    /// </summary>
    public Rectangle? Frame { get; protected set; }
    
    /// <summary>
    ///     Gets or sets the rotation of this image.
    /// </summary>
    public float Rotation { get; set; } = 0f;

    /// <summary>
    ///     Gets or sets the color of this image.
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Gets or sets the opacity of this image.
    /// </summary>
    public float Opacity
    {
        get => opacity;
        set => opacity = Math.Clamp(value, 0f, 1f);
    }
    
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StretchedImage"/> class.
    /// </summary>
    /// <param name="texture">
    ///     The texture of the image.
    /// </param>
    /// <param name="frame">
    ///     The frame of the image's texture.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="texture"/> is <see langword="null"/>.
    /// </exception>
    public StretchedImage(Asset<Texture2D> texture, Rectangle? frame = null)
    {
        ArgumentNullException.ThrowIfNull(texture);
        
        Texture = texture;
        Frame = frame;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Recalculate();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        var texture = Texture.Value;
        var dimensions = GetDimensions();
        
        spriteBatch.Draw(texture, dimensions.ToRectangle(), Frame, Color * Opacity, Rotation, default, Effects, 0f);
    }

    /// <summary>
    ///     Sets the texture of this image.
    /// </summary>
    /// <param name="texture">
    ///     The texture to set.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="texture"/> is <see langword="null"/>.
    /// </exception>
    public virtual void SetTexture(Asset<Texture2D> texture)
    {
        ArgumentNullException.ThrowIfNull(texture);
        
        Texture = texture;
        
        Recalculate();
    }
}