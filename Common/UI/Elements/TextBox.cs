using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace QuestBooks.Common.UI.Elements;

public sealed class TextBox : UIElement
{
    private float opacity = 1f;
    
    /// <summary>
    ///     Gets or sets the font of the text box.
    /// </summary>
    public Asset<DynamicSpriteFont> Font { get; set; } = FontAssets.MouseText;
    
    /// <summary>
    ///     Gets or sets the contents of the text box.
    /// </summary>
    public string Contents { get; set; }

    /// <summary>
    ///     Gets or sets the scale of the text box.
    /// </summary>
    public float Scale { get; set; } = 1f;

    /// <summary>
    ///     Gets or sets the rotation of the text.
    /// </summary>
    public float Rotation { get; set; } = 0f;
    
    /// <summary>
    ///     Gets or sets the color of the text box.
    /// </summary>
    public Color Color { get; set; } = Color.White;
    
    /// <summary>
    ///     Gets or sets the opacity of the text box.
    /// </summary>
    /// <value>
    ///     A value in the range of <c>[0f - 1f]</c>, where <c>0f</c> represents fully transparent and <c>1f</c> represents fully opaque.
    /// </value>
    public float Opacity
    {
        get => opacity;
        set => opacity = Math.Clamp(value, 0f, 1f);
    }

    public TextBox() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TextBox"/> <see langword="class"/>.
    /// </summary>
    /// <param name="contents">
    ///     The value of the text box.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     <paramref name="contents"/> is <see langword="null"/> or empty.
    /// </exception>
    public TextBox(string contents)
    {
        ArgumentNullException.ThrowIfNull(contents);    
        
        Contents = contents;
        
        var dimensions = GetInnerDimensions();
        
        var font = Font.Value;
        var wrap = font.CreateWrappedText(Contents, dimensions.Width / Scale);
        var size = ChatManager.GetStringSize(font, wrap, new Vector2(Scale));
        
        Height.Set(size.Y, 0f);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TextBox"/> <see langword="class"/>.
    /// </summary>
    /// <param name="value">
    ///     The value of the text box.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public TextBox(LocalizedText value) : this(value.Value) => ArgumentNullException.ThrowIfNull(value);

    public override void Recalculate()
    {
        base.Recalculate();
        
        if (string.IsNullOrEmpty(Contents))
        {
            return;
        }
        
        var dimensions = GetInnerDimensions();
        
        var font = Font.Value;
        var wrap = font.CreateWrappedText(Contents, dimensions.Width / Scale);
        var size = ChatManager.GetStringSize(font, wrap, new Vector2(Scale));
        
        Height.Set(size.Y, 0f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        if (string.IsNullOrEmpty(Contents))
        {
            return;
        }

        var dimensions = GetInnerDimensions();

        var scale = Scale;
        
        var font = Font.Value;
        var wrap = font.CreateWrappedText(Contents, dimensions.Width / Scale);
        var size = ChatManager.GetStringSize(font, wrap, new Vector2(scale));
        
        var position = dimensions.Position() + new Vector2(2f * Scale, dimensions.Height / 2f * Scale);

        position.Y -= size.Y / 2f * scale;
        
        if (size.X > dimensions.Width)
        {
            scale *= dimensions.Width / size.X;
        }

        var snippets = ChatManager.ParseMessage(wrap, Color).ToArray();
        
        ChatManager.ConvertNormalSnippets(snippets);
        
        ChatManager.DrawColorCodedStringShadow(spriteBatch, font, snippets, position, Color.Black * Opacity, 0f, default, new Vector2(scale), -1f, 1.5f);
        ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position, Color * Opacity, 0f, default, new Vector2(scale), out var _, -1f);
    }
}