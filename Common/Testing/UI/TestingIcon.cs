using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestBooks.Common.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI;

public static class TestingIcon
{
    [Autoload(Side = ModSide.Client)]
    public sealed class Callbacks : ModSystem
    {
        /// <summary>
        ///     Raised when the player's inventory is opened.
        /// </summary>
        public static event Action OnOpenInventory;

        /// <summary>
        ///     Raised when the player's inventory is closed.
        /// </summary>
        public static event Action OnCloseInventory;
        
        private static bool flag;
        
        public override void Unload()
        {
            base.Unload();

            OnOpenInventory = null;
            OnCloseInventory = null;
        }
        
        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
        
            if (Main.playerInventory == flag)
            {
                return;
            }

            flag = Main.playerInventory;
        
            (Main.playerInventory ? OnOpenInventory : OnCloseInventory)?.Invoke();
        }
    }
    
    public sealed class State : UIState
    {
        public override void OnInitialize()
        {
            base.OnInitialize();

            var icon = new Image(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/Testing/HeaderIcon"))
            {
                Left = StyleDimension.FromPixels(574f),
                Top = StyleDimension.FromPixels(100f),
                Sounds = new ImageSoundSettings(),
                Highlight = new ImageHighlightSettings(),
                Tooltip = new ImageTooltipSettings(Language.GetText("Mods.QuestBooks.UI.Testing.Buttons.Open"))
            };

            icon.OnLeftClick += static (_, _) => TestingMenu.Open();
            
            Append(icon);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Main.playerInventory)
            {
                return;
            }
        
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.playerInventory)
            {
                return;
            }

            base.Draw(spriteBatch);
        }
    }

    /// <summary>
    ///     Opens the quest testing icon.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Open()  => TestingIconSystem.Open();
    
    /// <summary>
    ///     Closes the quest testing icon.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Close() => TestingIconSystem.Close();
}

public sealed class TestingIconSystem : ModSystem
{
    private const string INSERTION_LAYER_NAME = "Vanilla: Inventory";
    
    /// <summary>
    ///     The name of the interface layer used by the testing interface.
    /// </summary>
    /// <remarks>
    ///     Use this value when inserting interface layers relative to this layer in <see cref="ModifyInterfaceLayers"/>.
    /// </remarks>
    public const string INTERFACE_LAYER_NAME = "QuestBooks: Testing Menu Icon";

    /// <summary>
    ///     Gets the user interface used to display the testing interface.
    /// </summary>
    public static UserInterface UserInterface { get; private set; } = null!;

    public override void Load()
    {
        base.Load();

        TestingIcon.Callbacks.OnOpenInventory += Open;
        TestingIcon.Callbacks.OnCloseInventory += Close;
    }
    
    public override void Unload()
    {
        base.Unload();
        
        Close();
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

    /// <summary>
    ///     Opens the quest testing icon.
    /// </summary>
    public static void Open()
    {
        UserInterface = new UserInterface();
        UserInterface.SetState(new TestingIcon.State());
    }

    /// <summary>
    ///     Closes the quest testing icon.
    /// </summary>
    public static void Close()
    {
        UserInterface?.SetState(null);
        UserInterface = null;
    }
}