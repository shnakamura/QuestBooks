using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using QuestBooks.Common.UI;
using QuestBooks.Common.UI.States;
using QuestBooks.QuestLog;
using QuestBooks.Quests;
using QuestBooks.Systems;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader.UI;

namespace QuestBooks.Common.Testing.UI;

/// <summary>
///     
/// </summary>
/// <param name="Log">
///     The display name of the quest log containing the quest reference.
/// </param>
/// <param name="Book">
///     The display name of the book containing the quest reference.
/// </param>
/// <param name="Chapter">
///     The display name of the chapter containing the quest reference.
/// </param>
public readonly record struct TestingMenuReference(string Log, string Book, string Chapter);

public static class TestingMenuReferences
{
    /// <summary>
    ///     
    /// </summary>
    public static readonly FrozenDictionary<Quest, ReadOnlyCollection<TestingMenuReference>> References;

    static TestingMenuReferences()
    {
        var references = new Dictionary<Quest, List<TestingMenuReference>>();
        
        foreach (var (log, books) in QuestManager.QuestLogs)
        {
            foreach (var book in books)
            {
                foreach (var chapter in book.Chapters)
                {
                    foreach (var quest in chapter.QuestList)
                    {
                        if (!references.TryGetValue(quest, out var value))
                        {
                            value = [];
                            
                            references[quest] = value;
                        }

                        value.Add(new TestingMenuReference(log, book.DisplayName, chapter.DisplayName));
                    }
                }
            }
        }
        
        References = references.ToFrozenDictionary(static pair => pair.Key, static pair => pair.Value.AsReadOnly());
    }
    
    /// <summary>
    ///     Attempts to retrieve the references from the specified quest.
    /// </summary>
    /// <param name="quest">
    ///     The quest to retrieve references from.
    /// </param>
    /// <param name="references">
    ///     When this method returns <see langword="true"/>, contains the references; otherwise, contains <see langword="null"/>.
    /// </param>
    /// <typeparam name="TQuest">
    ///     The type of the quest to retrieve references from.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if references exist for the specified quest; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGet<TQuest>(TQuest quest, [MaybeNullWhen(false)] out ReadOnlyCollection<TestingMenuReference> references) where TQuest : Quest => References.TryGetValue(quest, out references);
}

public static class TestingMenuProgress
{
    public static class Quests
    {
        private static IList<Quest> All => QuestManager.ActiveQuests.Values;
        
        /// <summary>
        ///     Gets the completion progress of the quests.
        /// </summary>
        /// <value>
        ///     A value in the range of <c>[0f - 1f]</c>, where <c>0f</c> represents no progress and <c>1f</c> represents full progress.
        /// </value>
        public static float Progress => Complete / (float)Total;
        
        /// <summary>
        ///     Gets the total number of quests.
        /// </summary>
        public static int Total => All.Count;
        
        /// <summary>
        ///     Gets the number of complete quests.
        /// </summary>
        public static int Complete => All.Where(static quest => quest.Completed).Count();
    }

    public static class Books
    {
        /// <summary>
        ///     Gets the completion progress of quest books.
        /// </summary>
        /// <value>
        ///     A value in the range of <c>[0f - 1f]</c>, where <c>0f</c> represents no progress and <c>1f</c> represents full progress.
        /// </value>
        public static float Progress => Complete / (float)Total;
        
        /// <summary>
        ///     Gets the total number of quest books.
        /// </summary>
        public static int Total => QuestManager.QuestBooks.Count;

        /// <summary>
        ///     Gets the number of complete quest books.
        /// </summary>
        public static int Complete => QuestManager.QuestBooks.Where(static book => book.Complete).Count();
    }

    public static class Chapters
    {
        private static IList<QuestChapter> All => QuestManager.QuestBooks.Select(static book => book.Chapters).SelectMany(static chapters => chapters).ToList();

        /// <summary>
        ///     Gets the completion progress of quest chapters.
        /// </summary>
        /// <value>
        ///     A value in the range of <c>[0f - 1f]</c>, where <c>0f</c> represents no progress and <c>1f</c> represents full progress.
        /// </value>
        public static float Progress => Complete / (float)Total;
        
        /// <summary>
        ///     Gets the total number of quest chapters.
        /// </summary>
        public static int Total => All.Count;
        
        /// <summary>
        ///     Gets the number of complete quest chapters.
        /// </summary>
        public static int Complete => All.Where(static chapter => chapter.Complete).Count();
    }
}

