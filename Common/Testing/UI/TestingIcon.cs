using System.Collections.Generic;
using QuestBooks.Common.UI.Elements;
using ReLogic.Content;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI;

public static class TestingIcon
{
    public sealed class Button() : ImageButton(ButtonTexture)
    {
        private static readonly Asset<Texture2D> ButtonTexture = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/Testing/HeaderIcon");
        
        public override string Tooltip => Language.GetTextValue("Mods.QuestBooks.UI.Testing.Buttons.Open");

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            
            TestingMenuSystem.Open();

            Main.playerInventory = false;
        }
    }
    
    public sealed class State : UIState
    {
        public override void OnInitialize()
        {
            base.OnInitialize();

            Append(new Button
            {
                Left = StyleDimension.FromPixels(574f),
                Top = StyleDimension.FromPixels(100f)
            });
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
}

public sealed class TestingMenuIconSystem : ModSystem
{
    private const string InsertionLayerName = "Vanilla: Inventory";
    
    /// <summary>
    ///     The name of the interface layer used by the testing interface.
    /// </summary>
    /// <remarks>
    ///     Use this value when inserting interface layers relative to this layer in <see cref="ModifyInterfaceLayers"/>.
    /// </remarks>
    public const string InterfaceLayerName = "QuestBooks: Testing Menu Icon";

    /// <summary>
    ///     Gets the user interface used to display the testing interface.
    /// </summary>
    public static UserInterface UserInterface { get; private set; } = null!;

    public override void Load()
    {
        base.Load();
        
        UserInterface = new UserInterface();
        UserInterface.SetState(new TestingIcon.State());
    }
    
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        static bool Draw()
        {
            UserInterface?.Draw(Main.spriteBatch, new GameTime());
            
            return true;
        }
        
        var index = layers.FindIndex(static layer => layer.Name.Equals(InsertionLayerName));
        var layer = new LegacyGameInterfaceLayer(InterfaceLayerName, Draw, InterfaceScaleType.UI);
        
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