using System.Collections.Generic;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI;

[Autoload(Side = ModSide.Client)]
public sealed class TestingInterfaceSystem : ModSystem
{
    private const string InsertionLayerName = "Vanilla: Mouse Text";

    /// <summary>
    ///     The name of the interface layer used by the testing interface.
    /// </summary>
    /// <remarks>
    ///     Use this value when inserting interface layers relative to this layer in <see cref="ModifyInterfaceLayers"/>.
    /// </remarks>
    public const string LayerName = "QuestBooks: Testing";

    /// <summary>
    ///     Gets the user interface used to display the testing interface.
    /// </summary>
    public static UserInterface UserInterface { get; private set; } = null!;

    public override void Load()
    {
        UserInterface = new UserInterface();
        UserInterface.SetState(new TestingState());
    }

    public override void Unload()
    {
        UserInterface.SetState(null);
        UserInterface = null;
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        static bool Draw()
        {
            UserInterface.Draw(Main.spriteBatch, new GameTime());
            
            return true;
        }
        
        var index = layers.FindIndex(static layer => layer.Name.Equals(InsertionLayerName));
        var layer = new LegacyGameInterfaceLayer(LayerName, Draw, InterfaceScaleType.UI);
        
        if (index == -1)
        {
            layers.Add(layer);
        }
        else
        {
            layers.Insert(index, layer);
        }
    }

    public override void UpdateUI(GameTime gameTime) => UserInterface.Update(gameTime);

    /// <summary>
    ///     Opens the quest testing interface.
    /// </summary>
    public static void Open() => UserInterface.SetState(new TestingState());

    /// <summary>
    ///     Closes the quest testing interface.
    /// </summary>
    public static void Close() => UserInterface.SetState(null);
}