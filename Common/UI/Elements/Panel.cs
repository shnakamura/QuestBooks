using QuestBooks.Core.Graphics;
using ReLogic.Content;
using Terraria.ModLoader.UI;

namespace QuestBooks.Common.UI.Elements;

public readonly record struct PanelHighlightSettings
{
    /// <summary>
    ///     The default highlight color of a panel.
    /// </summary>
    public static readonly Color DEFAULT_HIGHLIGHT_COLOR = UICommon.DefaultUIBorderMouseOver;
    
    /// <summary>
    ///     Gets the color of the highlight.
    /// </summary>
    public readonly Color Color { get; }
    
    /// <summary>
    ///     Gets a value indicating whether the highlight is enabled.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the highlight is enabled; otherwise, <see langword="false"/>.
    /// </value>
    public readonly bool Enabled { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PanelHighlightSettings"/> struct with the specified color.
    /// </summary>
    /// <param name="color">
    ///     The color of the highlight.
    /// </param>
    public PanelHighlightSettings(Color color)
    {
        Color = color;

        Enabled = true;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="PanelHighlightSettings"/> struct with the default highlight color.
    /// </summary>
    public PanelHighlightSettings() : this(DEFAULT_HIGHLIGHT_COLOR) { }
}

public readonly record struct PanelEdgeSettings
{
    /// <summary>
    ///     The default corner size of a panel, in pixels.
    /// </summary>
    public const int DEFAULT_CORNER_SIZE = 12;
    
    /// <summary>
    ///     The default side size of a panel, in pixels.
    /// </summary>
    public const int DEFAULT_SIDE_SIZE = 4;
    
    /// <summary>
    ///     Gets the size of the corner of the panel, in pixels.
    /// </summary>
    public readonly int Corner { get; }

    /// <summary>
    ///     Gets the size of the side of the panel, in pixels.
    /// </summary>
    public readonly int Side { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PanelEdgeSettings"/> struct with the specified corner and edge sizes.
    /// </summary>
    /// <param name="corner">
    ///     The size of the corner of the panel, in pixels.
    /// </param>
    /// <param name="side">
    ///     The size of the side of the panel, in pixels.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public PanelEdgeSettings(int corner, int side)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(corner);
        ArgumentOutOfRangeException.ThrowIfNegative(side);
        
        Corner = corner;
        Side = side;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PanelEdgeSettings"/> struct with the default corner and edge sizes.
    /// </summary>
    public PanelEdgeSettings() : this(DEFAULT_CORNER_SIZE, DEFAULT_SIDE_SIZE) { }
}

public readonly record struct PanelColorSettings
{
    /// <summary>
    ///     The default background color of a panel.
    /// </summary>
    public static readonly Color DEFAULT_BACKGROUND_COLOR = new Color(63, 82, 151) * 0.7f;
    
    /// <summary>
    ///     The default border color of a panel.
    /// </summary>
    public static readonly Color DEFAULT_BORDER_COLOR = Color.Black;
    
    /// <summary>
    ///     Gets the background color of the panel.
    /// </summary>
    public Color Background { get; }

    /// <summary>
    ///     Gets the border color of the panel.
    /// </summary>
    public Color Border { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PanelColorSettings"/> struct with the specified background and border colors.
    /// </summary>
    /// <param name="background">
    ///     The background color of the panel.
    /// </param>
    /// <param name="border">
    ///     The border color of the panel.
    /// </param>
    public PanelColorSettings(Color background, Color border)
    {
        Background = background;
        Border = border;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PanelColorSettings"/> struct with the default background and border colors.
    /// </summary>
    public PanelColorSettings() : this(DEFAULT_BACKGROUND_COLOR, DEFAULT_BORDER_COLOR) { }
}

// ReSharper disable once LocalFunctionHidesMethod
public class Panel : Element
{
    /// <summary>
    ///     The default background texture asset of a panel.
    /// </summary>
    public static readonly Asset<Texture2D> DEFAULT_BACKGROUND = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground", AssetRequestMode.ImmediateLoad);
    
    /// <summary>
    ///     The default border texture asset of a panel.
    /// </summary>
    public static readonly Asset<Texture2D> DEFAULT_BORDER = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder", AssetRequestMode.ImmediateLoad);
    
    private Asset<Texture2D> background;

    private Asset<Texture2D> border;

    private float opacity = 1f;

    /// <summary>
    ///     Gets or sets the highlight settings of the panel.
    /// </summary>
    public PanelHighlightSettings Highlight { get; set; }

    /// <summary>
    ///     Gets or sets the edge settings of the panel.
    /// </summary>
    public PanelEdgeSettings Edge { get; set; } = new();

    /// <summary>
    ///     Gets or sets the color settings of the panel.
    /// </summary>
    public PanelColorSettings Colors { get; set; } = new();
    
    /// <summary>
    ///     Gets or sets the background texture asset of the panel.
    /// </summary>
    public Asset<Texture2D> Background
    {
        get => background;
        set
        {
            background = value;
            
            Recalculate();
        }
    }
    
    /// <summary>
    ///     Gets or sets the border texture asset of the panel.
    /// </summary>
    public Asset<Texture2D> Border
    {
        get => border;
        set
        {
            border = value;
            
            Recalculate();
        }
    }
    
    /// <summary>
    ///     Gets or sets the opacity of the panel.
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
    ///     Initializes a new instance of the <see cref="Panel"/> class with the specified background and border.
    /// </summary>
    /// <param name="background">
    ///     The background texture asset of the panel.
    /// </param>
    /// <param name="border">
    ///     The border texture asset of the panel.
    /// </param>
    public Panel(Asset<Texture2D> background, Asset<Texture2D> border)
    {
        Background = background;
        Border = border;

        Padding = Edge.Corner;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="Panel"/> class.
    /// </summary>
    public Panel() : this(DEFAULT_BACKGROUND, DEFAULT_BORDER) { }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        var dimensions = GetDimensions();

        var start = new Point((int)dimensions.X, (int)dimensions.Y);
        var end = new Point(start.X + (int)dimensions.Width - Edge.Corner, start.Y + (int)dimensions.Height - Edge.Corner);

        var width = end.X - start.X - Edge.Corner;
        var height = end.Y - start.Y - Edge.Corner;

        var parameters = spriteBatch.Capture() with
        {
            SamplerState = SamplerState.PointClamp
        };
        
        using var scope = spriteBatch.Scope(parameters);

        void Draw(Asset<Texture2D> asset, Color color)
        {
            var texture = asset.Value;

            spriteBatch.Draw(texture, new Rectangle(start.X, start.Y, Edge.Corner, Edge.Corner), new Rectangle(0, 0, Edge.Corner, Edge.Corner), color);
            spriteBatch.Draw(texture, new Rectangle(end.X, start.Y, Edge.Corner, Edge.Corner), new Rectangle(Edge.Corner + Edge.Side, 0, Edge.Corner, Edge.Corner), color);
            spriteBatch.Draw(texture, new Rectangle(start.X, end.Y, Edge.Corner, Edge.Corner), new Rectangle(0, Edge.Corner + Edge.Side, Edge.Corner, Edge.Corner), color);
            spriteBatch.Draw(texture, new Rectangle(end.X, end.Y, Edge.Corner, Edge.Corner), new Rectangle(Edge.Corner + Edge.Side, Edge.Corner + Edge.Side, Edge.Corner, Edge.Corner), color);

            spriteBatch.Draw(texture, new Rectangle(start.X + Edge.Corner, start.Y, width, Edge.Corner), new Rectangle(Edge.Corner, 0, Edge.Side, Edge.Corner), color);
            spriteBatch.Draw(texture, new Rectangle(start.X + Edge.Corner, end.Y, width, Edge.Corner), new Rectangle(Edge.Corner, Edge.Corner + Edge.Side, Edge.Side, Edge.Corner), color);
            spriteBatch.Draw(texture, new Rectangle(start.X, start.Y + Edge.Corner, Edge.Corner, height), new Rectangle(0, Edge.Corner, Edge.Corner, Edge.Side), color);
            spriteBatch.Draw(texture, new Rectangle(end.X, start.Y + Edge.Corner, Edge.Corner, height), new Rectangle(Edge.Corner + Edge.Side, Edge.Corner, Edge.Corner, Edge.Side), color);

            spriteBatch.Draw(texture, new Rectangle(start.X + Edge.Corner, start.Y + Edge.Corner, width, height), new Rectangle(Edge.Corner, Edge.Corner, Edge.Side, Edge.Side), color);
        }

        var backgroundColor = Colors.Background * Opacity;
        var borderColor = (Highlight.Enabled && IsMouseHovering ? Highlight.Color : Colors.Border) * Opacity;
        
        Draw(Background, backgroundColor);
        Draw(Border, borderColor);
    }
}

public static class PanelExtensions
{
    public static Panel Highlight(this Panel panel, Color color)
    {
        panel.Highlight = new PanelHighlightSettings(color);
        
        return panel;
    }

    public static Panel Highlight(this Panel panel) => panel.Highlight(PanelHighlightSettings.DEFAULT_HIGHLIGHT_COLOR);
}