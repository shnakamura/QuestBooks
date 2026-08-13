using System.Linq;
using QuestBooks.Common.UI;
using QuestBooks.Common.UI.Components;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.Layout;
using QuestBooks.Quests;
using QuestBooks.Systems;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Components;

public sealed class TestingMenuQuestDisplay : UIElement
{
    private sealed class MarkCompleteButton : PanelButton
    {
        /// <summary>
        ///     The quest associated with the button.
        /// </summary>
        private readonly Quest quest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MarkCompleteButton"/> <see langword="class"/>.
        /// </summary>
        /// <param name="quest">
        ///     The quest associated with the button.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="quest"/> is <see langword="null"/>.
        /// </exception>
        public MarkCompleteButton(Quest quest) : base(Language.GetText("Mods.QuestBooks.UI.Testing.Buttons.MarkComplete"))
        {
            ArgumentNullException.ThrowIfNull(quest);
            
            this.quest = quest;
        }
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            Scale = 0.9f;
            
            SetPadding(8f);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            
            QuestManager.MarkComplete(quest);
        }
    }

    private sealed class MarkIncompleteButton : PanelButton
    {
        /// <summary>
        ///     The quest associated with the button.
        /// </summary>
        private readonly Quest quest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MarkIncompleteButton"/> <see langword="class"/>.
        /// </summary>
        /// <param name="quest">
        ///     The quest associated with the button.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="quest"/> is <see langword="null"/>.
        /// </exception>
        public MarkIncompleteButton(Quest quest) : base(Language.GetText("Mods.QuestBooks.UI.Testing.Buttons.MarkIncomplete"))
        {
            ArgumentNullException.ThrowIfNull(quest);

            this.quest = quest;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            
            Scale = 0.9f;
            
            SetPadding(8f);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            
            QuestManager.MarkIncomplete(quest);
        }
    }
    
    private Quest quest;

    private bool empty;

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (empty)
        {
            return;
        }
        
        SetPadding(8f);

        Append(new SettingsPanel
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        });
        
        var verticalStack = new VerticalStack
        {
            PaddingTop = 8f,
            PaddingLeft = 8f,
            PaddingBottom = 8f,
            PaddingRight = 8f,
            Gap = 8f,
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };

        Append(verticalStack);

        verticalStack.Add(new Text(quest.GetLocalization("Title"))
        {
            Font = FontAssets.DeathText
        });

        verticalStack.Add(new Text("From: " + quest.Mod.DisplayName)
        {
            Scale = 0.7f,
            Color = Color.DarkGray
        });
        
        verticalStack.Add(new Text(quest.GetLocalization("Tooltip"))
        {
            Scale = 0.8f
        });

        verticalStack.Add(new Text("Contents")
        {
            Scale = 1.2f
        });

        verticalStack.Add(new TextBox(quest.GetLocalization("Contents"))
        {
            Scale = 0.8f,
            Width = StyleDimension.FromPercent(1f)
        });

        verticalStack.Add(new Text("Appears In")
        {
            Scale = 1.2f
        });

        foreach (var (key, books) in QuestManager.QuestLogs)
        {
            foreach (var book in books)
            {
                foreach (var chapter in book.Chapters)
                {
                    // TODO: For some reason, books and chapters use different quest instances and equality isn't explicitly supported.
                    if (!chapter.QuestList.Any(other => other.FullName == quest.FullName))
                    {
                        continue;
                    }

                    verticalStack.Add(new Text($"- {key}, {book.DisplayName}, {chapter.DisplayName}")
                    {
                        Scale = 0.7f
                    });
                }
            }
        }

        Append(new MarkCompleteButton(quest)
        {
            PaddingTop = 8f,
            PaddingLeft = 8f,
            PaddingBottom = 8f,
            PaddingRight = 8f,
            HAlign = 0f,
            VAlign = 1f,
            Width = StyleDimension.FromPixelsAndPercent(-PaddingLeft / 2f, 0.5f),
            Height = StyleDimension.FromPercent(0.1f)
        });

        Append(new MarkIncompleteButton(quest)
        {
            PaddingTop = 8f,
            PaddingLeft = 8f,
            PaddingBottom = 8f,
            PaddingRight = 8f,
            HAlign = 1f,
            VAlign = 1f,
            Width = StyleDimension.FromPixelsAndPercent(-PaddingRight / 2f, 0.5f),
            Height = StyleDimension.FromPercent(0.1f),
        });   
    }
    
    // ReSharper disable once ParameterHidesMember
    public bool TrySetQuest(Quest quest)
    {
        ArgumentNullException.ThrowIfNull(quest);

        if (this.quest == quest)
        {
            return false;
        }

        this.quest = quest;

        empty = false;
        
        this.TryClear();
        this.TryInitialize();

        return true;
    }

    public bool TryClearQuest()
    {
        if (quest == null)
        {
            return false;
        }

        quest = null;
        empty = true;

        this.TryClear();
        
        return true;
    }
}