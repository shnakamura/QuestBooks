using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestBooks.Common.UI;
using QuestBooks.Common.UI.Components;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.Layout;
using ReLogic.Content;
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
        public static readonly LocalizedText ICON_LABEL = Language.GetText("Mods.QuestBooks.Testing.UI.Header");
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            Padding = 8f;
            
            Append(new SettingsPanel().Fill());

            var stack = new HorizontalStack
            {
                Gap = 4f,
                Padding = 8f,
                Fill = (1f, 1f)
            };

            Append(stack);
            
            stack.Add(new Image(ICON_TEXTURE).Allign(0f, 0.5f));
            stack.Add(new Text(ICON_LABEL).Allign(0f, 0.5f));
        }
    }
    
    public sealed class Sidebar : Element
    {
        public override void OnInitialize()
        {
            base.OnInitialize();

            Padding = 8f;
            
            Append(new SettingsPanel().Fill());
        }
    }

    public sealed class List : Element
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            Padding = 8f;
            
            Append(new SettingsPanel().Fill());
        }
    }

    public sealed class State : UIState
    {
        public override void OnInitialize()
        {
            base.OnInitialize();

            var container = new Element
            {
                Allign = (0.5f, 0.5f),
                Fill = (0.8f, 0.7f)
            };

            Append(container);

            container.Append(new BackgroundPanel().Fill(1f));

            var vertical = new VerticalStack
            {
                Allign = (0.5f, 0.5f),
                Fill = (1f, 1f)
            };
            
            container.Append(vertical);
            
            vertical.Add(new Header().Fill(1f, 0.1f));

            var horizontal = new HorizontalStack
            {
                Fill = (1f, 0.9f)
            };
            
            vertical.Add(horizontal);
            
            horizontal.Add(new Sidebar().Fill(0.2f, 1f));
            horizontal.Add(new List().Fill(0.5f, 1f));
        }
    }

    /// <summary>
    ///     Opens the quest testing menu.
    /// </summary>
    /// <param name="closeInventory">
    ///     <see langword="true" /> to close the player inventory while opening the quest testing menu;
    ///     otherwise, <see langword="false" />.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Open(bool closeInventory = true) => TestingMenuSystem.Open(closeInventory);

    /// <summary>
    ///     Closes the quest testing menu.
    /// </summary>
    /// <param name="closeInventory">
    ///     <see langword="true" /> to close the player inventory while closing the quest testing menu;
    ///     otherwise, <see langword="false" />.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Close(bool closeInventory = true) => TestingMenuSystem.Close(closeInventory);
}

// ReSharper disable MemberHidesStaticFromOuterClass
[Autoload(Side = ModSide.Client)]
public sealed class TestingMenuSystem : ModSystem
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

    /// <summary>
    ///     Opens the quest testing menu.
    /// </summary>
    /// <param name="closeInventory">
    ///     <see langword="true" /> to close the player inventory while opening the quest testing menu;
    ///     otherwise, <see langword="false" />.
    /// </param>
    public static void Open(bool closeInventory = true)
    {
        UserInterface = new UserInterface();
        UserInterface.SetState(new TestingMenu.State());

        if (!closeInventory)
        {
            return;
        }

        Main.playerInventory = false;
    }

    /// <summary>
    ///     Closes the quest testing menu.
    /// </summary>
    /// <param name="closeInventory">
    ///     <see langword="true" /> to close the player inventory while closing the quest testing menu;
    ///     otherwise, <see langword="false" />.
    /// </param>
    public static void Close(bool closeInventory = true)
    {
        UserInterface?.SetState(null);
        UserInterface = null;

        if (!closeInventory)
        {
            return;
        }

        Main.playerInventory = false;
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        static bool Draw()
        {
            UserInterface?.Draw(Main.spriteBatch, new GameTime());

            return true;
        }

        var index = layers.FindIndex(static layer => layer.Name.Equals(INSERTION_LAYER_NAME));
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