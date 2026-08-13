using System.Collections.Generic;
using QuestBooks.Common.UI.Components;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.Layout;
using QuestBooks.QuestLog;
using QuestBooks.Systems;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Components;

public sealed class TestingMenuFilterList : UIElement
{
    private sealed class FilterListItem : UIElement
    {
        private readonly QuestBook book;

        public FilterListItem(QuestBook book)
        {
            ArgumentNullException.ThrowIfNull(book);
            
            this.book = book;
        }
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            Append(new SettingsPanel
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f)
            });

            Append(new Text(book.DisplayName));

            foreach (var chapter in book.Chapters)
            {
                Append(new Text(chapter.DisplayName)
                {
                   Scale = 0.8f
                });
            }
        }
    }
    
    private readonly record struct FilterListItemData(QuestBook Book, FilterListItem Item);

    private readonly List<FilterListItemData> data = new();

    private UIList list;

    public override void OnInitialize()
    {
        base.OnInitialize();
        
        SetPadding(8f);

        Append(new SettingsPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        });

        var stack = new VerticalStack
        {
            Gap = 4f,
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        Append(stack);

        var search = new SearchBar
        {
            PaddingTop = 8f,
            PaddingLeft = 8f,
            PaddingBottom = 8f,
            PaddingRight = 8f,
            Capacity = 50,
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(0.1f)
        };

        search.OnChangeContents += Refresh;
        
        stack.Add(search);
        
        var container = new UIElement
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(0.9f)
        };
        
        stack.Add(container);
        
        var scrollbar = new UIScrollbar
        {
            Width = StyleDimension.FromPixels(20f),
            Height = StyleDimension.FromPixelsAndPercent(-24f, 1f),
            HAlign = 1f,
            VAlign = 0.5f
        };
        
        list = new UIList
        {
            PaddingTop = 8f,
            PaddingLeft = 8f,
            PaddingBottom = 8f,
            PaddingRight = 8f,
            ListPadding = 8f,
            Width = StyleDimension.FromPixelsAndPercent(-scrollbar.Width.Pixels, 1f),
            Height = StyleDimension.FromPixelsAndPercent(-search.Height.Pixels, 1f)
        };
        
        list.SetScrollbar(scrollbar);
        
        container.Append(list);
        container.Append(scrollbar);
        
        foreach (var (_, books) in QuestManager.QuestLogs)
        {
            foreach (var book in books)
            {
                var card = new FilterListItem(book)
                {
                    Width = StyleDimension.FromPercent(1f),
                    Height = StyleDimension.FromPixels(128f)
                };
                
                list.Add(card);
                data.Add(new FilterListItemData(book, card));
            }
        }
    }
    
    private void Refresh(string contents)
    {
        list.Clear();

        foreach (var element in data)
        {
            if (!element.Book.DisplayName.Contains(contents, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            list.Add(element.Item);
        }

        list.UpdateOrder();
    }
}