public sealed class TestingMenuHeader : Element
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Append(Panel.Full);
        Append
        (
            Flex.FromHorizontal(FlexAlignment.Center)
                .WithFullDimensions()
                .WithPadding(8f)
                .WithElement
                (
                    Flex.FromHorizontal(FlexAlignment.Start)
                        .WithWidth(StyleDimension.FromPercent(0.9f))
                        .WithHeight(StyleDimension.FromPercent(1f))
                        .WithVAlign(0.5f)
                        .WithGap(8f)
                        .WithElement
                        (
                            Image.FromPath("QuestBooks/Assets/Textures/UI/Testing/Header")
                                .WithHAlign(0f)
                                .WithVAlign(0.5f)
                        )
                        .WithElement
                        (
                            Text.FromLiteral("Quest Testing")
                                .WithHAlign(0f)
                                .WithVAlign(0.5f)
                        )
                )
                .WithElement
                (
                    Panel.Full
                        .WithWidth(StyleDimension.FromPercent(0.1f))
                        .WithHeight(StyleDimension.FromPercent(1f))
                        .WithStyle<Panel, Button>()
                        .WithElement
                        (
                            Text.FromLiteral("Close")
                                .WithHAlign(0.5f)
                                .WithVAlign(0.5f)
                        )
                        .WithLeftClickCallback(TestingMenuSystem.Close)
                )
        );
    }
}

public sealed class TestingMenuChapter : Element
{
    /// <summary>
    ///     Gets the quest chapter associated with the element.
    /// </summary>
    public QuestChapter Chapter { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TestingMenuChapter"/> class with the specified quest chapter.
    /// </summary>
    /// <param name="chapter">
    ///     The quest chapter associated with the element.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="chapter"/> is <see langword="null"/>.
    /// </exception>
    public TestingMenuChapter(QuestChapter chapter)
    {
        ArgumentNullException.ThrowIfNull(chapter);

        Chapter = chapter;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Append(Panel.FromPath("QuestBooks/Assets/Textures/UI/DarkPanel").WithFullDimensions());
    
        Append
        (
            Flex.FromHorizontal(FlexAlignment.Start)
                .WithFullDimensions()
                .WithGap(2f)
                .WithElement
                (
                    Flex.FromHorizontal(FlexAlignment.Start)
                        .WithFullDimensions()
                        .WithElement
                        (
                            Image.FromPath("QuestBooks/Assets/Textures/UI/Dropdown")
                                .WithRotation(-MathHelper.PiOver2)
                                .WithHAlign(0f)
                                .WithVAlign(0.5f)
                        )
                        .WithElement
                        (
                            Text.FromLiteral(Chapter.DisplayName)
                                .WithScale(0.8f)
                                .WithHAlign(0f)
                                .WithVAlign(0.5f)
                        )
                )
        );
    }
}

public sealed class TestingMenuBook : Element
{
    private readonly List<TestingMenuChapter> items = [];
    
    /// <summary>
    ///     Gets the quest book associated with the element.
    /// </summary>
    public QuestBook Book { get; }

    /// <summary>
    ///     Gets the list of the element.
    /// </summary>
    public UIList List { get; }
    
    /// <summary>
    ///     Gets a value indicating whether the element is expanded.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the element is expanded; otherwise, <see langword="false"/>.
    /// </value>
    public bool Expanded { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TestingMenuBook"/> class with the specified quest book.
    /// </summary>
    /// <param name="book">
    ///     The quest book associated with the element.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="book"/> is <see langword="null"/>.
    /// </exception>
    public TestingMenuBook(QuestBook book)
    {
        ArgumentNullException.ThrowIfNull(book);

        Book = book;

        List = new UIList()
            .WithWidth(StyleDimension.FromPercent(1f))
            .WithHeight(StyleDimension.FromPixels(0f))
            .WithGap(4f);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Append
        (
            Flex.FromVertical(FlexAlignment.Start)
                .WithFullDimensions()
                .WithGap(8f)
                .WithElement
                (
                    Container.Empty
                        .WithWidth(StyleDimension.FromPercent(1f))
                        .WithHeight(StyleDimension.FromPixels(32f))
                        .WithElement(Panel.Full)
                        .WithElement
                        (
                            Container.Empty
                                .WithFullDimensions()
                                .WithPadding(8f)
                                .WithElement
                                (
                                    Text.FromLiteral(Book.DisplayName)
                                        .WithHAlign(0f)
                                        .WithVAlign(0.5f)
                                        .WithScale(0.9f)
                                )
                                .WithElement
                                (
                                    Image.FromPath("QuestBooks/Assets/Textures/UI/Dropdown")
                                        .WithStyle<Image, Button>()
                                        .WithHAlign(1f)
                                        .WithVAlign(0.5f)
                                        .WithLeftClickCallback(Toggle)
                                )
                        )
                )
                .WithElement(List)
        );

        Populate();
        Collapse();
    }

    /// <summary>
    ///     Toggles the expanded state of the element.
    /// </summary>
    public void Toggle()
    {
        if (Expanded)
        {
            Collapse();
        }
        else
        {
            Expand();
        }
    }
    
    /// <summary>
    ///     Expands the element.
    /// </summary>
    public void Expand()
    {
        Expanded = true;

        List.Height = StyleDimension.FromPixels(items.Count * (32f + List.ListPadding));
        Height = StyleDimension.FromPixels(32f + (items.Count * (32f + List.ListPadding) + 4f));

        Recalculate();
    }

    /// <summary>
    ///     Collapses the element.
    /// </summary>
    public void Collapse()
    {
        Expanded = false;

        List.Height = StyleDimension.FromPixels(0f);
        Height = StyleDimension.FromPixels(32f);

        Recalculate();
    }
    
    private void Populate()
    {
        foreach (var chapter in Book.Chapters)
        {
            var item = new TestingMenuChapter(chapter)
                .WithWidth(StyleDimension.FromPercent(1f))
                .WithHeight(StyleDimension.FromPixels(32f));

            List.Add(item);

            items.Add(item);
        }
    }
}

public sealed class TestingMenuFilters : Element
{
    private readonly List<TestingMenuBook> items = [];
    
