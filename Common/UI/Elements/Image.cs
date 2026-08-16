using QuestBooks.Core.Graphics;
using ReLogic.Content;
using Terraria.ModLoader.UI;

namespace QuestBooks.Common.UI.Elements;

public readonly record struct ImageHighlightSettings
{
    public static readonly Color DEFAULT_HIGHLIGHT_COLOR = UICommon.DefaultUIBorderMouseOver;
    
    /// <summary>
    ///     Gets the color of the image highlight.
    /// </summary>
    public readonly Color Color { get; } 

    /// <summary>
    ///     Gets a value indicating whether image highlighting is enabled.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if image highlighting are enabled; otherwise, <see langword="false"/>.
    /// </value>
    public readonly bool Enabled { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageHighlightSettings"/> struct with the specified color.
    /// </summary>
    /// <param name="color">
    ///     The color of the image highlight.
    /// </param>
    public ImageHighlightSettings(Color color)
    {
        Color = color;

        Enabled = true;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageHighlightSettings"/> struct with the default highlight color.
    /// </summary>
    public ImageHighlightSettings() : this(DEFAULT_HIGHLIGHT_COLOR) { }
}

public class Image : Element
{
    private Asset<Texture2D> asset;

    private Rectangle? frame;
    
    private float opacity = 1f;
    
    private Vector2 origin = new Vector2(0.5f);

    /// <summary>
    ///     Gets or sets the highlight settings of the image.
    /// </summary>
    public ImageHighlightSettings Highlight { get; set; }

    /// <summary>
    ///     Gets or sets the texture asset of the image.
    /// </summary>
    public Asset<Texture2D> Asset
    {
        get => asset;
        set
        {
            asset = value;
            
            Resize();
        }
    }

    /// <summary>
    ///     Gets or sets the frame of the image.
    /// </summary>
    public Rectangle? Frame
    {
        get => frame;
        set
        {
            frame = value;
            
            Resize();
        }
    }

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
    ///     Gets or sets the normalized origin of the image.
    /// </summary>
    /// <value>
    ///     A value in the range of <c>[(0f, 0f) - (1f, 1f)]</c>, where <c>(0f, 0f)</c>
    ///     represents the top-left corner of the image, and <c>(1f, 1f)</c>
    ///     represents the bottom-right corner.
    /// </value>
    public Vector2 Origin
    {
        get => origin;
        set => origin = Vector2.Clamp(value, Vector2.Zero, Vector2.One);
    }

    /// <summary>
    ///     Gets or sets the sprite effects of the image.
    /// </summary>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    
    /// <summary>
    ///     Gets or sets a value indicating whether the image is stretched to fill its dimensions.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the image is stretched to fill its dimensions; otherwise, <see langword="false"/>.
    /// </value>
    public bool Stretch { get; set; }
    
    /// <summary>
    ///     Gets the texture of the image.
    /// </summary>
    public Texture2D Texture => Asset.Value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Image"/> <see langword="class"/> with the specified texture asset and frame.
    /// </summary>
    /// <param name="asset">
    ///     The texture asset of the image.
    /// </param>
    /// <param name="frame">
    ///     The frame of the image.
    /// </param>
    private Image(Asset<Texture2D> asset, Rectangle? frame = null)
    {
        Asset = asset;
        Frame = frame;
   
        Resize();
    }
    
    public override void Recalculate()
    {
        base.Recalculate();

        Resize();
    }
    
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        var dimensions = GetDimensions();
        
        var size = Frame.HasValue ? Frame.Value.Size() : Texture.Size();
        var position = dimensions.Position() + size * Origin;
        
        position = position.Floor();
        
        var scale = Scale;
        
        if (size.X > dimensions.Width)
        {
            scale *= dimensions.Width / size.X;
        }

        if (Highlight.Enabled && IsMouseHovering)
        {
            position = position.Floor();

            var parameters = spriteBatch.Capture() with
            {
                SpriteSortMode = SpriteSortMode.Immediate
            };

            using var scope = spriteBatch.Scope(in parameters);

            Main.pixelShader.CurrentTechnique.Passes["ColorOnly"].Apply();
        
            spriteBatch.Draw(Asset.Value, position + new Vector2(0f, 2f), Frame, Highlight.Color * Opacity, Rotation, size * Origin, scale, Effects, 0f);
            spriteBatch.Draw(Asset.Value, position + new Vector2(0f, -2f), Frame, Highlight.Color * Opacity, Rotation, size * Origin, scale, Effects, 0f);
            spriteBatch.Draw(Asset.Value, position + new Vector2(2f, 0f), Frame, Highlight.Color * Opacity, Rotation, size * Origin, scale, Effects, 0f);
            spriteBatch.Draw(Asset.Value, position + new Vector2(-2f, 0f), Frame, Highlight.Color * Opacity, Rotation, size * Origin, scale, Effects, 0f);
        }
        
        spriteBatch.Draw(Texture, position, Frame, Color * Opacity, Rotation, size * Origin, scale, Effects, 0f);
    }

    private void Resize()
    {
        if (Stretch)
        {
            return;
        }

        Width.Set((Frame.HasValue ? Frame.Value.Width : Asset.Width()) * Scale, 0f);
        Height.Set(Frame.HasValue ? Frame.Value.Height : Asset.Height() * Scale, 0f);
    }

    public static Image FromAsset(Asset<Texture2D> asset, Rectangle? frame = null) => new(asset, frame);
}

public static class ImageExtensions
{
    public static TElement WithHighlight<TElement>(this TElement element, ImageHighlightSettings settings) where TElement : Image
    {
        element.Highlight = settings;
        
        return element;
    }

    public static TElement WithDefaultHighlight<TElement>(this TElement element) where TElement : Image => element.WithHighlight(new ImageHighlightSettings());
}