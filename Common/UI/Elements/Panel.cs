using QuestBooks.Core.Graphics;
using ReLogic.Content;
using Terraria.ModLoader.UI;

namespace QuestBooks.Common.UI;

public sealed class Panel : Element
{
    public static Panel Full => new Panel().WithFullDimensions();

    public static Panel Empty => new();
    
    private float _opacity = 1f;
    
    /// <summary>
    ///     Gets or sets the texture asset of the panel.
    /// </summary>
    public Asset<Texture2D> Asset { get; set; }

    /// <summary>
    ///     Gets or sets the size of the corner of the panel, in pixels.
    /// </summary>
    public int Corner { get; set; } = 12;

    /// <summary>
    ///     Gets the size of the side of the panel, in pixels.
    /// </summary>
    public int Side { get; set; } = 4;
    
    /// <summary>
    ///     Gets or sets the highlight color of the panel.
    /// </summary>
    public Color Highlight { get; set; }

    /// <summary>
    ///     Gets or sets the border color of the panel.
    /// </summary>
    public Color Border { get; set; } = UICommon.DefaultUIBorder;

    /// <summary>
    ///     Gets or sets the background color of the panel.
    /// </summary>
    public Color Background { get; set; } = UICommon.DefaultUIBlue;
    
    /// <summary>
    ///     Gets or sets the opacity of the panel.
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
    ///     Gets the texture of the panel.
    /// </summary>
    public Texture2D Texture => Asset.Value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Panel"/> class.
    /// </summary>
    private Panel() : this(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/Panel", AssetRequestMode.ImmediateLoad)) { }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="Panel"/> class with the specified texture asset.
    /// </summary>
    /// <param name="asset">
    ///     The texture asset of the panel.
    /// </param>
    private Panel(Asset<Texture2D> asset)
    {
        Asset = asset;

        SetPadding(Corner);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Panel"/> class with the specified texture asset.
    /// </summary>
    /// <param name="path">
    ///     The path of the texture asset of the panel.
    /// </param>
    private Panel(string path)
    {
        Asset = ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad);
        
        SetPadding(Corner);
    }

    /// <inheritdoc/>
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        var dimensions = GetDimensions();
        var bounds = dimensions.ToRectangle();
        
        var parameters = spriteBatch.Capture() with
        {
           SamplerState = SamplerState.PointClamp
        };

        using var scope = spriteBatch.Scope(in parameters);

        var background = Background * Opacity;
        var border = (IsMouseHovering && Highlight != Color.Transparent ? Highlight : Border) * Opacity;
        
        PanelUtilities.Draw(Texture, bounds, Texture.Frame(2, 1, 1), in background, Corner, Side); 
        PanelUtilities.Draw(Texture, bounds, Texture.Frame(2), in border, Corner, Side); 
    }

    public static Panel FromPath(string path) => new(path);
    
    public static Panel FromAsset(Asset<Texture2D> asset) => new(asset);
}

public static class PanelExtensions
{
    public static Panel WithBackgroundColor(this Panel panel, in Color color)
    {
        panel.Background = color;
        
        return panel;
    }
    
    public static Panel WithBorderColor(this Panel panel, in Color color)
    {
        panel.Border = color;
        
        return panel;
    }
    
    public static Panel WithHighlight(this Panel panel, in Color color)
    {
        panel.Highlight = color;
        
        return panel;
    }
}

public static class PanelUtilities
{
    private static SpriteBatch Batch => Main.spriteBatch;
    
    public static void Draw(Texture2D texture, Rectangle bounds, Rectangle frame, in Color color, int corner, int side)
    {
        corner = Math.Min(corner, Math.Min(bounds.Width, bounds.Height) / 2);
        
        var start = bounds.Location;
        var end = new Point(bounds.Right - corner, bounds.Bottom - corner);

        var width = bounds.Width - corner * 2;
        var height = bounds.Height - corner * 2;

        Batch.Draw(texture, new Rectangle(start.X, start.Y, corner, corner), new Rectangle(frame.X, frame.Y, corner, corner), color);
        Batch.Draw(texture, new Rectangle(end.X, start.Y, corner, corner), new Rectangle(frame.Right - corner, frame.Y, corner, corner), color);
        Batch.Draw(texture, new Rectangle(start.X, end.Y, corner, corner), new Rectangle(frame.X, frame.Bottom - corner, corner, corner), color);
        Batch.Draw(texture, new Rectangle(end.X, end.Y, corner, corner), new Rectangle(frame.Right - corner, frame.Bottom - corner, corner, corner), color);

        Batch.Draw(texture, new Rectangle(start.X + corner, start.Y, width, corner), new Rectangle(frame.X + corner, frame.Y, side, corner), color);
        Batch.Draw(texture, new Rectangle(start.X + corner, end.Y, width, corner), new Rectangle(frame.X + corner, frame.Bottom - corner, side, corner), color);
        Batch.Draw(texture, new Rectangle(start.X, start.Y + corner, corner, height), new Rectangle(frame.X, frame.Y + corner, corner, side), color);
        Batch.Draw(texture, new Rectangle(end.X, start.Y + corner, corner, height), new Rectangle(frame.Right - corner, frame.Y + corner, corner, side), color);

        Batch.Draw(texture, new Rectangle(start.X + corner, start.Y + corner, width, height), new Rectangle(frame.X + corner, frame.Y + corner, side, side), color);
    }
}