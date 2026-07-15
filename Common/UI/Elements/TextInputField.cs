using Terraria.Audio;
using Terraria.GameInput;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class TextInputField : TextField
{
    /// <summary>
    ///     Occurs when the text input field starts writing.
    /// </summary>
    public event Action OnStartWriting;

    /// <summary>
    ///     Occurs when the text input field stops writing.
    /// </summary>
    public event Action OnStopWriting;
    
    /// <summary>
    ///     Gets the maximum number of characters that can be entered into this text input field.
    /// </summary>
    public int Capacity { get; init; } = 50;

    /// <summary>
    ///     Gets or sets a value indicating whether a ticker should be displayed when text is being written to this text input field.
    /// </summary>
    public bool Ticker { get; init; } = true;

    /// <summary>
    ///     Gets a value indicating whether text is currently being written to this text input field.
    /// </summary>
    public bool Writing { get; protected set; }

    public override string Display
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

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);

        SoundEngine.PlaySound(in SoundID.MenuTick);
    }

    public override void MouseOut(UIMouseEvent evt)
    {
        base.MouseOut(evt);
        
        SoundEngine.PlaySound(in SoundID.MenuTick);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!Writing)
        {
            return;
        }

        PlayerInput.WritingText = true;
        
        Main.CurrentInputTextTakerOverride = this;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        if (!Writing)
        {
            return;
        }
        
        PlayerInput.WritingText = true;
        
        Main.instance.HandleIME();

        SetContents(Main.GetInputText(Contents));
        
        if (!Main.inputTextEnter && !Main.inputTextEscape)
        {
            return;
        }

        StopWriting();
    }

    public override void SetContents(string contents)
    {
        base.SetContents(contents);
        
        if (Contents.Length <= Capacity)
        {
            return;
        }
        
        Contents = Contents[..Capacity];
    }

    /// <summary>
    ///     Toggles whether text is currently being written to this text input field.
    /// </summary>
    /// <param name="clear">
    ///     Whether to clear the contents of this text input field if stopping writing.
    /// </param>
    /// <param name="sound">
    ///     Whether to play a sound when toggling the writing state.
    /// </param>
    public virtual void ToggleWriting(bool clear = false, bool sound = true)
    {
        if (Writing)
        {
            StopWriting(clear, sound);
        }
        else
        {
            StartWriting(sound);
        }
    }

    /// <summary>
    ///     Begins writing text to this text input field.
    /// </summary>
    /// <param name="sound">
    ///     Whether to play a sound when starting to write.
    /// </param>
    public virtual void StartWriting(bool sound = true)
    {
        Writing = true;
        
        OnStartWriting?.Invoke();
        
        if (!sound)
        {
            return;
        }

        SoundEngine.PlaySound(in SoundID.MenuOpen);
    }

    /// <summary>
    ///     Stops writing text to this text input field.
    /// </summary>
    /// <param name="clear">
    ///     Whether to clear the contents of this text input field when stopping writing.
    /// </param>
    /// <param name="sound">
    ///     Whether to play a sound when stopping writing.
    /// </param>
    public virtual void StopWriting(bool clear = false, bool sound = true)
    {
        if (clear)
        {
            ClearContents();
        }

        Writing = false;
        
        OnStopWriting?.Invoke();
        
        if (!sound)
        {
            return;
        }

        SoundEngine.PlaySound(in SoundID.MenuClose); 
    }
}