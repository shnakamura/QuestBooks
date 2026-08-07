using ReLogic.Content;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class StretchedImage : UIElement
{
    private float opacity = 1f;

    /// <summary>
    ///     Gets the texture asset of the image.
    /// </summary>
    public Asset<Texture2D> Texture { get; protected set; }

    /// <summary>
    ///     Gets the frame of the image.
    /// </summary>
    public Rectangle? Frame { get; protected set; }

    /// <summary>
    ///     Gets or sets the rotation of the image.
    /// </summary>
    public float Rotation { get; set; } = 0f;

    /// <summary>
    ///     Gets or sets the color of the image.
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Gets or sets the opacity of the image.
    /// </summary>
    /// <value>
    ///     A value in the range of <c>[0f - 1f]</c>, where <c>0f</c> represents fully transparent and <c>1f</c> represents fully opaque.
    /// </value>
    public float Opacity
    {
        get => opacity;
        set => opacity = Math.Clamp(value, 0f, 1f);
    }
    
    /// <summary>
    ///     Gets or sets the sprite effects of the image.
    /// </summary>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    /// <summary>
    ///     Gets or sets the tooltip of the image.
    /// </summary>
    public virtual string Tooltip { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="StretchedImage"/> <see langword="class"/>.
    /// </summary>
    /// <param name="texture">
    ///     The texture of the image.
    /// </param>
    /// <param name="frame">
    ///     The frame of the image.
    /// </param>
    public StretchedImage(Asset<Texture2D> texture, Rectangle? frame = null)
    {
        texture.Wait();
        
        Texture = texture;
        Frame = frame;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        var texture = Texture.Value;
        var dimensions = GetInnerDimensions();

        var bounds = dimensions.ToRectangle();
        
        spriteBatch.Draw(texture, bounds, Frame, Color * Opacity, Rotation, default, Effects, 0f);

        if (!IsMouseHovering || string.IsNullOrEmpty(Tooltip))
        {
            return;
        }

        Main.instance.MouseText(Tooltip);
    }

    /// <summary>
    ///     Sets the texture of the image.
    /// </summary>
    /// <param name="texture">
    ///     The texture to set.
    /// </param>
    public virtual void SetTexture(Asset<Texture2D> texture)
    {
        texture.Wait();
        
        Texture = texture;
        
        Recalculate();
    }
    
    /// <summary>
    ///     Sets the frame of the image.
    /// </summary>
    /// <param name="frame">
    ///     The frame to set.
    /// </param>
    public virtual void SetFrame(Rectangle? frame)
    {
        Frame = frame;
        
        Recalculate();
    }
}