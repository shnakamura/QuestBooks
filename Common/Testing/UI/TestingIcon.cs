using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestBooks.Common.Inventory;
using QuestBooks.Common.UI;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.States;
using ReLogic.Content;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI;

public static class TestingIcon
{
    public sealed class State : Root<TestingIconSystem>
    {
        public static readonly Asset<Texture2D> ICON_TEXTURE = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/Testing/HeaderIcon", AssetRequestMode.ImmediateLoad);
        
        public override void OnInitialize()
        {
            base.OnInitialize();

            var icon = Image.FromAsset(ICON_TEXTURE).WithLeftClickEvent(TestingMenu.Open);

            icon.Left.Pixels = 574f;
            icon.Top.Pixels = 100f;

            Append(icon);
        }
    }

    /// <summary>
    ///     Opens the quest testing icon.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Open() => TestingIconSystem.Open();
    
    /// <summary>
    ///     Closes the quest testing icon.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Close() => TestingIconSystem.Close();
}

public sealed class TestingIconSystem : ModSystem, IRootSystem
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

        InventoryCallbacks.OnOpenInventory += Open;
        InventoryCallbacks.OnCloseInventory += Close;
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