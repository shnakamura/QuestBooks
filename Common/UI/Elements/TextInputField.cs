using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI.Chat;

namespace QuestBooks.Common.UI.Elements;

public class TextInputField : Element
{
    public delegate void TextInputFieldChangeCallback(string contents);
    
    /// <summary>
    ///     Raised when the contents of the text input field are changed.
    /// </summary>
    public event TextInputFieldChangeCallback OnChangeContents;
    
    /// <summary>
    ///     Raised when the text input field begins writing.
    /// </summary>
    public event Action OnBeginWriting;

    /// <summary>
    ///     Raised when the text input field ends writing.
    /// </summary>
    public event Action OnEndWriting;
    
    private string contents;
    
    /// <summary>
    ///     Gets a value indicating whether the text input field is writing.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the text input field is writing; otherwise, <see langword="false"/>.
    /// </value>
    public bool Writing { get; private set; }

    /// <summary>
    ///     Gets or sets the contents of the text input field.
    /// </summary>
    public string Contents
    {
        get => contents;
        set
        {
            contents = value;
            
            OnChangeContents?.Invoke(contents);
        }
    }
    
    /// <summary>
    ///     Gets a value indicating whether the text input field is empty.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the text input field is empty; otherwise, <see langword="false"/>.
    /// </value>
    public bool Empty => Contents == string.Empty;

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        
        Write();
        
        if (string.IsNullOrEmpty(Contents))
        {
            return;
        }
        
        var dimensions = GetInnerDimensions();

        var Scale = 1f;
        var Color = Microsoft.Xna.Framework.Color.White;
        var Opacity = 1f;

        var position = dimensions.Position() + new Vector2(0f, (4f + dimensions.Height / 2f) * Scale);

        var font = FontAssets.MouseText.Value;
        var size = ChatManager.GetStringSize(font, Contents, new Vector2(Scale));
        
        var center = new Vector2(size.X * 0f, size.Y / 2f);
        
        var scale = Scale;
        
        if (size.X > dimensions.Width)
        {
            scale *= dimensions.Width / size.X;
        }

        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, Contents, position, Color * Opacity, 0f, center, new Vector2(scale));
    }

    /// <summary>
    ///     Toggles writing to the text input field.
    /// </summary>
    public void Toggle()
    {
        if (Writing)
        {
            End();
        }
        else
        {
            Begin();
        }
    }
    
    /// <summary>
    ///     Begins writing to the text input field.
    /// </summary>
    public void Begin()
    {
        if (Writing)
        {
            return;
        }

        Writing = true;
        
        OnBeginWriting?.Invoke();
    }

    /// <summary>
    ///     Ends writing to the text input field.
    /// </summary>
    public void End()
    {
        if (!Writing)
        {
            return;
        }

        Writing = false;
        
        OnEndWriting?.Invoke();
    }
    
    /// <summary>
    ///     Clears the contents of the text input field.
    /// </summary>
    public void Clear() => Contents = string.Empty;

    private void Write()
    {
        if (!Writing)
        {
            return;
        }
        
        PlayerInput.WritingText = true;
        
        Main.instance.HandleIME();
        Main.CurrentInputTextTakerOverride = this;

        Contents = Main.GetInputText(Contents);
    }
}