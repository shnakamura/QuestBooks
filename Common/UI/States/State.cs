using Microsoft.Xna.Framework.Input;
using Terraria.UI;

namespace QuestBooks.Common.UI.States;

public abstract class State<TSystem> : UIState where TSystem : IStateSystem
{
    /// <inheritdoc/>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!Main.keyState.IsKeyDown(Keys.Escape))
        {
            return;
        }

        TSystem.Close();
    }
}