    /// <summary>
    ///     Gets the scrollbar of the list.
    /// </summary>
    public UIScrollbar Scrollbar { get; }
    
    /// <summary>
    ///     Gets the list of the element.
    /// </summary>
    public UIList List { get; }
    
    /// <summary>
    ///     Gets the items in the list.
    /// </summary>
    public IReadOnlyList<TestingMenuBook> Items => items;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TestingMenuFilters"/> class.
    /// </summary>
    public TestingMenuFilters()
    {
        Scrollbar = new UIScrollbar()
            .WithWidth(StyleDimension.FromPixels(20f))
            .WithHeight(StyleDimension.FromPercent(1f))
            .WithHAlign(1f)
            .WithVAlign(0.5f);

        List = new UIList()
            .WithWidth(StyleDimension.FromPixelsAndPercent(-28f, 1f))
            .WithHeight(StyleDimension.FromPercent(0.9f))
            .WithGap(8f)
            .WithScrollbar(Scrollbar);
    }
    
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Append(Panel.Full);
        
        Append
        (
            Flex.FromVertical(FlexAlignment.Start)
                .WithFullDimensions()
                .WithElement
                (
                    Container.Empty
                        .WithWidth(StyleDimension.FromPercent(1f))
                        .WithHeight(StyleDimension.FromPercent(0.1f))
                        .WithPadding(8f)
                        .WithElement
                        (
                            SearchBar.Empty
                                .WithWidth(StyleDimension.FromPercent(1f))
                                .WithHeight(StyleDimension.FromPercent(1f))
                                .WithSearch(Search)
                        )
                )
                .WithElement
                (
                    Flex.FromHorizontal(FlexAlignment.Center)
                        .WithWidth(StyleDimension.FromPercent(1f))
                        .WithHeight(StyleDimension.FromPercent(0.9f))
                        .WithPadding(8f)
                        .WithElement(List)
                        .WithElement(Scrollbar)
                )
        );
        
        Populate();
    }

    private void Populate()
    {
        foreach (var (_, books) in QuestManager.QuestLogs)
        {
            foreach (var book in books)
            {
                var item = new TestingMenuBook(book)
                    .WithWidth(StyleDimension.FromPercent(1f))
                    .WithHeight(StyleDimension.FromPixels(32f));

                List.Add(item);
                
                items.Add(item);
            }
        }
    }
    
