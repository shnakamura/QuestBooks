using QuestBooks.Quests;
using QuestBooks.Systems;

namespace QuestBooks;

public sealed partial class QuestBooksMod
{
    /// <summary>
    ///     Retrieves a quest from the specified name.
    /// </summary>
    /// <param name="name">
    ///     The name of the quest to retrieve.
    /// </param>
    /// <returns>
    ///     The retrieved quest.
    /// </returns>
    public static Quest GetQuest(string name) => QuestManager.GetQuest(name);
    
    /// <summary>
    ///     Retrieves a quest from the specified type.
    /// </summary>
    /// <typeparam name="TQuest">
    ///     The type of the quest to retrieve.
    /// </typeparam>
    /// <returns>
    ///     The retrieved quest.
    /// </returns>
    public static TQuest GetQuest<TQuest>() where TQuest : Quest => QuestManager.GetQuest<TQuest>();

    /// <summary>
    ///     Attempts to retrieve a quest from the specified name.
    /// </summary>
    /// <param name="name">
    ///     The name of the quest to retrieve.
    /// </param>
    /// <param name="result">
    ///     When this method returns, contains the retrieved quest if successful; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the quest was retrieved successfully; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetQuest(string name, out Quest result) => QuestManager.TryGetQuest(name, out result);
 
    /// <summary>
    ///     Attempts to retrieve a quest from the specified type.
    /// </summary>
    /// <param name="result">
    ///     When this method returns, contains the retrieved quest if successful; otherwise, <see langword="null"/>.
    /// </param>
    /// <typeparam name="TQuest">
    ///     The type of the quest to retrieve.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if the quest was retrieved successfully; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetQuest<TQuest>(out TQuest result) where TQuest : Quest => QuestManager.TryGetQuest(out result);

    /// <summary>
    ///     Completes the specified quest.
    /// </summary>
    /// <typeparam name="TQuest">
    ///     The type of the quest to complete.
    /// </typeparam>
    public static void CompleteQuest<TQuest>() where TQuest : Quest => CompleteQuest(GetQuest<TQuest>());

    /// <summary>
    ///     Completes the specified quest.
    /// </summary>
    /// <param name="name">
    ///     The name of the quest to complete.
    /// </param>
    public static void CompleteQuest(string name) => CompleteQuest(GetQuest(name));

    /// <summary>
    ///     Completes the specified quest.
    /// </summary>
    /// <param name="quest">
    ///     The quest to complete.
    /// </param>
    public static void CompleteQuest(Quest quest) => QuestManager.CompleteQuest(quest);

    /// <summary>
    ///     Marks the specified quest as complete without invoking quest completion logic.
    /// </summary>
    /// <typeparam name="TQuest">
    ///     The type of the quest to mark as complete.
    /// </typeparam>
    public static void MarkComplete<TQuest>() where TQuest : Quest => MarkComplete(GetQuest<TQuest>());

    /// <summary>
    ///     Marks the specified quest as complete without invoking quest completion logic.
    /// </summary>
    /// <param name="name">
    ///     The name of the quest to mark as complete.
    /// </param>
    public static void MarkComplete(string name) => MarkComplete(GetQuest(name));

    /// <summary>
    ///     Marks the specified quest as complete without invoking quest completion logic.
    /// </summary>
    /// <param name="quest">
    ///     The quest to mark as complete.
    /// </param>
    public static void MarkComplete(Quest quest) => QuestManager.MarkComplete(quest);

    /// <summary>
    ///     Marks the specified quest as incomplete.
    /// </summary>
    /// <typeparam name="TQuest">
    ///     The type of the quest to mark as incomplete.
    /// </typeparam>
    public static void MarkIncomplete<TQuest>() where TQuest : Quest => MarkIncomplete(GetQuest<TQuest>());

    /// <summary>
    ///     Marks the specified quest as incomplete.
    /// </summary>
    /// <param name="name">
    ///     The name of the quest to mark as incomplete.
    /// </param>
    public static void MarkIncomplete(string name) => MarkIncomplete(GetQuest(name));

    /// <summary>
    ///     Marks the specified quest as incomplete.
    /// </summary>
    /// <param name="quest">
    ///     The quest to mark as incomplete.
    /// </param>
    public static void MarkIncomplete(Quest quest) => QuestManager.MarkIncomplete(quest);
}