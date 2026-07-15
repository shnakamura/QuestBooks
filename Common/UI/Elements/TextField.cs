using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;

namespace QuestBooks.Common.UI.Elements;

public class TextField : UIElement
{
    /// <summary>
    ///     Represents the callback that is invoked when the contents of a <see cref="TextField"/> are changed.
    /// </summary>
    /// <param name="contents">
    ///     The updated contents of the text field.
    /// </param>
    public delegate void TextFieldChangeCallback(in string contents);

    /// <summary>
    ///     Occurs when the contents of the text field are changed.
    /// </summary>
    public event TextFieldChangeCallback OnChangeContents;
    
    /// <summary>
    ///     Gets the font used to render the text in this text field.
    /// </summary>
    public Asset<DynamicSpriteFont> Font { get; init; } = FontAssets.MouseText;

    /// <summary>
    ///     Gets a value indicating whether text tags should be displayed in this text field.
    /// </summary>
    public bool Tags { get; init; } = true;
    
    /// <summary>
    ///     Gets the placeholder text displayed when this text field has no contents.
    /// </summary>
    public string Placeholder { get; init; } = string.Empty;
    
    /// <summary>
    ///     Gets the scale at which the text is rendered in this text field.
    /// </summary>
    public float Scale { get; set; } = 1f;
    
    /// <summary>
    ///     Gets the contents of this text field.
    /// </summary>
    public string Contents { get; protected set; } = string.Empty;
    
    /// <summary>
    ///     Gets a value indicating whether this text field has no contents.
    /// </summary>
    public bool Empty => Contents == string.Empty;
    
    /// <summary>
    ///     Gets the text currently displayed by this text field.
    /// </summary>
    /// <value>
    ///     <see cref="Placeholder"/> if <see cref="Empty"/> is <see langword="true"/>; otherwise, <see cref="Contents"/>.
    /// </value>
    public virtual string Display => Empty ? Placeholder : Contents;

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        var font = Font.Value;
        var scale = new Vector2(Scale);

        var dimensions = GetInnerDimensions();
        
        var position = dimensions.Position() + new Vector2(dimensions.Width * 0f + 2f * Scale, dimensions.Height / 2f + 4f * Scale);

        var size = font.MeasureString(Display);
        var origin = new Vector2(size.X * 0f, size.Y / 2f);

        var color = Empty ? Color.Gray : Color.White;

        // TODO: Ensure rendering doesnt go out of element bounds.
        ChatManager.DrawColorCodedStringWithShadow
        (
            spriteBatch,
            font,
            Display,
            position,
            color,
            0f,
            origin,
            scale
        );
    }

    /// <summary>
    ///     Sets the contents of this text field.
    /// </summary>
    /// <param name="contents">
    ///     The updated contents of this text field.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="contents"/> is <see langword="null"/>.
    /// </exception>
    public virtual void SetContents(string contents)
    {
        ArgumentNullException.ThrowIfNull(contents);

        if (Contents == contents)
        {
            return;
        }

        Contents = contents;
        
        OnChangeContents?.Invoke(in contents);
    }

    /// <summary>
    ///     Clears the contents of this text field.
    /// </summary>
    public virtual void ClearContents()
    {
        Contents = string.Empty;
        
        OnChangeContents?.Invoke(in string.Empty);
    }
}