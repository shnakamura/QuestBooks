using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace QuestBooks.Common.UI.Elements;

public class Text : Element
{
    private Vector2 origin = new Vector2(0.5f);

    private float opacity = 1f;
    
    /// <summary>
    ///     Gets or sets the font of the text.
    /// </summary>
    public Asset<DynamicSpriteFont> Font { get; set; } = FontAssets.MouseText;
    
    /// <summary>
    ///     Gets or sets the contents of the text.
    /// </summary>
    public string Contents { get; set; }

    /// <summary>
    ///     Gets or sets the scale of the text.
    /// </summary>
    public float Scale { get; set; } = 1f;

    /// <summary>
    ///     Gets or sets the normalized origin of the text.
    /// </summary>
    /// <value>
    ///     A value in the range of <c>[(0f, 0f) - (1f, 1f)]</c>, where <c>(0f, 0f)</c>
    ///     represents the top-left corner of the text and <c>(1f, 1f)</c>
    ///     represents the bottom-right corner.
    /// </value>
    public Vector2 Origin
    {
        get => origin;
        set => origin = Vector2.Clamp(value, Vector2.Zero, Vector2.One);
    }
    
    /// <summary>
    ///     Gets or sets the rotation of the text.
    /// </summary>
    public float Rotation { get; set; }
    
    /// <summary>
    ///     Gets or sets the color of the text.
    /// </summary>
    public Color Color { get; set; } = Color.White;
    
    /// <summary>
    ///     Gets or sets the opacity of the text.
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
    ///     Initializes a new instance of the <see cref="Text"/> <see langword="class"/>.
    /// </summary>
    public Text() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Text"/> <see langword="class"/> with the specified contents.
    /// </summary>
    /// <param name="contents">
    ///     The contents of the text.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     <paramref name="contents"/> is <see langword="null"/> or empty.
    /// </exception>
    public Text(string contents)
    {
        ArgumentException.ThrowIfNullOrEmpty(contents);    
        
        Contents = contents;
        
        Resize();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Text"/> <see langword="class"/> with the specified localized text.
    /// </summary>
    /// <param name="text">
    ///     The localized text of the text.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    public Text(LocalizedText text) : this(text.Value) => ArgumentNullException.ThrowIfNull(text);

    public override void Recalculate()
    {
        base.Recalculate();
        
        Resize();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (string.IsNullOrEmpty(Contents))
        {
            return;
        }
        
        var dimensions = GetInnerDimensions();

        var position = dimensions.Position() + new Vector2(0f, (4f + dimensions.Height / 2f) * Scale);
        
        var font = Font.Value;
        var size = ChatManager.GetStringSize(font, Contents, new Vector2(Scale));
        
        var center = new Vector2(size.X * 0f, size.Y / 2f);
        
        var scale = Scale;
        
        if (size.X > dimensions.Width)
        {
            scale *= dimensions.Width / size.X;
        }

        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, Contents, position, Color * Opacity, 0f, center, new Vector2(scale));
    }

    private void Resize()
    {
        if (string.IsNullOrEmpty(Contents))
        {
            return;
        }

        var font = Font.Value;
        var size = ChatManager.GetStringSize(font, Contents, new Vector2(Scale));
        
        Width.Set(size.X + 2f * Scale, 0f);
        Height.Set(size.Y * Scale - 4f, 0f);
    }
    
    public static Text FromLiteral(string contents) => new(contents);

    public static Text FromKey(string key) => new(Language.GetText(key));
    
    public static Text FromLocalization(LocalizedText text) => new(text);
}

public static class TextExtensions
{
    public static TText WithScale<TText>(this TText text, float scale) where TText : Text
    {
        text.Scale = scale;

        return text;
    }
}