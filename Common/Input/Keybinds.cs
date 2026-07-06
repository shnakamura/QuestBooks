using Microsoft.Xna.Framework.Input;

namespace QuestBooks.Common.Input;

/// <summary>
///     Handles loading and unloading all keybinds from <see cref="QuestBooksMod"/>.
/// </summary>
public sealed class KeybindSystem : ModSystem
{
    /// <summary>
    ///     Gets the keybind used to toggle the quest log.
    /// </summary>
    public static ModKeybind ToggleQuestLog { get; private set; }

    public override void Load() => ToggleQuestLog = KeybindLoader.RegisterKeybind(Mod, "ToggleQuestLog", Keys.L);

    public override void Unload() => ToggleQuestLog = null;
}