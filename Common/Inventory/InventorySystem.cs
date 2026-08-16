namespace QuestBooks.Common.Inventory;

[Autoload(Side = ModSide.Client)]
public sealed class InventoryCallbacks : ModSystem
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