    private void Search(string contents)
    {
        List.Clear();

        foreach (var item in items)
        {
            if (!item.Book.DisplayName.Contains(contents, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            List.Add(item);
        }

        List.UpdateOrder();
    }
}

public sealed class TestingMenuListItem : Element
{
    private static readonly Rectangle INCOMPLETE_FRAME = new(0, 24, 24, 24);
    
    private static readonly Rectangle COMPLETE_FRAME = new(0, 0, 24, 24);

    /// <summary>
    ///     Gets the callback invoked when the element is selected.
    /// </summary>
    public TestingMenuListCallback Callback { get; }
    
    /// <summary>
    ///     Gets the quest associated with the element.
    /// </summary>
    public Quest Quest { get; }
    
    public Image Button { get; }
    
    /// <summary>
    ///     Gets a value indicating whether the element is selected.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the element is selected; otherwise, <see langword="false"/>.
    /// </value>
    public bool Selected { get; private set; }
    
    /// <summary>
    ///     Gets a value indicating whether the quest is completed.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the quest is completed; otherwise, <see langword="false"/>.
    /// </value>
    public bool Completed => Quest.Completed;
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="TestingMenuListItem"/> class with the specified quest.
    /// </summary>
    /// <param name="quest">
    ///     The quest associated with the element.
    /// </param>
    /// <param name="callback">
    ///     The callback to invoke when the element is selected.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="quest"/> is <see langword="null"/>.
    /// </exception>
    public TestingMenuListItem(Quest quest, TestingMenuListCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        ArgumentNullException.ThrowIfNull(quest);
        
        Quest = quest;
        Callback = callback;

        Button = Image.FromPath("QuestBooks/Assets/Textures/UI/Testing/Status")
            .WithStyle<Image, Button>()
            .WithHAlign(0f)
            .WithVAlign(0.5f)
            .WithLeftClickCallback(Toggle)
            .WithComponent(InterfaceTooltip.FromLiteral("Toggle"))
            .WithUpdateCallback(image => image.Frame = Completed ? COMPLETE_FRAME : INCOMPLETE_FRAME);
    }
    
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Append(Background.Full.WithUpdateCallback(surface => surface.Color = IsMouseHovering ? UICommon.DefaultUIBlueMouseOver : UICommon.DefaultUIBlue));
        Append
        (
            Flex.FromHorizontal(FlexAlignment.Center)
                .WithFullDimensions()
                .WithElement
                (
                    Text.FromLiteral(Quest.Name)
                        .WithScale(0.8f)
                        .WithHAlign(0f)
                        .WithVAlign(0.5f)
                        .WithUpdateCallback(text => text.Color = Selected ? UICommon.DefaultUIBorderMouseOver : Color.White)
                )
                .WithElement
                (
                    Text.FromLiteral(Quest.Mod.Name)
                        .WithScale(0.8f)
                        .WithHAlign(0f)
                        .WithVAlign(0.5f)
                        .WithUpdateCallback(text => text.Color = Selected ? UICommon.DefaultUIBorderMouseOver : Color.White)
                )
                .WithElement(Button)
        );
    }

    /// <summary>
    ///     Toggles the completion of the quest associated with the element.
    /// </summary>
    public void Toggle()
    {
        if (Completed)
        {
            QuestBooksMod.MarkIncomplete(Quest);
        }
        else
        {
            QuestBooksMod.MarkComplete(Quest);
        }
    }

    /// <summary>
    ///     Selects the element.
    /// </summary>
    public void Select()
    {
        if (Button.IsMouseHovering)
        {
            return;
        }

        Selected = !Selected;
        
        Callback.Invoke(this);
    }

    /// <summary>
    ///     Unselects the element.
    /// </summary>
    public void Unselect() => Selected = false;
}

/// <summary>
///     Represents a callback invoked when an item is selected.
/// </summary>
/// <param name="item">
///     The item that was selected.
/// </param>
public delegate void TestingMenuListCallback(TestingMenuListItem item);

public enum TestingMenuSorting : byte
{
    Name,
    Mod,
    Completion
}

public sealed class TestingMenuList : Element
{
    private static readonly IComparer<TestingMenuListItem> NAME_COMPARER = Comparer<TestingMenuListItem>.Create(static (left, right) => string.CompareOrdinal(left.Quest.Name, right.Quest.Name));
    
    private static readonly IComparer<TestingMenuListItem> MOD_COMPARER = Comparer<TestingMenuListItem>.Create(static (left, right) => string.CompareOrdinal(left.Quest.Mod.Name, right.Quest.Mod.Name));
    
    private static readonly IComparer<TestingMenuListItem> COMPLETION_COMPARER = Comparer<TestingMenuListItem>.Create(static (left, right) => left.Completed.CompareTo(right.Completed));
    
    private static readonly Rectangle INCOMPLETE_FRAME = new(0, 24, 24, 24);
    
    private static readonly Rectangle COMPLETE_FRAME = new(0, 0, 24, 24);

    private readonly List<TestingMenuListItem> items = [];
    
    /// <summary>
    ///     Gets the callback invoked when a quest is selected.
    /// </summary>
    public TestingMenuListCallback Callback { get; }

    /// <summary>
    ///     Gets the scrollbar of the list.
    /// </summary>
    public ElementListScrollbar ElementListScrollbar { get; }

    /// <summary>
    ///     Gets the list of the list.
    /// </summary>
    public ElementList<TestingMenuListItem> List { get; }
    
    public TestingMenuSorting Sorting { get; private set; }
    
    /// <summary>
    ///     Gets the items in the list.
    /// </summary>
    public IReadOnlyList<TestingMenuListItem> Items => items;

    public IComparer<TestingMenuListItem> Comparer => Sorting switch
    {
        TestingMenuSorting.Name => NAME_COMPARER,
        TestingMenuSorting.Mod => MOD_COMPARER,
        TestingMenuSorting.Completion => COMPLETION_COMPARER,
        _ => Comparer<TestingMenuListItem>.Default
    };

    /// <summary>
    ///     Initializes a new instance of the <see cref="TestingMenuList"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     The callback invoked when a quest is selected.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public TestingMenuList(TestingMenuListCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        Callback = callback;

        ElementListScrollbar = ElementListScrollbar.Empty
            .WithWidth(StyleDimension.FromPixels(20f))
            .WithHeight(StyleDimension.FromPercent(1f))
            .WithHAlign(1f)
            .WithVAlign(0.5f);

