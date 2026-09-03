using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace QuestBooks.Common.UI;

public class Text : Element
{
    /// <summary>
    ///     Gets an empty text.
    /// </summary>
    public static Text Empty => new();
    
    private Asset<DynamicSpriteFont> _asset = FontAssets.MouseText;
    
    private float _opacity = 1f;

    private float _scale = 1f;

    private string _contents;

    /// <summary>
    ///     Gets or sets the font asset of the text.
    /// </summary>
    public Asset<DynamicSpriteFont> Asset
    {
        get => _asset;
        set
        {
            _asset = value;
            
            Resize();
        }
    }

    /// <summary>
    ///     Gets or sets the contents of the text.
    /// </summary>
    public string Contents
    {
        get => _contents;
        set
        {
            _contents = value;
            
            Resize();
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
            
            Resize();
        }
    }

    /// <summary>
    ///     Gets or sets the rotation of the text, in radians.
    /// </summary>
    public float Rotation { get; set; }
    
    /// <summary>
    ///     Gets or sets the opacity of the text.
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
    ///     Gets or sets the color of the text.
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Gets or sets a value indicating whether the text should fit within the bounds of the element.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the text should fit within the bounds of the element; otherwise, <see langword="false"/>.
    /// </value>
    public bool Fit { get; set; } = true;
    
    /// <summary>
    ///     Gets the font of the text.
    /// </summary>
    public DynamicSpriteFont Font => Asset.Value;
    
    /// <summary>
    ///     Gets a value indicating whether the contents of the text are empty.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the contents of the text are empty; otherwise, <see langword="false"/>.
    /// </value>
    public bool Blank => string.IsNullOrEmpty(Contents);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Text"/> class.
    /// </summary>
    private Text() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Text"/> class with the specified contents.
    /// </summary>
    /// <param name="contents">
    ///     The contents of the text.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     <paramref name="contents"/> is <see langword="null"/> or empty.
    /// </exception>
    private Text(string contents)
    {
        ArgumentException.ThrowIfNullOrEmpty(contents);    
        
        Contents = contents;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Text"/> class with the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized text of the text.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    private Text(LocalizedText text) : this(text.Value) => ArgumentNullException.ThrowIfNull(text);

    public override void Recalculate()
    {
        base.Recalculate();
        
        Resize();
    }

    protected override void Draw(in ElementDrawContext context)
    {
        base.Draw(in context);

        if (Blank || !context.Self)
        {
            return;
        }
        
        var size = ChatManager.GetStringSize(Font, Contents, new Vector2(Scale));
        var dimensions = GetInnerDimensions();

        var fit = Fit && size.X > dimensions.Width;
        var scale = new Vector2(fit ? Scale * dimensions.Width / size.X : Scale);

        var origin = new Vector2(0f, size.Y / 2f);
        var position = dimensions.Position() + origin;

        ChatManager.DrawColorCodedStringWithShadow(context.Batch, Font, Contents, position, Color * Opacity, Rotation, origin, scale);
    }
    
    /// <summary>
    ///     Resizes the text to fit its contents.
    /// </summary>
    public void Resize()
    {
        if (Blank)
        {
            return;
        }
        
        var size = ChatManager.GetStringSize(Font, Contents, new Vector2(Scale));
        
        Width.Set(size.X, 0f);
        Height.Set(size.Y, 0f);
    }

    /// <summary>
    ///     Returns a new <see cref="Text"/> with the specified contents.
    /// </summary>
    /// <param name="contents">
    ///     The contents of the text.
    /// </param>
    /// <returns>
    ///     A new <see cref="Text"/> with the specified contents.
    /// </returns>
    public static Text FromLiteral(string contents) => new(contents);

    /// <summary>
    ///     Returns a new <see cref="Text"/> with the specified localization key.
    /// </summary>
    /// <param name="key">
    ///     The localization key of the text.
    /// </param>
    /// <returns>
    ///     A new <see cref="Text"/> with the specified localization key.
    /// </returns>
    public static Text FromKey(string key) => new(Language.GetText(key));

    /// <summary>
    ///     Returns a new <see cref="Text"/> with the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized text.
    /// </param>
    /// <returns>
    ///     A new <see cref="Text"/> with the specified localized text.
    /// </returns>
    public static Text FromLocalization(LocalizedText text) => new(text);
}

public static class TextExtensions
{
    public static TText WithRotation<TText>(this TText text, float rotation) where TText : Text
    {
        text.Rotation = rotation;

        return text;
    }
    
    public static TText WithOpacity<TText>(this TText text, float opacity) where TText : Text
    {
        text.Opacity = opacity;

        return text;
    }
    
    public static TText WithColor<TText>(this TText text, Color color) where TText : Text
    {
        text.Color = color;

        return text;
    }
    
    public static TText WithFont<TText>(this TText text, Asset<DynamicSpriteFont> font) where TText : Text
    {
        text.Asset = font;

        return text;
    }
    
    public static TText WithScale<TText>(this TText text, float scale) where TText : Text
    {
        text.Scale = scale;

        return text;
    }
}