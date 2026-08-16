using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestBooks.Common.UI;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.States;
using QuestBooks.Quests;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI;

// ReSharper disable MemberHidesStaticFromOuterClass
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
        public static readonly LocalizedText ICON_TEXT = Language.GetText("Mods.QuestBooks.UI.Testing.Header");
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            SetPadding(8f);
            
            Append(new SettingsPanel().WithFill(1f));

            var horizontal = Flex.Horizontal(FlexAlignment.Evenly).WithFill(1f).WithPadding(8f);
            var container = new Element().WithFill(1f);
            
            var display = new Flex(FlexDirection.Horizontal, FlexAlignment.Start).WithFill(0.9f, 1f).WithVerticalAlignment(0.5f).Gap(8f);

            display.WithElement(Image.FromAsset(ICON_TEXTURE).WithVerticalAlignment(0.5f));
            display.WithElement(Text.FromLocalization(ICON_TEXT).WithVerticalAlignment(0.5f));

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
            
            Append(horizontal);
        }
    }
    
    public sealed class Sidebar : Element
    {
        public override void OnInitialize()
        {
            base.OnInitialize();

            SetPadding(8f);
            
            Append(new SettingsPanel().WithFill(1f));
        }
    }

    public sealed class ListItem : Element, IComparable<ListItem>
    {
        /// <summary>
        ///     The localized text of the complete status of a quest.
        /// </summary>
        public static readonly LocalizedText COMPLETE_STATUS_TEXT = Language.GetText("Mods.QuestBooks.UI.Testing.Status.Complete");
        
        /// <summary>
        ///     The localized text of the incomplete status of a quest.
        /// </summary>
        public static readonly LocalizedText INCOMPLETE_STATUS_TEXT = Language.GetText("Mods.QuestBooks.UI.Testing.Status.Incomplete");

        private readonly Text status = Text.Empty().WithScale(0.8f).WithVerticalAlignment(0.5f);
        
        private readonly Quest quest;
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="ListItem"/> class with the specified quest.
        /// </summary>
        /// <param name="quest">
        ///     
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="quest"/> is <see langword="null"/>.
        /// </exception>
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
                    .WithElement(status)
            );
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            status.Contents = quest.Completed ? COMPLETE_STATUS_TEXT.Value : INCOMPLETE_STATUS_TEXT.Value;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            
            var dimensions = GetDimensions();
            var position = dimensions.Position();
            
            var color = IsMouseHovering ? UICommon.DefaultUIBlue : UICommon.DefaultUIBlueMouseOver;
            
            if (quest.Completed)
            {
                color = IsMouseHovering ? new Color(67, 191, 77) : new Color(27, 151, 37);
            }

            Utils.DrawSettingsPanel(spriteBatch, position, dimensions.Width, color);
        }

        public int CompareTo(ListItem other) => string.CompareOrdinal(quest.Name, other.quest.Name);
    }
    
    public sealed class List : Element
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            SetPadding(8f);
            
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
            var list = new UIList().WithFill(1f).WithWidth(-20f).WithHiddenOverflow(true).WithScrollbar(scrollbar).WithSort(Sort);

            foreach (var quest in ModContent.GetContent<Quest>())
            {
                list.Add(new ListItem(quest).WithHorizontalFill(1f).WithHeight(32f));
            }

            horizontal.WithElement(list);
            horizontal.WithElement(scrollbar);

            vertical.WithElement(horizontal);
        }

        private static void Sort(List<UIElement> elements) => elements.Sort(static (left, right) => ((ListItem)left).CompareTo((ListItem)right));
    }
    
    public sealed class State : Root<System>
    {
        /// <inheritdoc/>
        public override bool Escape => true;

        public override void OnInitialize()
        {
            base.OnInitialize();

            var container = new Element().WithAlignment(0.5f).WithFill(0.8f, 0.7f).WithFocus();

            Append(container);

            container.Append(new BackgroundPanel().WithFill(1f));

            var vertical = Flex.Vertical(FlexAlignment.Start).WithFill(1f).WithAlignment(0.5f);
            
            container.Append(vertical);
            
            vertical.Append(new Header().WithFill(1f, 0.1f));

            var horizontal = Flex.Horizontal(FlexAlignment.Start).WithFill(1f, 0.9f);
            
            vertical.Append(horizontal);
            
            horizontal.Append(new Sidebar().WithFill(0.2f, 1f));
            horizontal.Append(new List().WithFill(0.5f, 1f));
        }
    }
    
    [Autoload(Side = ModSide.Client)]
    public sealed class System : ModSystem, IRootSystem
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

        /// <summary>
        ///     Opens the quest testing menu.
        /// </summary>
        public static void Open()
        {
            Main.playerInventory = false;
        
            UserInterface = new UserInterface();
            UserInterface.SetState(new State());
        }

        /// <summary>
        ///     Closes the quest testing menu.
        /// </summary>
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
    
    /// <summary>
    ///     Opens the quest testing menu.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Open() => System.Open();

    /// <summary>
    ///     Closes the quest testing menu.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Close() => System.Close();
}
// ReSharper restore MemberHidesStaticFromOuterClass