        List = ElementList<TestingMenuListItem>.Empty
            .WithWidth(StyleDimension.FromPixelsAndPercent(-28f, 1f))
            .WithHeight(StyleDimension.FromPercent(1f))
            .WithComparer(Comparer);
    }
    
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Append(Panel.Full);
        Append
        (
            Flex.FromVertical(FlexAlignment.Start)
                .WithFullDimensions()
                .WithElement
                (
                    Flex.FromHorizontal(FlexAlignment.Start)
                        .WithWidth(StyleDimension.FromPercent(1f))
                        .WithHeight(StyleDimension.FromPercent(0.1f))
                        .WithPadding(8f)
                        .WithGap(8f)
                        .WithElement
                        (
                            SearchBar.Empty
                                .WithWidth(StyleDimension.FromPixelsAndPercent(-(48f + 8f) * 3f, 1f))
                                .WithHeight(StyleDimension.FromPercent(1f))
                                .WithSearch(Search)
                        )
                        .WithElement
                        (
                            Panel.Full
                                .WithWidth(StyleDimension.FromPixels(48f))
                                .WithHeight(StyleDimension.FromPercent(1f))
                                .WithElement
                                (
                                    Image.FromPath("QuestBooks/Assets/Textures/UI/Testing/Status")
                                        .WithStyle<Image, Button>()
                                        .WithFrame(COMPLETE_FRAME)
                                        .WithHAlign(0.5f)
                                        .WithVAlign(0.5f)
                                        .WithLeftClickCallback(Complete)
                                        .WithComponent(InterfaceTooltip.FromLiteral("Complete All"))
                                )
                        )
                        .WithElement
                        (
                            Panel.Full
                                .WithWidth(StyleDimension.FromPixels(48f))
                                .WithHeight(StyleDimension.FromPercent(1f))
                                .WithElement
                                (
                                    Image.FromPath("QuestBooks/Assets/Textures/UI/Testing/Status")
                                        .WithStyle<Image, Button>()
                                        .WithFrame(INCOMPLETE_FRAME)
                                        .WithHAlign(0.5f)
                                        .WithVAlign(0.5f)
                                        .WithLeftClickCallback(Incomplete)
                                        .WithComponent(InterfaceTooltip.FromLiteral("Incomplete All"))
                                )
                        )
                        .WithElement
                        (
                            Panel.Full
                                .WithWidth(StyleDimension.FromPixels(48f))
                                .WithHeight(StyleDimension.FromPercent(1f))
                                .WithElement
                                (
                                    Image.FromPath("QuestBooks/Assets/Textures/UI/Sort")
                                        .WithStyle<Image, Button>()
                                        .WithHAlign(0.5f)
                                        .WithVAlign(0.5f)
                                        .WithLeftClickCallback(Cycle)
                                        .WithComponent(InterfaceTooltip.FromCallback(() => "Sorting by " + Sorting))
                                )
                        )
                )
                .WithElement
                (
                    Container.Empty
                        .WithWidth(StyleDimension.FromPercent(1f))
                        .WithHeight(StyleDimension.FromPercent(0.1f))
                        .WithPadding(8f)
                        .WithElement(Panel.Full)
                        .WithElement
                        (
                            Flex.FromHorizontal(FlexAlignment.Center)
                                .WithFullDimensions()
                                .WithPadding(8f)
                                .WithElement(Text.FromLiteral("Name").WithVAlign(0.5f))
                                .WithElement(Text.FromLiteral("Mod").WithVAlign(0.5f))
                                .WithElement(Text.FromLiteral("Completed").WithVAlign(0.5f))
                        )
                )
                .WithElement
                (
                    Flex.FromHorizontal(FlexAlignment.Center)
                        .WithWidth(StyleDimension.FromPercent(1f))
                        .WithHeight(StyleDimension.FromPercent(0.8f))
                        .WithPadding(8f)
                        .WithElement(List)
                        .WithElement(ElementListScrollbar)
                )
        );
        
        Populate();
    }

    private void Select(TestingMenuListItem item)
    {
        foreach (var element in Items)
        {
            if (element == item)
            {
                continue;
            }
            
            element.Unselect();
        }

        item.Select();
    }

