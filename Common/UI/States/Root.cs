using Microsoft.Xna.Framework.Input;
using Terraria.UI;

namespace QuestBooks.Common.UI.States;

public abstract class Root<TSystem> : UIState where TSystem : IRootSystem
{
    /// <summary>
    ///     Gets a value indicating whether the state should close when the Escape key is pressed.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the state should close when the Escape key is pressed; otherwise, <see langword="false"/>.
    /// </value>
    public virtual bool Escape { get; } = false;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        var escape = Escape && Main.keyState.IsKeyDown(Keys.Escape);

        if (!escape)
        {
            return;
        }
        
        TSystem.Close();
    }
}