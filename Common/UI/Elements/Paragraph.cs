using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace QuestBooks.Common.UI;

public sealed class Paragraph : Element
{
    /// <summary>
    ///     Gets an empty paragraph with full dimensions.
    /// </summary>
    public static Paragraph Full => new Paragraph().WithWidth(StyleDimension.FromPercent(1f));
    
    /// <summary>
    ///     Gets an empty paragraph.
    /// </summary>
    public static Paragraph Empty => new();
    
    private Asset<DynamicSpriteFont> asset = FontAssets.MouseText;
    
    private float _opacity = 1f;
    
    private float _scale = 1f;
    
    private string _contents;
    
    /// <summary>
    ///     Gets or sets the font asset of the paragraph.
    /// </summary>
    public Asset<DynamicSpriteFont> Asset
    {
        get => asset;
        set
        {
            asset = value;
            
            Recalculate();
        }
    }
    
    /// <summary>
    ///     Gets or sets the contents of the paragraph.
    /// </summary>
    public string Contents
    {
        get => _contents;
        set
        {
            if (_contents == value)
            {
                return;
            }
            
            _contents = value;
            
            Recalculate();
        }
    }

    /// <summary>
    ///     Gets or sets the scale of the text.
    /// </summary>
    public float Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            
            Recalculate();
        }
    }

    /// <summary>
    ///     Gets or sets the rotation of the paragraph, in radians.
    /// </summary>
    public float Rotation { get; set; }
    
    /// <summary>
    ///     Gets or sets the opacity of the paragraph.
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
    ///     Gets or sets the color of the paragraph.
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Gets the font of the paragraph.
    /// </summary>
    public DynamicSpriteFont Font => asset.Value;
    
    /// <summary>
    ///     Gets a value indicating whether the contents of the paragraph are empty.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the contents of the paragraph are empty; otherwise, <see langword="false"/>.
    /// </value>
    public bool Blank => string.IsNullOrEmpty(Contents);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Paragraph"/> class.
    /// </summary>
    private Paragraph() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Paragraph"/> class with the specified contents.
    /// </summary>
    /// <param name="contents">
    ///     The contents of the paragraph.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="contents"/> is <see langword="null"/>.
    /// </exception>
    private Paragraph(string contents)
    {
        ArgumentNullException.ThrowIfNull(contents);    
        
        Contents = contents;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Paragraph"/> class with the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized text.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    private Paragraph(LocalizedText text) : this(text.Value) => ArgumentNullException.ThrowIfNull(text);
    
    /// <inheritdoc/>
    public override void Recalculate()
    {
        base.Recalculate();
        
        if (Blank)
        {
            return;
        }
        
        var dimensions = GetInnerDimensions();
        
        var wrap = Font.CreateWrappedText(Contents, dimensions.Width / Scale);
        var size = ChatManager.GetStringSize(Font, wrap, new Vector2(Scale));
        
        Height.Set(size.Y, 0f);
    }

    /// <inheritdoc/>
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        if (Blank)
        {
            return;
        }

        var dimensions = GetInnerDimensions();
        var position = dimensions.Position();

        var wrap = Font.CreateWrappedText(Contents, dimensions.Width / Scale);
        var snippets = ChatManager.ParseMessage(wrap, Color).ToArray();
        
        ChatManager.ConvertNormalSnippets(snippets);
        
        ChatManager.DrawColorCodedStringShadow(spriteBatch, Font, snippets, position, Color.Black * Opacity, Rotation, default, new Vector2(Scale));
        ChatManager.DrawColorCodedString(spriteBatch, Font, snippets, position, Color * Opacity, Rotation, default, new Vector2(Scale), out var _, -1f);
    }

    /// <summary>
    ///     Returns a new <see cref="Paragraph"/> with the specified contents.
    /// </summary>
    /// <param name="contents">
    ///     The contents of the paragraph.
    /// </param>
    /// <returns>
    ///     A new <see cref="Paragraph"/> with the specified contents.
    /// </returns>
    public static Paragraph FromLiteral(string contents) => new(contents);

    /// <summary>
    ///     Returns a new <see cref="Paragraph"/> with the specified localization key.
    /// </summary>
    /// <param name="key">
    ///     The localization key of the paragraph.
    /// </param>
    /// <returns>
    ///     A new <see cref="Paragraph"/> with the specified localization key.
    /// </returns>
    public static Paragraph FromKey(string key) => new(Language.GetText(key));

    /// <summary>
    ///     Returns a new <see cref="Paragraph"/> with the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized text.
    /// </param>
    /// <returns>
    ///     A new <see cref="Paragraph"/> with the specified localized text.
    /// </returns>
    public static Paragraph FromLocalization(LocalizedText text) => new(text);
}

public static class ParagraphExtensions
{
    public static Paragraph WithRotation(this Paragraph paragraph, float rotation)
    {
        paragraph.Rotation = rotation;

        return paragraph;
    }
    
    public static Paragraph WithOpacity(this Paragraph paragraph, float opacity)
    {
        paragraph.Opacity = opacity;

        return paragraph;
    }
    
    public static Paragraph WithColor(this Paragraph paragraph, Color color)
    {
        paragraph.Color = color;

        return paragraph;
    }
    
    public static Paragraph WithFont(this Paragraph paragraph, Asset<DynamicSpriteFont> font)
    {
        paragraph.Asset = font;

        return paragraph;
    }
    
    public static Paragraph WithScale(this Paragraph paragraph, float scale)
    {
        paragraph.Scale = scale;

        return paragraph;
    }
}