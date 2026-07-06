using System.Collections.Generic;
using QuestBooks.Assets;
using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultStyles;

public partial class BasicQuestLogStyle
{
    private readonly List<(Rectangle area, Action onClick, bool selected, Type type)> typeSelections = [];

    private bool selectingBookType;
    private bool selectingLineType;

    private int bookTypeScrollOffset;
    private int lineTypeScrollOffset;

    private void HandleTypeSelection()
    {
        var questBookType = LogArea.CookieCutter(new Vector2(-1.28f, -0.82f), new Vector2(0.15f, 0.05f));
        var questLineType = questBookType.CookieCutter(new Vector2(0f, 5f), Vector2.One);
        var typeDropDown = LogArea.CookieCutter(new Vector2(-1.22f, 0.26f), new Vector2(0.21f, 0.74f));

        var bookMovement = LogArea.CookieCutter(new Vector2(-0.955f, -0.1f), new Vector2(0.02f, 0.063f));
        var bookUp = bookMovement.CookieCutter(new Vector2(0f, -0.5f), new Vector2(1f, 0.5f));
        var bookDown = bookUp.CookieCutter(new Vector2(0f, 2f), Vector2.One);

        var chapterMovement = LogArea.CookieCutter(new Vector2(-0.05f, -0.1f), new Vector2(0.02f, 0.063f));
        var chapterUp = chapterMovement.CookieCutter(new Vector2(0f, -0.5f), new Vector2(1f, 0.5f));
        var chapterDown = chapterUp.CookieCutter(new Vector2(0f, 2f), Vector2.One);

        if (SelectedBook is not null)
        {
            if (SelectedElement is null)
            {
                var bookIndex = QuestManager.QuestBooks.IndexOf(SelectedBook);
                var firstBook = bookIndex == 0;
                var lastBook = bookIndex == QuestManager.QuestBooks.Count - 1;

                var bookUpHovered = false;
                var bookDownHovered = false;

                if (bookUp.Contains(MouseCanvas))
                {
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ShiftBookUp");

                    if (LeftMouseJustReleased && !firstBook)
                    {
                        (QuestManager.QuestBooks[bookIndex], QuestManager.QuestBooks[bookIndex - 1]) = (QuestManager.QuestBooks[bookIndex - 1], QuestManager.QuestBooks[bookIndex]);

                        AddHistory
                        (
                            () => { (QuestManager.QuestBooks[bookIndex], QuestManager.QuestBooks[bookIndex - 1]) = (QuestManager.QuestBooks[bookIndex - 1], QuestManager.QuestBooks[bookIndex]); },
                            () => { (QuestManager.QuestBooks[bookIndex], QuestManager.QuestBooks[bookIndex - 1]) = (QuestManager.QuestBooks[bookIndex - 1], QuestManager.QuestBooks[bookIndex]); }
                        );
                    }
                }

                else if (bookDown.Contains(MouseCanvas))
                {
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ShiftBookDown");

                    if (LeftMouseJustReleased && !lastBook)
                    {
                        (QuestManager.QuestBooks[bookIndex], QuestManager.QuestBooks[bookIndex + 1]) = (QuestManager.QuestBooks[bookIndex + 1], QuestManager.QuestBooks[bookIndex]);

                        AddHistory
                        (
                            () => { (QuestManager.QuestBooks[bookIndex], QuestManager.QuestBooks[bookIndex + 1]) = (QuestManager.QuestBooks[bookIndex + 1], QuestManager.QuestBooks[bookIndex]); },
                            () => { (QuestManager.QuestBooks[bookIndex], QuestManager.QuestBooks[bookIndex + 1]) = (QuestManager.QuestBooks[bookIndex + 1], QuestManager.QuestBooks[bookIndex]); }
                        );
                    }
                }

                Texture2D bookUpTexture = bookUpHovered && !firstBook ? QuestAssets.ShiftBookUpHovered : QuestAssets.ShiftBookUp;
                Texture2D bookDownTexture = bookDownHovered && !lastBook ? QuestAssets.ShiftBookDownHovered : QuestAssets.ShiftBookDown;

                DrawTasks.Add
                (sb =>
                    {
                        var scale = 0.5f * LogScale;
                        sb.Draw(bookUpTexture, bookUp.Center(), null, Color.White * (firstBook ? 0.4f : 1f), 0f, bookUpTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                        sb.Draw(bookDownTexture, bookDown.Center(), null, Color.White * (lastBook ? 0.4f : 1f), 0f, bookDownTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    }
                );
            }

            AddRectangle(questBookType, Color.Gray * 0.6f, fill: true);
            AddRectangle(questBookType, selectingBookType ? Color.Yellow : questBookType.Contains(MouseCanvas) ? Color.LightGray : Color.Black, 3f);

            var font = FontAssets.DeathText.Value;
            var typeName = SelectedBook.GetType().Name;

            DrawTasks.Add
            (sb =>
                {
                    sb.DrawOutlinedStringInRectangle
                    (
                        questBookType.CookieCutter(new Vector2(0f, -1.6f), Vector2.One),
                        font,
                        Color.White,
                        Color.Black,
                        Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.QuestBookType"),
                        maxScale: 0.5f
                    );

                    questBookType = questBookType.CookieCutter(new Vector2(0f, 0.2f), Vector2.One);
                    sb.DrawOutlinedStringInRectangle(questBookType.CreateMargins(2, 2), font, Color.White, Color.Black, typeName, alignment: TextAlignment.Left);
                }
            );

            if (questBookType.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ChangeQuestBookType");

                if (LeftMouseJustReleased)
                {
                    selectingBookType = !selectingBookType;
                    selectingLineType = false;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }
        }

        else
        {
            selectingBookType = false;
        }

        if (SelectedChapter is not null && (SelectedBook?.Chapters.Contains(SelectedChapter) ?? false))
        {
            if (SelectedElement is null)
            {
                var chapterIndex = SelectedBook.Chapters.IndexOf(SelectedChapter);
                var firstChapter = chapterIndex == 0;
                var lastChapter = chapterIndex == SelectedBook.Chapters.Count - 1;

                var chapterUpHovered = false;
                var chapterDownHovered = false;

                if (chapterUp.Contains(MouseCanvas))
                {
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ShiftChapterUp");
                    chapterUpHovered = true;

                    if (LeftMouseJustReleased && !firstChapter)
                    {
                        (SelectedBook.Chapters[chapterIndex], SelectedBook.Chapters[chapterIndex - 1]) = (SelectedBook.Chapters[chapterIndex - 1], SelectedBook.Chapters[chapterIndex]);

                        AddHistory
                        (
                            () => { (SelectedBook.Chapters[chapterIndex], SelectedBook.Chapters[chapterIndex - 1]) = (SelectedBook.Chapters[chapterIndex - 1], SelectedBook.Chapters[chapterIndex]); },
                            () => { (SelectedBook.Chapters[chapterIndex], SelectedBook.Chapters[chapterIndex - 1]) = (SelectedBook.Chapters[chapterIndex - 1], SelectedBook.Chapters[chapterIndex]); }
                        );
                    }
                }

                else if (chapterDown.Contains(MouseCanvas))
                {
                    MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ShiftChapterDown");
                    chapterDownHovered = true;

                    if (LeftMouseJustReleased && !lastChapter)
                    {
                        (SelectedBook.Chapters[chapterIndex], SelectedBook.Chapters[chapterIndex + 1]) = (SelectedBook.Chapters[chapterIndex + 1], SelectedBook.Chapters[chapterIndex]);

                        AddHistory
                        (
                            () => { (SelectedBook.Chapters[chapterIndex], SelectedBook.Chapters[chapterIndex + 1]) = (SelectedBook.Chapters[chapterIndex + 1], SelectedBook.Chapters[chapterIndex]); },
                            () => { (SelectedBook.Chapters[chapterIndex], SelectedBook.Chapters[chapterIndex + 1]) = (SelectedBook.Chapters[chapterIndex + 1], SelectedBook.Chapters[chapterIndex]); }
                        );
                    }
                }

                Texture2D chapterUpTexture = chapterUpHovered && !firstChapter ? QuestAssets.ShiftBookUpHovered : QuestAssets.ShiftBookUp;
                Texture2D chapterDownTexture = chapterDownHovered && !lastChapter ? QuestAssets.ShiftBookDownHovered : QuestAssets.ShiftBookDown;

                DrawTasks.Add
                (sb =>
                    {
                        var scale = 0.5f * LogScale;
                        sb.Draw(chapterUpTexture, chapterUp.Center(), null, Color.White * (firstChapter ? 0.4f : 1f), 0f, chapterUpTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                        sb.Draw(chapterDownTexture, chapterDown.Center(), null, Color.White * (lastChapter ? 0.4f : 1f), 0f, chapterDownTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    }
                );
            }

            AddRectangle(questLineType, Color.Gray * 0.6f, fill: true);
            AddRectangle(questLineType, selectingLineType ? Color.Yellow : questLineType.Contains(MouseCanvas) ? Color.LightGray : Color.Black, 3f);

            var font = FontAssets.DeathText.Value;
            var typeName = SelectedChapter.GetType().Name;

            DrawTasks.Add
            (sb =>
                {
                    sb.DrawOutlinedStringInRectangle
                    (
                        questLineType.CookieCutter(new Vector2(0f, -1.6f), Vector2.One),
                        font,
                        Color.White,
                        Color.Black,
                        Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.QuestLineType"),
                        maxScale: 0.5f
                    );

                    questLineType = questLineType.CookieCutter(new Vector2(0f, 0.2f), Vector2.One);
                    sb.DrawOutlinedStringInRectangle(questLineType.CreateMargins(2, 2), font, Color.White, Color.Black, typeName, alignment: TextAlignment.Left);
                }
            );

            if (questLineType.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ChangeChapterType");

                if (LeftMouseJustReleased)
                {
                    selectingLineType = !selectingLineType;
                    selectingBookType = false;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }
        }

        else
        {
            selectingLineType = false;
        }

        if (selectingBookType || selectingLineType)
        {
            AddRectangle(typeDropDown, Color.Gray * 0.6f, fill: true);
            AddRectangle(typeDropDown, Color.Black, 3f);
            typeSelections.Clear();

            var typeBox = typeDropDown.CreateScaledMargin(0.025f).CookieCutter(new Vector2(0f, -0.95f), new Vector2(1f, 0.078f));

            if (selectingBookType)
            {
                foreach (var bookType in QuestManager.AvailableQuestBookTypes)
                {
                    void onClick()
                    {
                        var oldType = SelectedBook.GetType();
                        var instance = (QuestBook)Activator.CreateInstance(bookType);
                        SelectedBook.CloneTo(instance);
                        instance.CloneFrom(SelectedBook);

                        var index = QuestManager.QuestBooks.IndexOf(SelectedBook);
                        QuestManager.QuestBooks[index] = instance;
                        SelectedBook = instance;

                        AddHistory
                        (
                            () =>
                            {
                                var oldInstance = (QuestBook)Activator.CreateInstance(oldType);
                                instance.CloneTo(oldInstance);
                                oldInstance.CloneFrom(instance);
                                QuestManager.QuestBooks[index] = oldInstance;
                            },
                            () =>
                            {
                                var newInstance = (QuestBook)Activator.CreateInstance(bookType);
                                var instance = QuestManager.QuestBooks[index];
                                instance.CloneTo(newInstance);
                                newInstance.CloneFrom(instance);
                                QuestManager.QuestBooks[index] = newInstance;
                            }
                        );
                    }

                    typeSelections.Add((typeBox, onClick, bookType == SelectedBook.GetType(), bookType));
                    typeBox = typeBox.CookieCutter(new Vector2(0, 2.2f), Vector2.One);
                }
            }

            else
            {
                foreach (var lineType in QuestManager.AvailableQuestLineTypes)
                {
                    void onClick()
                    {
                        var oldType = SelectedChapter.GetType();
                        var instance = (QuestChapter)Activator.CreateInstance(lineType);
                        SelectedChapter.CloneTo(instance);
                        instance.CloneFrom(SelectedChapter);

                        var book = SelectedBook;
                        var index = book.Chapters.IndexOf(SelectedChapter);
                        book.Chapters[index] = instance;
                        SelectedChapter = instance;

                        AddHistory
                        (
                            () =>
                            {
                                var oldInstance = (QuestChapter)Activator.CreateInstance(oldType);
                                instance.CloneTo(oldInstance);
                                oldInstance.CloneFrom(instance);
                                book.Chapters[index] = oldInstance;
                            },
                            () =>
                            {
                                var newInstance = (QuestChapter)Activator.CreateInstance(lineType);
                                var instance = book.Chapters[index];
                                instance.CloneTo(newInstance);
                                newInstance.CloneFrom(instance);
                                book.Chapters[index] = newInstance;
                            }
                        );
                    }

                    typeSelections.Add((typeBox, onClick, lineType == SelectedChapter.GetType(), lineType));
                    typeBox = typeBox.CookieCutter(new Vector2(0f, 2.2f), Vector2.One);
                }
            }

            if (typeDropDown.Contains(MouseCanvas))
            {
                LockMouse();
                var data = PlayerInput.ScrollWheelDeltaForUI;

                if (data != 0)
                {
                    var scrollAmount = data / 6;

                    if (selectingBookType)
                    {
                        UpdateScroll(ref bookTypeScrollOffset);
                    }
                    else
                    {
                        UpdateScroll(ref lineTypeScrollOffset);
                    }

                    void UpdateScroll(ref int scrollOffset)
                    {
                        var initialOffset = scrollOffset;
                        scrollOffset += scrollAmount;

                        var lastBox = typeSelections[^1].area;
                        var minScrollValue = -(lastBox.Bottom - (typeDropDown.Height + typeDropDown.Y));

                        scrollOffset = minScrollValue < 0 ? int.Clamp(scrollOffset, minScrollValue, 0) : 0;
                    }
                }
            }

            DrawTasks.Add
            (sb =>
                {
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                    sb.End();

                    sb.GraphicsDevice.ScissorRectangle = typeDropDown;
                    raster.ScissorTestEnable = true;

                    sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
                }
            );

            foreach (var (box, onClick, selected, type) in typeSelections)
            {
                var offset = selectingBookType ? bookTypeScrollOffset : lineTypeScrollOffset;
                box.Offset(0, offset);
                AddRectangle(box, Color.Gray, fill: true);

                if (selected)
                {
                    AddRectangle(box, Color.Yellow);
                }

                else
                {
                    AddRectangle(box, Color.LightGray);
                }

                DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(box.CookieCutter(new Vector2(0f, 0.3f), Vector2.One), FontAssets.DeathText.Value, Color.White, Color.Black, type.Name));

                if (box.Contains(MouseCanvas) && typeDropDown.Contains(MouseCanvas))
                {
                    if (!selected)
                    {
                        AddRectangle(box, Color.White);
                    }

                    MouseTooltip = $"[c/CCC018:{type.FullName}]";

                    if (Attribute.GetCustomAttribute(type, typeof(TooltipAttribute)) is TooltipAttribute tooltip)
                    {
                        MouseTooltip += $"\n{Language.GetTextValue(tooltip.LocalizationKey)}";
                    }

                    if (LeftMouseJustReleased)
                    {
                        onClick();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
            }

            DrawTasks.Add
            (sb =>
                {
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                    sb.End();

                    sb.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
                    raster.ScissorTestEnable = false;

                    sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
                }
            );
        }
    }
}