    private void Search(string contents)
    {
        List.Clear();

        foreach (var item in items)
        {
            if (!item.Quest.Name.Contains(contents, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            List.Add(item);
        }

        List.Sort();
    }
    
    private void Populate()
    {
        foreach (var quest in QuestManager.ActiveQuests.Values)
        {
            var item = new TestingMenuListItem(quest, Callback)
                .WithWidth(StyleDimension.FromPercent(1f))
                .WithHeight(StyleDimension.FromPixels(32f))
                .WithLeftClickCallback(Select)
                .WithComponent(InterfaceSounds.FromSounds(in SoundID.MenuTick, in SoundID.MenuOpen));
            
            List.Add(item);
            items.Add(item);
        }
    }

    private void Cycle()
    {
        var values = Enum.GetValues<TestingMenuSorting>();
        var index = Array.IndexOf(values, Sorting);

        Sorting = values[(index + 1) % values.Length];
        
        List.Sort();
    }
    
    private static void Complete()
    {
        foreach (var quest in QuestManager.ActiveQuests.Values)
        {
            QuestBooksMod.MarkComplete(quest);
        }
    }

    private static void Incomplete()
    {
        foreach (var quest in QuestManager.ActiveQuests.Values)
        {
            QuestBooksMod.MarkIncomplete(quest);
        }
    }
}

public sealed class TestingMenuEmpty : Element
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Append(Panel.Full);
        Append
        (
            Text.FromLiteral("Select a quest to view more information.")
                .WithPadding(32f)
                .WithHAlign(0.5f)
                .WithVAlign(0.5f)
                .WithFont(FontAssets.DeathText)
        );
    }
}

public sealed class TestingMenuQuest : Element
{
    /// <summary>
    ///     Gets the quest of the element.
    /// </summary>
    public Quest Quest { get; }
    
