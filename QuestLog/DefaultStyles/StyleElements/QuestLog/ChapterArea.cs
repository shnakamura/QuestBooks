using System.Collections.Generic;
using Terraria.GameInput;

namespace QuestBooks.QuestLog.DefaultStyles;

public partial class BasicQuestLogStyle
{
    // These keep track of the selected QuestLine, as well as
    // some parameters to handle "sliding" between lines
    private int booksScrollOffset;
    private int chaptersScrollOffset;
    private int previousChapterScrollOffset;

    private readonly List<(Rectangle area, QuestChapter questBook)> chapterLibrary = [];

    private void UpdateChapters(Rectangle chapters, Vector2 scaledMouse)
    {
        SwitchTargets(chaptersTarget, LibraryBlending, SamplerState.PointClamp);
        DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Black * 0.08f));

        // If we aren't in the middle of swiping and there are no chapters to draw,
        // return early
        var availableChapters = SelectedBook?.Chapters ?? [];

        if (previousBookSwipeOffset == 0f && availableChapters.Count == 0)
        {
            SwitchTargets(null);
            return;
        }

        // Cache mouse position within chapters and whether the chapter area contians the mouse
        var mouseChapters = scaledMouse.ToPoint();
        var hoveringChapters = false;

        // Handle the scrolling of the chapter area
        if (chapters.Contains(mouseChapters))
        {
            hoveringChapters = true;
            var data = PlayerInput.ScrollWheelDeltaForUI;

            if (data != 0)
            {
                var scrollAmount = (int)(data / 2.5f);
                var initialScrollOffset = chaptersScrollOffset;
                chaptersScrollOffset += scrollAmount;

                var lastBook = chapterLibrary[^1].area;
                var minScrollValue = -(lastBook.Bottom - (chapters.Height + chapters.Y));

                chaptersScrollOffset = minScrollValue < 0 ? int.Clamp(chaptersScrollOffset, minScrollValue, 0) : 0;

                //if (ChaptersScrollOffset != initialScrollOffset)
                //    SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.3f });
            }
        }

        // Lerp to the real scroll value to create smooth transitions
        realChaptersScrollOffset = MathHelper.Lerp(realChaptersScrollOffset, chaptersScrollOffset, scrollAcceleration);

        // Re-set the chapter library rectangles
        chapterLibrary.Clear();
        var chapter = chapters.CookieCutter(new Vector2(0f, -0.9f), new Vector2(1f, 0.092f));
        var xOffset = 0f;

        // Add any available chapters
        foreach (var questLine in availableChapters)
        {
            if (!questLine.VisibleInLog() && !UseDesigner)
            {
                continue;
            }

            chapterLibrary.Add((chapter, questLine));
            chapter = chapter.CookieCutter(new Vector2(0f, 2.25f), Vector2.One);
        }

        // Lerp between book chapters to create smooth transitions
        if (previousBookSwipeOffset > 0f)
        {
            previousBookSwipeOffset = MathHelper.Lerp(previousBookSwipeOffset, 0f, 0.25f);
        }

        // Check if we've completed the swiping
        if (previousBookSwipeOffset <= 0.005f)
        {
            previousBook = null;
            previousBookSwipeOffset = 0f;
        }

        // Otherwise add in the previously visible chapters
        else
        {
            // We swipe from right to left if going "forward" chapters,
            // and left to right if going "back" chapters
            var sign = previousBookSwipeDirection ? -1 : 1;
            var firstChapter = chapters.CookieCutter(new Vector2(0f, -0.9f), new Vector2(1f, 0.1f));

            var nextChapter = firstChapter.CookieCutter(new Vector2(2.2f * sign, 0f), Vector2.One);
            nextChapter.Offset(0, previousChapterScrollOffset - (int)realChaptersScrollOffset);
            xOffset = nextChapter.Width * 1.1f * previousBookSwipeOffset * -sign;

            var previousChapters = previousBook?.Chapters ?? [];

            foreach (var questLine in previousChapters)
            {
                if (!questLine.VisibleInLog() && !UseDesigner)
                {
                    continue;
                }

                chapterLibrary.Add((nextChapter, questLine));
                nextChapter = nextChapter.CookieCutter(new Vector2(0f, 2.25f), Vector2.One);
            }
        }

        // Check for selecting a new questline and draw each questline to the area
        foreach (var (rectangle, questLine) in chapterLibrary)
        {
            rectangle.Offset((int)xOffset, (int)realChaptersScrollOffset);
            var hovered = hoveringChapters && rectangle.Contains(mouseChapters) && SelectedElement is null;

            if (hovered && LeftMouseJustReleased && (questLine.IsUnlocked() || UseDesigner)) // && questElementSwipeOffset == 0f)
            {
                var selectedChapter = questLine == SelectedChapter ? null : questLine;
                SelectChapter(selectedChapter);
            }

            var selected = SelectedChapter == questLine;
            DrawTasks.Add(sb => questLine.Draw(sb, rectangle, TargetScale, selected, hovered));

            //AddRectangle(rectangle, Color.Red * 0.5f);
        }

        SwitchTargets(null);
    }
}