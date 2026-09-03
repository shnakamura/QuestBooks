using System.Collections.Generic;
using QuestBooks.Common.Inventory;
using QuestBooks.Common.UI;
using ReLogic.Content;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI;

public sealed class TestingIconState : UIState
{
    public override void OnInitialize()
    {
        base.OnInitialize();

        Append
        (
            Image.FromPath("QuestBooks/Assets/Textures/UI/Testing/Header")
                .WithHighlight(UICommon.DefaultUIBorderMouseOver)
                .WithTop(StyleDimension.FromPixels(InventoryDimensions.Height))
                .WithLeft(StyleDimension.FromPixels(InventoryDimensions.Width))
                .WithLeftClickCallback(TestingMenuSystem.Open)
                .WithComponent(new InterfaceMouse())
                .WithComponent(InterfaceSounds.FromSounds(in SoundID.MenuTick, in SoundID.MenuOpen))
        );
    }
}

[Autoload(Side = ModSide.Client)]
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

    private static UserInterface userInterface = null!;

    /// <summary>
    ///     Opens the quest testing icon.
    /// </summary>
    public static void Open() => userInterface.SetState(new TestingIconState());

    /// <summary>
    ///     Closes the quest testing icon.
    /// </summary>
    public static void Close() => userInterface.SetState(null);
    
    public override void Load()
    {
        base.Load();

        userInterface = new UserInterface();
        
        InventorySystem.OnOpenInventory += Open;
        InventorySystem.OnCloseInventory += Close;
    }
    
    public override void Unload()
    {
        base.Unload();
        
        userInterface.SetState(null);
        userInterface = null;
        
        InventorySystem.OnOpenInventory -= Open;
        InventorySystem.OnCloseInventory -= Close;
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        base.ModifyInterfaceLayers(layers);
        
        if (!TestingSystem.Enabled)
        {
            return;
        }
        
        static bool Draw()
        {
            userInterface.Draw(Main.spriteBatch, new GameTime());
            
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

    public override void UpdateUI(GameTime gameTime)
    {
        base.UpdateUI(gameTime);
        
        if (!TestingSystem.Enabled)
        {
            return;
        }
        
        userInterface.Update(gameTime);
    }
}