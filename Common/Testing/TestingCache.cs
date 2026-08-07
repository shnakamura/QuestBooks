using System.Collections.Generic;
using System.Linq;
using QuestBooks.QuestLog;
using QuestBooks.Quests;
using QuestBooks.Systems;

namespace QuestBooks.Common.Testing;

public static class TestingCache
{
    public static class Mods
    {
        /// <summary>
        ///     Gets a read-only list containing all mods with at least one quest.
        /// </summary>
        public static IReadOnlyList<Mod> All => ModContent.GetContent<Quest>().Select(static quest => quest.Mod).Distinct().ToList();
    }

    public static class Quests
    {
        /// <summary>
        ///     Gets a read-only list containing all quests.
        /// </summary>
        public static IReadOnlyList<Quest> All => ModContent.GetContent<Quest>().ToList();

        /// <summary>
        ///     Gets a read-only list containing all completed quests.
        /// </summary>
        public static IReadOnlyList<Quest> Complete => ModContent.GetContent<Quest>().Where(static quest => quest.Completed).ToList();

        /// <summary>
        ///     Gets a read-only list containing all incomplete quests.
        /// </summary>
        public static IReadOnlyList<Quest> Incomplete => ModContent.GetContent<Quest>().Where(static quest => !quest.Completed).ToList();

        /// <summary>
        ///     Gets the total number of quests.
        /// </summary>
        public static int Count => All.Count;

        /// <summary>
        ///     Gets the completion progress of all quests as a value between <c>0</c> and <c>1</c>.
        /// </summary>
        public static float Progress => Complete.Count / (float)Count;
    }

    public static class Chapters
    {
        /// <summary>
        ///     Gets a read-only list containing all quest chapters.
        /// </summary>
        public static IReadOnlyList<QuestChapter> All => Books.All.Select(static book => book.Chapters).SelectMany(static chapters => chapters).ToList();

        /// <summary>
        ///     Gets a read-only list containing all completed quest chapters.
        /// </summary>
        public static IReadOnlyList<QuestChapter> Complete => All.Where(static chapter => chapter.Complete).ToList();

        /// <summary>
        ///     Gets a read-only list containing all incomplete quest chapters.
        /// </summary>
        public static IReadOnlyList<QuestChapter> Incomplete => All.Where(static chapter => !chapter.Complete).ToList();

        /// <summary>
        ///     Gets the total number of quest chapters.
        /// </summary>
        public static int Count => All.Count;

        /// <summary>
        ///     Gets the completion progress of all quest chapters as a value between <c>0</c> and <c>1</c>.
        /// </summary>
        public static float Progress => Complete.Count / (float)Count;
    }

    public static class Books
    {
        /// <summary>
        ///     Gets a read-only list containing all quest books.
        /// </summary>
        public static IReadOnlyList<QuestBook> All => QuestManager.QuestBooks.ToList();

        /// <summary>
        ///     Gets a read-only list containing all completed quest books.
        /// </summary>
        public static IReadOnlyList<QuestBook> Complete => All.Where(static book => book.Complete).ToList();

        /// <summary>
        ///     Gets a read-only list containing all incomplete quest books.
        /// </summary>
        public static IReadOnlyList<QuestBook> Incomplete => All.Where(static book => !book.Complete).ToList();

        /// <summary>
        ///     Gets the total number of quest books.
        /// </summary>
        public static int Count => All.Count;

        /// <summary>
        ///     Gets the completion progress of all quest books as a value between <c>0</c> and <c>1</c>.
        /// </summary>
        public static float Progress => Complete.Count / (float)Count;
    }
}