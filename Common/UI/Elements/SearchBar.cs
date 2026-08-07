using QuestBooks.Common.UI.Components;
using QuestBooks.Common.UI.Layout;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public sealed class SearchBar : UIElement
{
    private sealed class SearchBarSearchButton : ImageButton
    {
        private readonly TextInputField input;

        public override string Tooltip => Language.GetTextValue("Mods.QuestBooks.UI.Common.Buttons.Search");

        public SearchBarSearchButton(TextInputField input) : base(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/SearchIcon"))
        {
            ArgumentNullException.ThrowIfNull(input);
            
            this.input = input;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            
            input.ToggleWriting();
        }
    }
    
    private sealed class SearchBarClearButton : ImageButton
    {
        private readonly TextInputField input;

        public override string Tooltip => Language.GetTextValue("Mods.QuestBooks.UI.Common.Buttons.Clear");

        public SearchBarClearButton(TextInputField input) : base(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/SearchClearIcon"))
        {
            ArgumentNullException.ThrowIfNull(input);
            
            this.input = input;
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            
            input.StopWriting(true);
        }
    }
    
    private readonly TextInputField input;
    
    public event TextInputField.TextInputFieldChangeCallback OnChangeContents
    {
        add => input.OnChangeContents += value;
        remove => input.OnChangeContents -= value;
    }

    public event Action OnStartWriting
    {
        add => input.OnStartWriting += value;
        remove => input.OnStartWriting -= value;
    }
    
    public event Action OnStopWriting
    {
        add => input.OnStopWriting += value;
        remove => input.OnStopWriting -= value;
    }
    
    /// <summary>
    ///     Gets or sets the capacity of the search bar, in characters.
    /// </summary>
    public int Capacity
    {
        get => input.Capacity;
        set => input.Capacity = value;
    }

    public float Scale
    {
        get => input.Scale;
        set => input.Scale = value;
    }

    public string Placeholder
    {
        get => input.Placeholder;
        set => input.Placeholder = value;
    }
    
    /// <summary>
    ///     Gets a value indicating whether the search bar is writing.
    /// </summary>
    public bool Writing => input.Writing;

    public SearchBar()
    {
        Append(new SettingsPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        });

        var stack = new JustifiedHorizontalStack
        {
            PaddingTop = 8f,
            PaddingLeft = 8f,
            PaddingBottom = 8f,
            PaddingRight = 8f,
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        Append(stack);
        
        input = new TextInputField
        {
            Placeholder = Language.GetTextValue("Mods.QuestBooks.UI.Common.Searches.Placeholder"),
            Width = StyleDimension.FromPercent(0.9f),
            Height = StyleDimension.FromPercent(1f),
            VAlign = 0.5f
        };
        
        stack.Add(new SearchBarSearchButton(input)
        {
            VAlign = 0.5f
        });
        
        stack.Add(input);
        
        stack.Add(new SearchBarClearButton(input)
        {
            VAlign = 0.5f
        });
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        if (IsMouseHovering)
        {
            return;
        }

        input?.StopWriting();
    }
}