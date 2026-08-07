using QuestBooks.Core.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Chat;

namespace QuestBooks.Common.UI.Elements;

// TODO: Implement text cursor positioning.
public sealed class TextInputField : UIElement
{
    public delegate void TextInputFieldChangeCallback(string contents);

    /// <summary>
    ///     Raised when the contents of the text input field are changed.
    /// </summary>
    public event TextInputFieldChangeCallback OnChangeContents;
    
    /// <summary>
    ///     Raised when the text input field starts writing.
    /// </summary>
    public event Action OnStartWriting;

    /// <summary>
    ///     Raised when the text input field stops writing.
    /// </summary>
    public event Action OnStopWriting;
    
    /// <summary>
    ///     Gets a value indicating whether the text input field is writing.
    /// </summary>
    public bool Writing { get; private set; }
    
    /// <summary>
    ///     Gets or sets the capacity of the text input field, in characters.
    /// </summary>
    public int Capacity { get; set; }

    public bool Ticker { get; init; } = true;
    
    public string Placeholder { get; set; } = string.Empty;

    public float Scale { get; set; } = 1f;

    public string Contents { get; private set; } = string.Empty;

    public bool Empty => Contents == string.Empty;

    /// <summary>
    ///     Gets or sets the font of the text.
    /// </summary>
    public Asset<DynamicSpriteFont> Font { get; set; } = FontAssets.MouseText;
    
    public string Display
    {
        get
        {
            var contents = Empty && !Writing ? Placeholder : Contents;

            if (Ticker && Writing && Main.GameUpdateCount % 60 < 30)
            {
                contents += "|";
            }

            return contents;
        }
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        
        ToggleWriting();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        if (!Writing)
        {
            return;
        }
        
        UpdateWriting();
    }

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

        if (!Writing)
        {
            return;
        }
        
        UpdateWriting();
    }
    
    public void SetContents(string contents)
    {
        ArgumentNullException.ThrowIfNull(contents);
        
        if (Contents == contents)
        {
            return;
        }

        Contents = contents;
        
        OnChangeContents?.Invoke(contents);
        
        if (Contents.Length <= Capacity)
        {
            return;
        }
        
        Contents = Contents[..Capacity];
    }
    
    public void ClearContents()
    {
        Contents = string.Empty;
        
        OnChangeContents?.Invoke(string.Empty);
    }

    public void ToggleWriting(bool clear = false, bool sound = true)
    {
        if (Writing)
        {
            StopWriting(clear, sound);
        }
        else
        {
            StartWriting(clear, sound);
        }
    }

    public void StartWriting(bool clear = false, bool sound = true)
    {
        if (clear)
        {
            ClearContents();
        }
        
        if (Writing)
        {
            return;
        }
        
        Writing = true;
        
        OnStartWriting?.Invoke();
        
        if (!sound)
        {
            return;
        }

        SoundEngine.PlaySound(in SoundID.MenuOpen);
    }

    public void StopWriting(bool clear = false, bool sound = true)
    {
        if (clear)
        {
            ClearContents();
        }
        
        if (!Writing)
        {
            return;
        }
        
        Writing = false;
        
        OnStopWriting?.Invoke();
        
        if (!sound)
        {
            return;
        }

        SoundEngine.PlaySound(in SoundID.MenuClose); 
    }

    private void UpdateWriting()
    {
        if (!Writing)
        {
            return;
        }
        
        PlayerInput.WritingText = true;
        
        Main.instance.HandleIME();
        Main.CurrentInputTextTakerOverride = this;

        SetContents(Main.GetInputText(Contents));

        var escape = Main.inputTextEnter || Main.inputTextEscape;
        
        if (!escape)
        {
            return;
        }

        StopWriting();
    }
}