    /// <summary>
    ///     Gets the list of the element.
    /// </summary>
    public ElementList<Element> List { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TestingMenuQuest"/> class with the specified quest.
    /// </summary>
    /// <param name="quest">
    ///     The quest of the element.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="quest"/> is <see langword="null"/>.
    /// </exception>
    public TestingMenuQuest(Quest quest)
    {
        ArgumentNullException.ThrowIfNull(quest);

        Quest = quest;

        List = ElementList<Element>.Empty
            .WithWidth(StyleDimension.FromPercent(1f))
            .WithHeight(StyleDimension.FromPercent(0.9f))
            .WithPadding(8f)
            .WithGap(4f);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        
        Append(Panel.Full);
        Append(List);
        
        Populate();
        
        Append
        (
            Container.Empty
                .WithWidth(StyleDimension.FromPercent(0.5f))
                .WithHeight(StyleDimension.FromPercent(0.1f))
                .WithPadding(8f)
                .WithHAlign(0f)
                .WithVAlign(1f)
                .WithElement
                (
                    Panel.Full
                        .WithWidth(StyleDimension.FromPercent(1f))
                        .WithHeight(StyleDimension.FromPercent(1f))
                        .WithHighlight(UICommon.DefaultUIBorderMouseOver)
                        .WithElement
                        (
                            Text.FromLiteral("Mark Complete")
                                .WithHAlign(0.5f)
                                .WithVAlign(0.5f)
                        )
                )
                .WithLeftClickCallback(() => QuestBooksMod.MarkComplete(Quest))
                .WithComponent(InterfaceSounds.FromSounds(in SoundID.MenuTick, in SoundID.MenuOpen))
        );
        
        Append
        (
            Container.Empty
                .WithWidth(StyleDimension.FromPercent(0.5f))
                .WithHeight(StyleDimension.FromPercent(0.1f))
                .WithPadding(8f)
                .WithHAlign(1f)
                .WithVAlign(1f)
                .WithElement
                (
                    Panel.Full
                        .WithWidth(StyleDimension.FromPercent(1f))
                        .WithHeight(StyleDimension.FromPercent(1f))
                        .WithHighlight(UICommon.DefaultUIBorderMouseOver)
                        .WithElement(Text.FromLiteral("Mark Incomplete").WithHAlign(0.5f).WithVAlign(0.5f))
                )
                .WithLeftClickCallback(() => QuestBooksMod.MarkIncomplete(Quest))
                .WithComponent(InterfaceSounds.FromSounds(in SoundID.MenuTick, in SoundID.MenuOpen))
        );
    }

    private void Populate()
    {
        List.Clear();
        
        List.Add(Text.FromLocalization(Quest.GetLocalization("Title")).WithFont(FontAssets.DeathText));
        List.Add(Text.FromLocalization(Quest.GetLocalization("Tooltip")));
        
        List.Add
        (
            Text.FromLiteral(Quest.Mod.DisplayName)
                .WithScale(0.8f)
                .WithColor(Color.Gray)
        );
        
        List.Add
        (
            Paragraph.FromLocalization(Quest.GetLocalization("Contents"))
                .WithWidth(StyleDimension.FromPercent(1f))
                .WithScale(0.8f)
        );
        
        if (!TestingMenuReferences.TryGet(Quest, out var references))
        {
            return;
        }

        List.Add(Text.FromLiteral("References"));
        
        foreach (var reference in references)
        {
            List.Add(Text.FromLiteral($"- {reference.Log}, {reference.Book}, {reference.Chapter}").WithScale(0.8f));
        }
    }
}

public sealed class TestingMenuFooter : Element
{
    public override void OnInitialize()
    {
        base.OnInitialize();

        Append
        (
            Flex.FromHorizontal(FlexAlignment.Center)
                .WithFullDimensions()
                .WithGap(8f)
                .WithElement
                (
                    Container.Empty
                        .WithWidth(StyleDimension.FromPercent(0.325f))
                        .WithHeight(StyleDimension.FromPercent(1f))
                        .WithElement(Panel.Full)
                        .WithElement
                        (
                            Container.Full
                                .WithPadding(8f)
                                .WithElement(Text.FromLiteral("Quests"))
                                .WithElement
                                (
                                    Text.Empty
                                        .WithHAlign(1f)
                                        .WithVAlign(0f)
                                        .WithUpdateCallback(text => text.Contents = TestingMenuProgress.Quests.Progress.ToString("P2"))
                                )
                                .WithElement
                                (
                                    ProgressBar.Empty
                                        .WithWidth(StyleDimension.FromPercent(1f))
                                        .WithHeight(StyleDimension.FromPixels(20f))
                                        .WithHAlign(0f)
                                        .WithVAlign(1f)
                                        .WithColor(Color.Goldenrod)
                                        .WithUpdateCallback(bar => bar.Progress = MathHelper.SmoothStep(bar.Progress, TestingMenuProgress.Quests.Progress, 0.33f))
                                        .WithComponent(InterfaceTooltip.FromCallback(() => $"{TestingMenuProgress.Quests.Complete}/{TestingMenuProgress.Quests.Total}"))
                                )
                        )
                )
                .WithElement
                (
                    Container.Empty
                        .WithWidth(StyleDimension.FromPercent(0.325f))
                        .WithHeight(StyleDimension.FromPercent(1f))
                        .WithElement(Panel.Full)
                        .WithElement
                        (
                            Container.Full
                                .WithPadding(8f)
                                .WithElement(Text.FromLiteral("Books"))
                                .WithElement
                                (
                                    Text.Empty
                                        .WithHAlign(1f)
                                        .WithVAlign(0f)
                                        .WithUpdateCallback(text => text.Contents = TestingMenuProgress.Books.Progress.ToString("P2"))
                                )
                                .WithElement
                                (
                                    ProgressBar.Empty
                                        .WithWidth(StyleDimension.FromPercent(1f))
                                        .WithHeight(StyleDimension.FromPixels(20f))
                                        .WithHAlign(0f)
                                        .WithVAlign(1f)
                                        .WithColor(Color.MediumPurple)
                                        .WithUpdateCallback(bar => bar.Progress = MathHelper.SmoothStep(bar.Progress, TestingMenuProgress.Books.Progress, 0.33f))
                                        .WithComponent(InterfaceTooltip.FromCallback(() => $"{TestingMenuProgress.Books.Complete}/{TestingMenuProgress.Books.Total}"))
                                )
                        )
                )
                .WithElement
                (
                    Container.Empty
                        .WithWidth(StyleDimension.FromPercent(0.325f))
                        .WithHeight(StyleDimension.FromPercent(1f))
                        .WithElement(Panel.Full)
                        .WithElement
                        (
                            Container.Full
                                .WithPadding(8f)
                                .WithElement(Text.FromLiteral("Chapters"))
                                .WithElement
                                (
                                    Text.Empty
                                        .WithHAlign(1f)
                                        .WithVAlign(0f)
                                        .WithUpdateCallback(text => text.Contents = TestingMenuProgress.Chapters.Progress.ToString("P2"))
                                )
                                .WithElement
                                (
                                    ProgressBar.Empty
                                        .WithWidth(StyleDimension.FromPercent(1f))
                                        .WithHeight(StyleDimension.FromPixels(20f))
                                        .WithHAlign(0f)
                                        .WithVAlign(1f)
                                        .WithColor(Color.DeepSkyBlue)
                                        .WithUpdateCallback(bar => bar.Progress = MathHelper.SmoothStep(bar.Progress, TestingMenuProgress.Chapters.Progress, 0.33f))
                                        .WithComponent(InterfaceTooltip.FromCallback(() => $"{TestingMenuProgress.Chapters.Complete}/{TestingMenuProgress.Chapters.Total}"))
                                )
                        )
                )
        );
    }
}

public sealed class TestingMenuDisplay : Element
{
    /// <summary>
    ///     Gets the quest associated with the element.
    /// </summary>
    public Quest Quest { get; private set; }

    public override void OnInitialize()
    {
        base.OnInitialize();

        Append(new TestingMenuEmpty().WithFullDimensions());
    }

