using Microsoft.Xna.Framework.Input;
using QuestBooks.QuestLog.DefaultChapters;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.Utilities;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultStyles;

public partial class BasicQuestLogStyle
{
    private bool typingBookName;
    private bool typingChapterName;

    private string lastBookName;
    private string lastChapterName;

    private void HandleRenaming(Rectangle books, Rectangle chapters, Rectangle questArea)
    {
        var bookNameArea = books.CookieCutter(new Vector2(-0.15f, -1.25f), new Vector2(1.15f, 0.08f));
        var chapterNameArea = chapters.CookieCutter(new Vector2(0.15f, -1.25f), new Vector2(1.15f, 0.08f));

        var colorLerp = (float)(Main.timeForVisualEffects % 60);

        if (colorLerp > 30)
        {
            colorLerp -= colorLerp % 30 * 2;
        }

        colorLerp /= 30f;
        var saveNames = false;

        if (typingBookName || typingChapterName)
        {
            CancelChat = true;

            if (Main.keyState.IsKeyDown(Keys.Enter) || Main.keyState.IsKeyDown(Keys.Escape))
            {
                saveNames = true;
            }
        }

        if (bookNameArea.Contains(MouseCanvas) && SelectedBook is BasicQuestBook book)
        {
            LockMouse();
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ChangeQuestBookName");

            if (LeftMouseJustReleased)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                typingBookName = !typingBookName;
                saveNames |= !typingBookName;
                typingChapterName = false;

                if (typingBookName)
                {
                    lastBookName = book.NameKey;
                }
            }
        }

        else if (chapterNameArea.Contains(MouseCanvas) && SelectedChapter is BasicChapter chapter)
        {
            LockMouse();
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ChangeChapterName");

            if (LeftMouseJustReleased)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                typingChapterName = !typingChapterName;
                saveNames |= !typingChapterName;
                typingBookName = false;

                if (typingChapterName)
                {
                    lastChapterName = chapter.NameKey;
                }
            }
        }

        else if (LeftMouseJustPressed && (typingBookName || typingChapterName))
        {
            saveNames = true;
        }

        if (saveNames)
        {
            if (lastBookName is not null && SelectedBook is BasicQuestBook basicQuestBook && basicQuestBook.NameKey != lastBookName)
            {
                var lastNameKey = lastBookName;
                var newNameKey = basicQuestBook.NameKey;

                AddHistory(() => { basicQuestBook.NameKey = lastNameKey; }, () => { basicQuestBook.NameKey = newNameKey; });

                lastBookName = newNameKey;
            }

            else if (lastChapterName is not null && SelectedChapter is BasicChapter basic && basic.NameKey != lastChapterName)
            {
                var lastNameKey = lastChapterName;
                var newNameKey = basic.NameKey;

                AddHistory(() => { basic.NameKey = lastNameKey; }, () => { basic.NameKey = newNameKey; });

                lastChapterName = newNameKey;
            }

            typingBookName = false;
            typingChapterName = false;
        }

        var font = FontAssets.DeathText.Value;

        if (SelectedBook is BasicQuestBook basicBook)
        {
            AddRectangle(bookNameArea, Color.Gray * 0.6f, fill: true);

            DrawTasks.Add
            (sb => sb.DrawOutlinedStringInRectangle
                (
                    bookNameArea.CookieCutter(new Vector2(0f, -1.5f), Vector2.One),
                    font,
                    Color.White,
                    Color.Black,
                    Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.LocalizationKey"),
                    maxScale: 0.5f
                )
            );

            if (typingBookName)
            {
                AddRectangle(bookNameArea, Color.Lerp(Color.Black, Color.Yellow, colorLerp), 3f);

                DrawTasks.Add
                (_ =>
                    {
                        PlayerInput.WritingText = true;
                        Main.instance.HandleIME();
                    }
                );

                var newNameKey = Main.GetInputText(basicBook.NameKey);

                if (basicBook.NameKey != newNameKey)
                {
                    basicBook.NameKey = newNameKey;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }

            else
            {
                AddRectangle(bookNameArea, bookNameArea.Contains(MouseCanvas) ? Color.LightGray : Color.Black, 3f);
            }

            DrawTasks.Add
            (sb => sb.DrawOutlinedStringInRectangle
                (
                    bookNameArea.CookieCutter(new Vector2(0f, 0.15f), Vector2.One).CreateMargins(2, 3),
                    FontAssets.DeathText.Value,
                    Color.White,
                    Color.Black,
                    basicBook.NameKey,
                    minimumScale: 0.4f,
                    alignment: TextAlignment.Right
                )
            );
        }

        if (SelectedChapter is BasicChapter basicChapter && (SelectedBook?.Chapters.Contains(SelectedChapter) ?? false))
        {
            AddRectangle(chapterNameArea, Color.Gray * 0.6f, fill: true);

            DrawTasks.Add
            (sb => sb.DrawOutlinedStringInRectangle
                (
                    chapterNameArea.CookieCutter(new Vector2(0f, -1.5f), Vector2.One),
                    font,
                    Color.White,
                    Color.Black,
                    Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.LocalizationKey"),
                    maxScale: 0.5f
                )
            );

            if (typingChapterName)
            {
                AddRectangle(chapterNameArea, Color.Lerp(Color.Yellow, Color.Black, colorLerp), 3f);

                DrawTasks.Add
                (_ =>
                    {
                        PlayerInput.WritingText = true;
                        Main.instance.HandleIME();
                    }
                );

                var newChapterKey = Main.GetInputText(basicChapter.NameKey);

                if (basicChapter.NameKey != newChapterKey)
                {
                    basicChapter.NameKey = newChapterKey;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }

            else
            {
                AddRectangle(chapterNameArea, chapterNameArea.Contains(MouseCanvas) ? Color.LightGray : Color.Black, 3f);
            }

            DrawTasks.Add
            (sb => sb.DrawOutlinedStringInRectangle
                (
                    chapterNameArea.CookieCutter(new Vector2(0f, 0.15f), Vector2.One).CreateMargins(2, 3),
                    FontAssets.DeathText.Value,
                    Color.White,
                    Color.Black,
                    basicChapter.NameKey,
                    minimumScale: 0.4f,
                    alignment: TextAlignment.Right
                )
            );
        }
    }
}