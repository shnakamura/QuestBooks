using QuestBooks.Core.Graphics;
using ReLogic.Content;

namespace QuestBooks.Common.UI;

public sealed class Image : Element
{
    private Asset<Texture2D> _asset;

    private Rectangle? _frame;
    
    private float _opacity = 1f;
    
    /// <summary>
    ///     Gets or sets the texture asset of the image.
    /// </summary>
    public Asset<Texture2D> Asset
    {
        get => _asset;
        set
        {
            _asset = value;
            
            Recalculate();
        }
    }

    /// <summary>
    ///     Gets or sets the frame of the image.
    /// </summary>
    public Rectangle? Frame
    {
        get => _frame;
        set
        {
            _frame = value;
            
            Recalculate();
        }
    }

    /// <summary>
    ///     Gets or sets the scale of the image.
    /// </summary>
    public Vector2 Scale { get; set; } = new Vector2(1f);

    /// <summary>
    ///     Gets or sets the rotation of the image, in radians.
    /// </summary>
    public float Rotation { get; set; }
    
    /// <summary>
    ///     Gets or sets the opacity of the image.
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
    ///     Gets or sets the color of the image.
    /// </summary>
    public Color Color { get; set; } = Color.White;
    
    /// <summary>
    ///     Gets or sets the highlight color of the image.
    /// </summary>
    public Color Highlight { get; set; } = Color.Transparent;
 
    /// <summary>
    ///     Gets or sets the sprite effects of the image.
    /// </summary>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    
    /// <summary>
    ///     Gets or sets a value indicating whether the image should fit within the bounds of the element.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the image should fit within the bounds of the element; otherwise, <see langword="false"/>.
    /// </value>
    public bool Fit { get; set; } = true;
    
    /// <summary>
    ///     Gets the texture of the image.
    /// </summary>
    public Texture2D Texture => Asset.Value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Image"/> <see langword="class"/> with the specified texture asset.
    /// </summary>
    /// <param name="asset">
    ///     The texture asset of the image.
    /// </param>
    private Image(Asset<Texture2D> asset) => Asset = asset;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Image"/> <see langword="class"/> with the specified texture asset path.
    /// </summary>
    /// <param name="path">
    ///     The path of the texture asset of the image.
    /// </param>
    private Image(string path) => Asset = ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad);
    
    /// <inheritdoc/>
    public override void Recalculate()
    {
        base.Recalculate();

        Width.Set((Frame.HasValue ? Frame.Value.Width : Asset.Width()) * Scale.X, 0f);
        Height.Set(Frame.HasValue ? Frame.Value.Height : Asset.Height() * Scale.Y, 0f);
    }
    
    /// <inheritdoc/>
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        var dimensions = GetDimensions();
        
        var size = Frame.HasValue ? Frame.Value.Size() : Texture.Size();

        var position = dimensions.Center();
        var origin = size / 2f;
        
        var fit = Fit && size.X > dimensions.Width;
        var scale = fit ? Scale * dimensions.Width / size.X : Scale;

        if (Highlight != Color.Transparent && IsMouseHovering)
        {
            var parameters = spriteBatch.Capture() with
            {
                SpriteSortMode = SpriteSortMode.Immediate
            };

            using var scope = spriteBatch.Scope(in parameters);

            Main.pixelShader.CurrentTechnique.Passes["ColorOnly"].Apply();
            
            const float offset = 2f;
            
            var highlight = Highlight * Opacity;
        
            spriteBatch.Draw(Asset.Value, position + new Vector2(0f, offset), Frame, highlight, Rotation, origin, scale, Effects, 0f);
            spriteBatch.Draw(Asset.Value, position + new Vector2(0f, -offset), Frame, highlight, Rotation, origin, scale, Effects, 0f);
            spriteBatch.Draw(Asset.Value, position + new Vector2(offset, 0f), Frame, highlight, Rotation, origin, scale, Effects, 0f);
            spriteBatch.Draw(Asset.Value, position + new Vector2(-offset, 0f), Frame, highlight, Rotation, origin, scale, Effects, 0f);
        }
        
        spriteBatch.Draw(Texture, position, Frame, Color * Opacity, Rotation, origin, scale, Effects, 0f);
    }
    
    /// <summary>
    ///     Returns a new <see cref="Image"/> from the specified texture asset path.
    /// </summary>
    /// <param name="path">
    ///     The path of the texture asset of the image.
    /// </param>
    /// <returns>
    ///     A new <see cref="Image"/> with the specified asset path.
    /// </returns>
    public static Image FromPath(string path) => new(path);

    /// <summary>
    ///     Returns a new <see cref="Image"/> from the specified texture asset.
    /// </summary>
    /// <param name="asset">
    ///     The texture asset of the image.
    /// </param>
    /// <returns>
    ///     A new <see cref="Image"/> with the specified asset.
    /// </returns>
    public static Image FromAsset(Asset<Texture2D> asset) => new(asset);
}

public static class ImageExtensions
{
    public static Image WithRotation(this Image image, float rotation)
    {
        image.Rotation = rotation;
        
        return image;
    }
    
    public static Image WithFrame(this Image image, in Rectangle frame)
    {
        image.Frame = frame;

        return image;
    }
    
    public static Image WithHighlight(this Image image, in Color color)
    {
        image.Highlight = color;
        
        return image;
    }
    
    public static Image WithColor(this Image image, in Color color)
    {
        image.Color = color;
        
        return image;
    }
    
    public static Image WithOpacity(this Image image, float opacity)
    {
        image.Opacity = opacity;
        
        return image;
    }
}