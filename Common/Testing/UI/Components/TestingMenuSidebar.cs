using System.Linq;
using QuestBooks.Common.UI.Components;
using QuestBooks.Common.UI.Elements;
using QuestBooks.Common.UI.Layout;
using QuestBooks.Quests;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Components;

public sealed class TestingMenuSidebar : UIElement
{
    private sealed class ModProgressList : UIElement
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

            var stack = new VerticalStack
            {
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPercent(1f),
                Gap = 4f
            };
            
            stack.SetPadding(8f);
            
            Append(stack);
        
            var label = new Text(Language.GetText("Mods.QuestBooks.UI.Testing.Labels.Mods"))
            {
                Scale = 0.9f
            };
        
            stack.Add(label);

            var list = new UIList
            {
                OverflowHidden = true,
                ListPadding = 0f,
                Width = StyleDimension.FromPercent(1f),
                Height = StyleDimension.FromPixelsAndPercent(-label.Height.Pixels - stack.Gap, 1f)
            };
            
            list.SetScrollbar(new UIScrollbar());

            foreach (var mod in TestingCache.Mods.All)
            {
                list.Add(new ModProgressCard(mod)
                {
                    Width = StyleDimension.FromPercent(1f),
                    Height = StyleDimension.FromPixels(64f)
                });
            }
        
            stack.Add(list);
        }
    }

    private sealed class ModProgressCard : ProgressCard
    {
        private readonly Mod mod;

        public override string Tooltip
        {
            get
            {
                var complete = ModContent.GetContent<Quest>().Count(quest => quest.Mod == mod && quest.Completed);
                var total = ModContent.GetContent<Quest>().Count(quest => quest.Mod == mod);

                return $"{complete} / {total}";
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModProgressCard"/> <see langword="class"/>.
        /// </summary>
        /// <param name="mod">
        ///     The mod associated with the progress card.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="mod"/> is <see langword="null"/>.
        /// </exception>
        public ModProgressCard(Mod mod) : base(mod.DisplayNameClean)
        {
            ArgumentNullException.ThrowIfNull(mod);
            
            this.mod = mod;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            
            Color = new Color(67, 191, 77);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            var complete = ModContent.GetContent<Quest>().Count(quest => quest.Mod == mod && quest.Completed);
            var total = ModContent.GetContent<Quest>().Count(quest => quest.Mod == mod);

            Progress =  complete / (float)total;
        }
    }
    
    private sealed class QuestsProgressCard() : ProgressCard(Language.GetText("Mods.QuestBooks.UI.Testing.Progress.Quests"))
    {
        public override string Tooltip => $"{TestingCache.Quests.Complete.Count} / {TestingCache.Quests.Count}";

        public override void OnInitialize()
        {
            base.OnInitialize();
            
            SetPadding(8f);
            
            Color = new Color(67, 191, 77);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            Progress = TestingCache.Quests.Progress;
        }
    }
    
    private sealed class ChaptersProgressCard() : ProgressCard(Language.GetText("Mods.QuestBooks.UI.Testing.Progress.Chapters"))
    {
        public override string Tooltip => $"{TestingCache.Chapters.Complete.Count} / {TestingCache.Chapters.Count}";

        public override void OnInitialize()
        {
            base.OnInitialize();
            
            SetPadding(8f);
            
            Color = new Color(209, 161, 65);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            Progress = TestingCache.Chapters.Progress;
        }
    }
    
    private sealed class BooksProgressCard() : ProgressCard(Language.GetText("Mods.QuestBooks.UI.Testing.Progress.Books"))
    {
        public override string Tooltip  => $"{TestingCache.Books.Complete.Count} / {TestingCache.Books.Count}";

        public override void OnInitialize()
        {
            base.OnInitialize();
            
            SetPadding(8f);
            
            Color = new Color(154, 95, 199);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            Progress = TestingCache.Books.Progress;
        }
    }

    private sealed class CloseButton() : PanelButton(Language.GetText("Mods.QuestBooks.UI.Common.Buttons.Close"))
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            SetPadding(8f);
        }
        
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
            
            TestingMenuSystem.Close();
        }
    }
    
    public override void OnInitialize()
    {
        base.OnInitialize();
        
        var stack = new VerticalStack
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        Append(stack);
        
        stack.Add(new ModProgressList
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(0.5f)
        });

        stack.Add(new QuestsProgressCard
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(0.125f)
        });

        stack.Add(new ChaptersProgressCard
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(0.125f)
        });

        stack.Add(new BooksProgressCard
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(0.125f)
        });

        stack.Add(new CloseButton
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(0.125f)
        });
    }
}