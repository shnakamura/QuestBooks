using System.Collections.Generic;
using QuestBooks.Common.UI.Components;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.Layout;
using QuestBooks.Quests;
using QuestBooks.Systems;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Components;

public sealed class TestingMenuQuestList : UIElement
{
    private sealed class QuestFilterButton : ImageButton
    {
        private static readonly Asset<Texture2D> FilterTexture = ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/UI/Filter");

        public override string Tooltip => Language.GetTextValue("Mods.QuestBooks.UI.Common.Buttons.Filter");

        public QuestFilterButton() : base(FilterTexture)
        {
            
        }
    }
    
    private sealed class QuestListFilters : UIElement
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            SetPadding(8f);
            
            Append(new SettingsPanel
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f)
            });

            Append(new QuestFilterButton()
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            });
        }
    }
    
    private sealed class QuestListHeader : UIElement
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            SetPadding(8f);
            
            Append(new SettingsPanel
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f)
            });

            var stack = new JustifiedHorizontalStack
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f)
            };
            
            stack.SetPadding(8f);
            
            Append(stack);
            
            stack.Add(new Text(Language.GetText("Mods.QuestBooks.UI.Testing.Labels.Quest"))
            {
                VAlign = 0.5f
            });
            
            stack.Add(new Text(Language.GetText("Mods.QuestBooks.UI.Testing.Labels.Mod"))
            {
                VAlign = 0.5f
            });
            
            stack.Add(new Text(Language.GetText("Mods.QuestBooks.UI.Testing.Labels.Status"))
            {
                VAlign = 0.5f
            });
        }
    }
    
    private sealed class QuestListItem : UIElement
    {
        public readonly Quest Quest;

        private Text status;
        
        public QuestListItem(Quest quest)
        {
            ArgumentNullException.ThrowIfNull(quest);
            
            Quest = quest;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            
            SetPadding(8f);

            var stack = new JustifiedHorizontalStack
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f)
            };
            
            Append(stack);
            
            stack.Add(new Text(Quest.Name)
            {
                Scale = 0.7f
            });
            
            stack.Add(new Text(Quest.Mod.DisplayNameClean)
            {
                Scale = 0.7f
            });

            status = new Text(Language.GetText(Quest.Completed ? "Mods.QuestBooks.UI.Testing.Labels.Complete" : "Mods.QuestBooks.UI.Testing.Labels.Incomplete"))
            {
                Scale = 0.7f
            };
            
            stack.Add(status);
        }

        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);
            
            if (Quest.Completed)
            {
                QuestManager.MarkIncomplete(Quest);
            }
            else
            {
                QuestManager.MarkComplete(Quest);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            status.Contents = Language.GetTextValue(Quest.Completed ? "Mods.QuestBooks.UI.Testing.Labels.Complete" : "Mods.QuestBooks.UI.Testing.Labels.Incomplete");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var dimensions = GetDimensions();
            var position = dimensions.Position();

            var color = IsMouseHovering ? UICommon.DefaultUIBlue : UICommon.DefaultUIBlueMouseOver;

            if (Quest.Completed)
            {
                color = IsMouseHovering ? new Color(67, 191, 77) : new Color(27, 151, 37);
            }

            Utils.DrawSettingsPanel(spriteBatch, position, dimensions.Width, color);
        }
    }

    private readonly List<QuestListItem> items = new();

    private UIList list;
    
    public delegate void QuestSelectedCallback(Quest quest);
    
    public event QuestSelectedCallback OnQuestSelected;

    public override void OnInitialize()
    {
        base.OnInitialize();
        
        SetPadding(8f);

        Append(new SettingsPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        });

        var verticalStack = new VerticalStack
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        Append(verticalStack);

        var horizontalStack = new JustifiedHorizontalStack
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(0.1f)
        };
        
        verticalStack.Add(horizontalStack);
        
        var search = new SearchBar
        {
            PaddingTop = 8f,
            PaddingLeft = 8f,
            PaddingBottom = 8f,
            PaddingRight = 8f,
            Capacity = 50,
            Width = StyleDimension.FromPercent(0.9f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        search.OnChangeContents += Refresh;

        list = new UIList
        {
            ManualSortMethod = Sort,
            PaddingTop = 8f,
            PaddingLeft = 8f,
            PaddingBottom = 8f,
            PaddingRight = 8f,
            ListPadding = 0f,
            Width = StyleDimension.FromPixelsAndPercent(-20f, 1f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        horizontalStack.Add(search);
        
        horizontalStack.Add(new QuestListFilters
        {
            Width = StyleDimension.FromPercent(0.1f),
            Height = StyleDimension.FromPercent(1f)
        });
        
        verticalStack.Add(new QuestListHeader
        {
            Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
            Height = StyleDimension.FromPercent(0.1f)
        });

        var container = new UIElement
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(0.8f)
        };
        
        verticalStack.Add(container);
        
        var scrollbar = new UIScrollbar
        {
            Width = StyleDimension.FromPixels(20f),
            Height = StyleDimension.FromPixelsAndPercent(-16f, 1f),
            HAlign = 1f,
            VAlign = 0.5f
        };
        
        container.Append(scrollbar);
        
        list.SetScrollbar(scrollbar);
        
        container.Append(list);

        foreach (var quest in ModContent.GetContent<Quest>())
        {
            var item = new QuestListItem(quest)
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPixels(32f),
            };
            
            item.OnLeftClick += (_, _) => OnQuestSelected?.Invoke(quest); 
            
            list.Add(item);
            items.Add(item);
        }
    }

    private void Refresh(string contents)
    {
        list.Clear();

        foreach (var item in items)
        {
            if (!item.Quest.Name.Contains(contents, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            list.Add(item);
        }

        list.UpdateOrder();
    }
    
    private static void Sort(List<UIElement> elements) => elements.Sort((a, b) =>
    {
        if (a is not QuestListItem left || b is not QuestListItem right)
        {
            return 0;
        }

        return string.Compare(left.Quest.Name, right.Quest.Name, StringComparison.OrdinalIgnoreCase);
    });
}