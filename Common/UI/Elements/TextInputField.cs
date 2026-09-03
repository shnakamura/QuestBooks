using QuestBooks.Core.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Chat;

namespace QuestBooks.Common.UI;

/// <summary>
///     Represents a callback invoked when the contents of a text input field are changed.
/// </summary>
/// <param name="contents">
///     The contents of the text input field.
/// </param>
public delegate void TextInputFieldChangeCallback(string contents);

public class TextInputField : Element
{
    private static readonly RasterizerState RASTERIZER_STATE = new()
    {
        CullMode = CullMode.None,
        ScissorTestEnable = true
    };

    /// <summary>
    ///     Gets an empty text input field with full dimensions.
    /// </summary>
    public static TextInputField Full => new TextInputField().WithFullDimensions();
    
    /// <summary>
    ///     Gets an empty text input field.
    /// </summary>
    public static TextInputField Empty => new();
    
    private float _opacity = 1f;
    
    private string _contents;
    
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

    /// <summary>
    ///     Gets or sets the font asset of the text.
    /// </summary>
    public Asset<DynamicSpriteFont> Asset { get; set; } = FontAssets.MouseText;

    /// <summary>
    ///     Gets or sets the contents of the text input field.
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
            
            OnChangeContents?.Invoke(_contents);
        }
    }

    /// <summary>
    ///     Gets or sets the scale of the text.
    /// </summary>
    public float Scale { get; set; } = 1f;
    
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
        get => _opacity;
        set => _opacity = Math.Clamp(value, 0f, 1f);
    }

    public bool Ticker { get; set; } = true;
    
    /// <summary>
    ///     Gets a value indicating whether the text input field is writing.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the text input field is writing; otherwise, <see langword="false"/>.
    /// </value>
    public bool Writing { get; private set; }
    
    /// <summary>
    ///     Gets the font of the text input field.
    /// </summary>
    public DynamicSpriteFont Font => Asset.Value;

    /// <summary>
    ///     Gets a value indicating whether the contents of the text input field are empty.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the contents of the text input field are empty; otherwise, <see langword="false"/>.
    /// </value>
    public bool Blank => string.IsNullOrEmpty(Contents);

    /// <summary>
    ///     Initializes a new instance of the <see cref="TextInputField"/> class.
    /// </summary>
    public TextInputField() { }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        
        Begin();
    }

    protected override void Draw(in ElementDrawContext context)
    {
        base.Draw(in context);

        if (!context.Self)
        {
            return;
        }
        
        var batch = context.Batch;
        var device = context.Device;
        
        var snapshot = batch.Capture();
        
        batch.End();
        
        Write();

        var active = Writing || !Blank;

        var contents = active ? Contents : "Search";
        var color = active ? Color : Color.Gray;

        var dimensions = GetInnerDimensions();
        var position = dimensions.Position();

        var scale = new Vector2(Scale);
        var size = ChatManager.GetStringSize(Font, contents, scale);
        
        const int padding = 2;
        
        if (size.X > dimensions.Width - padding)
        {
            position.X -= size.X - dimensions.Width + padding;
        }

        var origin = new Vector2(0f, size.Y / 2f);

        position.Y += size.Y / 2f + 4f;

        var scissor = device.ScissorRectangle;
        var bounds = GetClippingRectangle(batch);
        
        bounds.Inflate(padding, padding);

        device.ScissorRectangle = bounds;

        var parameters = snapshot with
        {
            RasterizerState = RASTERIZER_STATE
        };
        
        batch.Begin(in parameters);
        
        ChatManager.DrawColorCodedStringWithShadow(batch, Font, contents, position, color * Opacity, 0f, origin, scale);
        
        batch.End();
        
        device.ScissorRectangle = scissor;
        
        batch.Begin(in snapshot);
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
        
        if (!Main.mouseLeft || ContainsPoint(Main.MouseScreen))
        {
            return;
        }

        End();
    }
}