using ReLogic.Content;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class Image : UIElement
{
    private Vector2 origin = new Vector2(0.5f);

    private float opacity = 1f;

    /// <summary>
    ///     Gets or sets the normalized origin of the image.
    /// </summary>
    /// <value>
    ///     A value in the range of <c>[(0f, 0f) - (1f, 1f)]</c>, where <c>(0f, 0f)</c>
    ///     represents the top-left corner of the image and <c>(1f, 1f)</c>
    ///     represents the bottom-right corner.
    /// </value>
    public Vector2 Origin
    {
        get => origin;
        set => origin = Vector2.Clamp(value, Vector2.Zero, Vector2.One);
    }

    /// <summary>
    ///     Gets the texture asset of the image.
    /// </summary>
    public Asset<Texture2D> Texture { get; protected set; }

    /// <summary>
    ///     Gets the frame of the image.
    /// </summary>
    public Rectangle? Frame { get; protected set; }

    /// <summary>
    ///     Gets or sets the scale of the image.
    /// </summary>
    public float Scale { get; set; } = 1f;

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
    
    public virtual string Tooltip { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Image"/> <see langword="class"/>.
    /// </summary>
    /// <param name="texture">
    ///     The texture of the image.
    /// </param>
    /// <param name="frame">
    ///     The frame of the image.
    /// </param>
    public Image(Asset<Texture2D> texture, Rectangle? frame = null)
    {
        texture.Wait();
        
        Texture = texture;
        Frame = frame;
   
        Width.Set((Frame.HasValue ? Frame.Value.Width : Texture.Width()) * Scale, 0f);
        Height.Set(Frame.HasValue ? Frame.Value.Height : Texture.Height() * Scale, 0f);
    }
    
    public override void Recalculate()
    {
        base.Recalculate();

        Width.Set((Frame.HasValue ? Frame.Value.Width : Texture.Width()) * Scale, 0f);
        Height.Set(Frame.HasValue ? Frame.Value.Height : Texture.Height() * Scale, 0f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        var dimensions = GetDimensions();
        
        var texture = Texture.Value;
        var size = Frame.HasValue ? Frame.Value.Size() : texture.Size();
        
        var position = dimensions.Position() + size * Origin;

        position = position.Floor();

        var scale = Scale;
        
        if (size.X > dimensions.Width)
        {
            scale *= dimensions.Width / size.X;
        }
        
        spriteBatch.Draw(texture, position, Frame, Color * Opacity, Rotation, size * Origin, scale, Effects, 0f);

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
        Texture = texture;
        
        Recalculate();
    }

    /// <summary>
    ///     Sets the frame of the image's texture.
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