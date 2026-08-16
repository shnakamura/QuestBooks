using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestBooks.Common.UI;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.States;
using QuestBooks.Quests;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI;

public static class TestingMenu
{
    public sealed class Header : Element
    {
        /// <summary>
        ///     The texture asset of the testing menu header.
        /// </summary>
        public static readonly Asset<Texture2D> ICON_TEXTURE = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/Testing/HeaderIcon", AssetRequestMode.ImmediateLoad);

        /// <summary>
        ///     The localized text of the testing menu header.
        /// </summary>
        public static readonly LocalizedText ICON_LABEL = Language.GetText("Mods.QuestBooks.UI.Testing.Header");
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            Padding = 8f;
            
            Append(new SettingsPanel().WithFill(1f));

            var horizontal = Flex.Horizontal(FlexAlignment.Evenly).WithFill(1f).WithPadding(8f);
            
            Append(horizontal);

            var container = new Element().WithFill(1f);
            var display = new Flex(FlexDirection.Horizontal, FlexAlignment.Start).WithFill(0.9f, 1f).WithVerticalAlignment(0.5f).Gap(8f);

            display.WithElement(new Image(ICON_TEXTURE).WithVerticalAlignment(0.5f));
            display.WithElement(new Text(ICON_LABEL).WithVerticalAlignment(0.5f));

            container.WithElement(display);
            
            horizontal.WithElement(container);
            horizontal.WithElement
            (
                new SettingsPanel()
                    .WithFill(0.1f, 1f)
                    .Highlight()
                    .WithLeftClickEvent(Close)
                    .WithElement(Text.FromKey("Mods.QuestBooks.UI.Common.Buttons.Close").WithAlignment(0.5f))
            );
        }
    }
    
    public sealed class Sidebar : Element
    {
        public override void OnInitialize()
        {
            base.OnInitialize();

            Padding = 8f;
            
            Append(new SettingsPanel().WithFill(1f));
        }
    }

    public sealed class ListItem : Element
    {
        private readonly Quest quest;
        
        public ListItem(Quest quest)
        {
            ArgumentNullException.ThrowIfNull(quest);

            this.quest = quest;
        }
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            Append
            (
                Flex.Horizontal(FlexAlignment.Evenly)
                    .WithFill(1f)
                    .WithElement(Text.FromLiteral(quest.Name).WithScale(0.8f).WithVerticalAlignment(0.5f))
                    .WithElement(Text.FromLiteral(quest.Mod.Name).WithScale(0.8f).WithVerticalAlignment(0.5f))
            );
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            
            var dimensions = GetDimensions();
            var position = dimensions.Position();

            Utils.DrawSettingsPanel(spriteBatch, position, dimensions.Width, Color.White);
        }
    }
    
    public sealed class List : Element
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            Padding = 8f;
            
            Append(new SettingsPanel().WithFill(1f));

            var vertical = Flex.Vertical(FlexAlignment.Start).WithFill(1f);
            
            Append(vertical);

            var header = new Element().WithFill(1f, 0.1f).WithPadding(8f);
            
            header.WithElement(new SettingsPanel().WithFill(1f));
            header.WithElement
            (
                Flex.Horizontal(FlexAlignment.Evenly)
                    .WithFill(1f, 1f)
                    .WithPadding(8f)
                    .WithElement(Text.FromKey("Mods.QuestBooks.UI.Testing.Labels.Quest").WithVerticalAlignment(0.5f))
                    .WithElement(Text.FromKey("Mods.QuestBooks.UI.Testing.Labels.Mod").WithVerticalAlignment(0.5f))
                    .WithElement(Text.FromKey("Mods.QuestBooks.UI.Testing.Labels.Status").WithVerticalAlignment(0.5f))
            );

            vertical.WithElement(header);
            
            var search = new SearchBar().WithFill(1f, 0.1f).WithPadding(8f);

            vertical.WithElement(search);
            
            var horizontal = Flex.Horizontal(FlexAlignment.Evenly).WithFill(1f, 0.8f).WithPadding(8f);

            var scrollbar = new UIScrollbar().WithAlignment(1f, 0.5f).WithVerticalFill(1f);
            var list = new UIList().WithFill(1f).WithWidth(-20f);

            foreach (var quest in ModContent.GetContent<Quest>())
            {
                list.Add(new ListItem(quest).WithHorizontalFill(1f).WithHeight(32f));
            }
            
            list.SetScrollbar(scrollbar);

            horizontal.WithElement(list);
            horizontal.WithElement(scrollbar);

            vertical.WithElement(horizontal);
        }
    }
    
    public sealed class State : Root<TestingMenuSystem>
    {
        public override bool Escape => true;

        public override void OnInitialize()
        {
            base.OnInitialize();

            var container = new Element().WithAlignment(0.5f).WithFill(0.8f, 0.7f).WithFocus();

            Append(container);

            container.Append(new BackgroundPanel().WithFill(1f));

            var vertical = Flex.Vertical(FlexAlignment.Start).WithFill(1f).WithAlignment(0.5f);
            
            container.Append(vertical);
            
            vertical.WithElement(new Header().WithFill(1f, 0.1f));

            var horizontal = Flex.Horizontal(FlexAlignment.Start).WithFill(1f, 0.9f);
            
            vertical.WithElement(horizontal);
            
            horizontal.WithElement(new Sidebar().WithFill(0.2f, 1f));
            horizontal.WithElement(new List().WithFill(0.5f, 1f));
        }
    }
    
    /// <summary>
    ///     Opens the quest testing menu.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Open() => TestingMenuSystem.Open();

    /// <summary>
    ///     Closes the quest testing menu.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Close() => TestingMenuSystem.Close();
}

// ReSharper disable MemberHidesStaticFromOuterClass
[Autoload(Side = ModSide.Client)]
public sealed class TestingMenuSystem : ModSystem, IRootSystem
{
    private const string INSERTION_LAYER_NAME = "Vanilla: Mouse Text";
    
    /// <summary>
    ///     The name of the interface layer used by the testing interface.
    /// </summary>
    /// <remarks>
    ///     Use this value when inserting interface layers relative to this layer in
    ///     <see cref="ModifyInterfaceLayers" />.
    /// </remarks>
    public const string INTERFACE_LAYER_NAME = "QuestBooks: Testing Menu";

    /// <summary>
    ///     Gets the user interface used to display the testing interface.
    /// </summary>
    public static UserInterface UserInterface { get; private set; } = null!;

    public override void Unload()
    {
        base.Unload();

        Close();
    }

    public static void Open()
    {
        Main.playerInventory = false;
        
        UserInterface = new UserInterface();
        UserInterface.SetState(new TestingMenu.State());
    }

    public static void Close()
    {
        UserInterface?.SetState(null);
        UserInterface = null;
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        static bool Draw()
        {
            UserInterface?.Draw(Main.spriteBatch, new GameTime());

            return true;
        }

        var index = layers.FindIndex(static layer => layer.Name == INSERTION_LAYER_NAME);
        var layer = new LegacyGameInterfaceLayer(INTERFACE_LAYER_NAME, Draw, InterfaceScaleType.UI);

        if (index == -1)
        {
            layers.Add(layer);
        }
        else
        {
            layers.Insert(index, layer);
        }
    }

    public override void UpdateUI(GameTime gameTime) => UserInterface?.Update(gameTime);
}
// ReSharper restore MemberHidesStaticFromOuterClass