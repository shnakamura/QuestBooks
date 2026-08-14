using Terraria.GameInput;

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

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Write();
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

    public static implicit operator string(TextInputField field) => field.Contents;
}