namespace QuestBooks.Common.UI.States;

public interface IStateSystem
{
    /// <summary>
    ///     Opens the user interface state associated with the system.
    /// </summary>
    static abstract void Open();

    /// <summary>
    ///     Closes the user interface state associated with the system.
    /// </summary>
    static abstract void Close();
}