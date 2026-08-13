using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using QuestBooks.Common.Testing.UI.Components;
using QuestBooks.Common.UI.Components;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.Layout;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI;

public static class TestingMenu
{
    public sealed class Header : UIElement
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
        
            SetPadding(8f);
        
            Append(new SettingsPanel
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f)
            });

            var stack = new HorizontalStack
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f),
                Gap = 4f,
                Padding = 8f
            };
        
            Append(stack);
        
            stack.Add(new Image(ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/Testing/HeaderIcon"))
            {
                HAlign = 0f,
                VAlign = 0.5f
            });

            stack.Add(new Text(Language.GetText("Mods.QuestBooks.UI.Testing.Header"))
            {
                HAlign = 0f,
                VAlign = 0.5f
            });
        }
    }
    
    public sealed class State : UIState
    {
        private HorizontalStack horizontalStack;

        private VerticalStack verticalStack;
        
        public override void OnInitialize()
        {
            base.OnInitialize();

            var container = new UIElement
            {
                HAlign = 0.5f,
                VAlign = 0.5f,
                Width = StyleDimension.FromPercent(0.8f),
                Height = StyleDimension.FromPercent(0.7f)
            };
        
            Append(container);
        
            container.Append(new BackgroundPanel
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f),
            });

            verticalStack = new VerticalStack
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f)
            };
        
            container.Append(verticalStack);
        
            var header = new Header
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(0.1f)
            };
        
            verticalStack.Add(header);

            horizontalStack = new HorizontalStack
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(0.9f)
            };
        
            verticalStack.Add(horizontalStack);
        
            horizontalStack.Add(new TestingMenuSidebar
            {
                Width = StyleDimension.FromPercent(0.2f),
                Height = StyleDimension.FromPixelsAndPercent(-header.Height.Pixels - verticalStack.Gap, 1f)
            });

            var list = new TestingMenuQuestList
            {
                Width = StyleDimension.FromPercent(0.5f),
                Height = StyleDimension.FromPixelsAndPercent(-header.Height.Pixels - verticalStack.Gap, 1f)
            };

            horizontalStack.Add(list);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Main.keyState.IsKeyDown(Keys.Escape))
            {
                return;
            }

            TestingMenuSystem.Close();
        }
    }
}

[Autoload(Side = ModSide.Client)]
public sealed class TestingMenuSystem : ModSystem
{
    private const string InsertionLayerName = "Vanilla: Mouse Text";
    
    /// <summary>
    ///     The name of the interface layer used by the testing interface.
    /// </summary>
    /// <remarks>
    ///     Use this value when inserting interface layers relative to this layer in <see cref="ModifyInterfaceLayers"/>.
    /// </remarks>
    public const string InterfaceLayerName = "QuestBooks: Testing Menu";

    /// <summary>
    ///     Gets the user interface used to display the testing interface.
    /// </summary>
    public static UserInterface UserInterface { get; private set; } = null!;

    /// <summary>
    ///     Opens the quest testing interface.
    /// </summary>
    public static void Open()
    {
        UserInterface = new UserInterface();
        UserInterface.SetState(new TestingMenu.State());
    }

    /// <summary>
    ///     Closes the quest testing interface.
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
