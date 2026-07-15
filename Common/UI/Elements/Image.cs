using ReLogic.Content;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class Image : UIElement
{
    private Vector2 origin = new Vector2(0.5f);

    private float opacity = 1f;

    public bool Resize { get; init; } = true;
    
    public bool Snap { get; init; }
    
    public Vector2 Origin
    {
        get => origin;
        init => origin = Vector2.Clamp(value, Vector2.Zero, Vector2.One);
    }
    
    /// <summary>
    ///     Gets the texture of this image.
    /// </summary>
    public Asset<Texture2D> Texture { get; protected set; }
    
    /// <summary>
    ///     Gets the frame of this image's texture.
    /// </summary>
    public Rectangle? Frame { get; protected set; }

    /// <summary>
    ///     Gets or sets the scale of this image.
    /// </summary>
    public float Scale { get; set; } = 1f;

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
    
    /// <summary>
    ///     Gets or sets the sprite effects of this image.
    /// </summary>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Image"/> class.
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
    public Image(Asset<Texture2D> texture, Rectangle? frame = null)
    {
        ArgumentNullException.ThrowIfNull(texture);
        
        Texture = texture;
        Frame = frame;
        
        if (!Resize)
        {
            return;
        }
        
        Width.Set(Frame.HasValue ? Frame.Value.Width : Texture.Width(), 0f);
        Height.Set(Frame.HasValue ? Frame.Value.Height : Texture.Height(), 0f);
    }
    
    public override void Recalculate()
    {
        base.Recalculate();
        
        if (!Resize)
        {
            return;
        }
        
        Width.Set(Frame.HasValue ? Frame.Value.Width : Texture.Width(), 0f);
        Height.Set(Frame.HasValue ? Frame.Value.Height : Texture.Height(), 0f);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Recalculate();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        var dimensions = GetDimensions();
        
        var texture = Texture.Value;
        var size = Frame.HasValue ? Frame.Value.Size() : texture.Size();
        
        var position = dimensions.Position() + size * Origin;

        if (Snap)
        {
            position = position.Floor();
        }
        
        spriteBatch.Draw(texture, position, Frame, Color * Opacity, Rotation, size * Origin, Scale, Effects, 0f);
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

    /// <summary>
    ///     Sets the frame of this image's texture.
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