    /// <summary>
    ///     Selects the specified quest.
    /// </summary>
    /// <param name="quest">
    ///     The quest to select.
    /// </param>
    public void Select(Quest quest)
    {
        ArgumentNullException.ThrowIfNull(quest);
        
        if (Quest == quest)
        {
            Quest = null;
            
            this.WithReplacement(new TestingMenuEmpty().WithFullDimensions());
        }
        else
        {
            Quest = quest;
            
            this.WithReplacement(new TestingMenuQuest(quest).WithFullDimensions());
        }
    }
}

public sealed class TestingMenuState : State<TestingMenuSystem>
{
    /// <summary>
    ///     Gets the filters of the testing menu.
    /// </summary>
    public TestingMenuFilters Filters { get; }
    
    /// <summary>
    ///     Gets the list of the testing menu.
    /// </summary>
    public TestingMenuList List { get; }
    
    /// <summary>
    ///     Gets the display of the testing menu.
    /// </summary>
    public TestingMenuDisplay Display { get; private set; }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="TestingMenuState"/> class.
    /// </summary>
    public TestingMenuState()
    {
        Filters = new TestingMenuFilters()
            .WithWidth(StyleDimension.FromPercent(0.2f))
            .WithHeight(StyleDimension.FromPercent(1f))
            .WithPadding(8f);
        
        List = new TestingMenuList(Select)
            .WithWidth(StyleDimension.FromPercent(0.5f))
            .WithHeight(StyleDimension.FromPercent(1f))
            .WithPadding(8f);

        Display = new TestingMenuDisplay()
            .WithWidth(StyleDimension.FromPercent(0.3f))
            .WithHeight(StyleDimension.FromPercent(1f))
            .WithPadding(8f);
    }
    
    public override void OnInitialize()
    {
        base.OnInitialize();

        Append
        (
            Container.Empty
                .WithWidth(StyleDimension.FromPercent(0.7f))
                .WithHeight(StyleDimension.FromPercent(0.7f))
                .WithHAlign(0.5f)
                .WithVAlign(0.5f)
                .WithElement(Panel.Full.WithBackgroundColor(UICommon.MainPanelBackground))
                .WithElement
                (
                    Flex.FromVertical(FlexAlignment.Start)
                        .WithWidth(StyleDimension.FromPercent(1f))
                        .WithHeight(StyleDimension.FromPercent(1f))
                        .WithElement
                        (
                            new TestingMenuHeader()
                                .WithWidth(StyleDimension.FromPercent(1f))
                                .WithHeight(StyleDimension.FromPercent(0.1f))
                                .WithPadding(8f)
                        )
                        .WithElement
                        (
                            Flex.FromHorizontal(FlexAlignment.Start)
                                .WithWidth(StyleDimension.FromPercent(1f))
                                .WithHeight(StyleDimension.FromPercent(0.8f))
                                .WithElement(Filters)
                                .WithElement(List)
                                .WithElement(Display)
                        )
                        .WithElement
                        (
                            new TestingMenuFooter()
                                .WithWidth(StyleDimension.FromPercent(1f))
                                .WithHeight(StyleDimension.FromPercent(0.1f))
                                .WithPadding(8f)
                        )
                )
        );
    }

    private void Select(TestingMenuListItem item) => Display.Select(item.Quest);
}

[Autoload(Side = ModSide.Client)]
public sealed class TestingMenuSystem : ModSystem, IStateSystem
{
    private const string INSERTION_LAYER_NAME = "Vanilla: Mouse Text";

    /// <summary>
    ///     The name of the interface layer used by the testing interface.
    /// </summary>
    /// <remarks>
    ///     Use this value when inserting interface layers relative to this layer in
    ///     <see cref="ModifyInterfaceLayers" />.
    /// </remarks>
    public const string INTERFACE_LAYER_NAME = "QuestBooks: Testing Menu";

    private static UserInterface userInterface = null!;

    /// <summary>
    ///     Opens the quest testing menu.
    /// </summary>
    public static void Open()
    {
        Main.playerInventory = false;
    
        userInterface.SetState(new TestingMenuState());
    }

    /// <summary>
    ///     Closes the quest testing menu.
    /// </summary>
    public static void Close() => userInterface.SetState(null);

    public override void Load()
    {
        base.Load();

        userInterface = new UserInterface();
    }

    public override void Unload()
    {
        base.Unload();

        userInterface.SetState(null);
        userInterface = null;
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        base.ModifyInterfaceLayers(layers);
        
        if (!TestingSystem.Enabled)
        {
            return;
        }
        
        static bool Draw()
        {
            userInterface.Draw(Main.spriteBatch, new GameTime());

            return true;
        }

        var index = layers.FindIndex(static layer => layer.Name == INSERTION_LAYER_NAME);
        var layer = new LegacyGameInterfaceLayer(INTERFACE_LAYER_NAME, Draw, InterfaceScaleType.UI);

        if (index == -1)
        {
            layers.Add(layer);
        }
        else
        {
            layers.Insert(index, layer);
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (!TestingSystem.Enabled)
        {
            return;
        }
        
        userInterface.Update(gameTime);
    }
}