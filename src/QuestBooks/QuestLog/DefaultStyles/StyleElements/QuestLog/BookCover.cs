using System.Linq;
using QuestBooks.Assets;
using QuestBooks.Core.Quests;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultStyles;

public partial class BasicQuestLogStyle
{
    private bool selectingQuestLog;
    private bool newLogSelected;

    private float logSelectionOffset;
    private float logSelectionOpacity;

    private void HandleBookCover(Vector2 questLogCenter)
    {
        var coverSize = QuestAssets.ClosedBook.Asset.Size() * LogScale;
        var coverRectangle = CenteredRectangle(questLogCenter, coverSize);
        var switchLog = coverRectangle.CookieCutter(new Vector2(0f, -1.15f), new Vector2(0.8f, 0.12f));

        if (selectingQuestLog)
        {
            HandleLogSelection(questLogCenter, switchLog);
            return;
        }

        var coverRectangleHovered = false;
        var switchLogHovered = false;

        if (coverRectangle.Contains(MouseCanvas))
        {
            LockMouse();
            coverRectangleHovered = true;

            if (LeftMouseJustReleased)
            {
                pageFlippingTimer = 45;
                onCoverPage = false;
            }
        }

        else if (switchLog.Contains(MouseCanvas))
        {
            LockMouse();
            switchLogHovered = true;
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.SelectQuestLog");

            if (LeftMouseJustReleased)
            {
                newLogSelected = false;
                logSelectionOffset = -1;
                selectingQuestLog = true;
            }
        }

        DrawTasks.Add
        (sb =>
            {
                if (coverRectangleHovered)
                {
                    Texture2D coverOutlineTexture = QuestAssets.ClosedBookOutline;
                    sb.Draw(coverOutlineTexture, questLogCenter, null, Color.White, 0f, coverOutlineTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);
                }

                Texture2D coverTexture = QuestAssets.ClosedBook;
                sb.Draw(coverTexture, questLogCenter, null, Color.White, 0f, coverTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);

                QuestLogDrawer.CoverDrawCalls[QuestManager.ActiveQuestLog](sb, questLogCenter, 0f, LogScale, 1f);
                var title = QuestLogDrawer.LogTitleRetrievalCalls[QuestManager.ActiveQuestLog](QuestManager.ActiveQuestLog);
                QuestLogDrawer.LogTitleDrawCalls[QuestManager.ActiveQuestLog](sb, switchLog, title, 1f, switchLogHovered, true);
            }
        );
    }

    private void HandleLogSelection(Vector2 questLogCenter, Rectangle switchLogLocation)
    {
        if (newLogSelected)
        {
            if (logSelectionOpacity > 0f)
            {
                logSelectionOpacity -= 0.025f;
                logSelectionOffset = float.Lerp(logSelectionOffset, 0f, 0.15f);
            }

            else
            {
                logSelectionOpacity = 0f;
                logSelectionOffset = 0f;
                selectingQuestLog = false;
            }
        }

        else
        {
            if (logSelectionOpacity < 1f)
            {
                logSelectionOpacity += 0.025f;
                logSelectionOffset = float.Lerp(logSelectionOffset, 0f, 0.15f);
            }

            else
            {
                logSelectionOpacity = 1f;
                logSelectionOffset = 0f;
            }
        }

        DrawTasks.Add
        (sb =>
            {
                Texture2D coverTexture = QuestAssets.ClosedBook;
                var coverOpacity = (float)Math.Pow(1d - logSelectionOpacity, 2d);
                sb.Draw(coverTexture, questLogCenter, null, Color.White * coverOpacity, 0f, coverTexture.Size() * 0.5f, LogScale, SpriteEffects.None, 0f);
                QuestLogDrawer.CoverDrawCalls[QuestManager.ActiveQuestLog](sb, questLogCenter, 0f, LogScale, coverOpacity);
            }
        );

        var drawArea = switchLogLocation;

        foreach (var log in QuestManager.AvailableQuestLogs.Select(kvp => kvp.Key))
        {
            var opacity = logSelectionOpacity;
            var logArea = drawArea;

            if (log == QuestManager.ActiveQuestLog)
            {
                opacity = 1f;

                if (newLogSelected)
                {
                    logArea = switchLogLocation;
                    logArea.Y += (int)logSelectionOffset;
                }

                else
                {
                    if (logSelectionOffset < 0f)
                    {
                        logSelectionOffset = logArea.Y - switchLogLocation.Y;
                    }

                    logArea.Y -= (int)logSelectionOffset;
                }
            }

            var logAreaHovered = false;

            if ((!newLogSelected && logArea.Contains(MouseCanvas)) || (newLogSelected && log == QuestManager.ActiveQuestLog))
            {
                LockMouse();
                logAreaHovered = true;

                if (LeftMouseJustReleased)
                {
                    QuestManager.SelectQuestLog(log);
                    newLogSelected = true;
                    logSelectionOffset = logArea.Y - switchLogLocation.Y;
                }
            }

            DrawTasks.Add
            (sb =>
                {
                    var title = QuestLogDrawer.LogTitleRetrievalCalls[log](log);
                    QuestLogDrawer.LogTitleDrawCalls[log](sb, logArea, title, opacity, logAreaHovered, true);
                }
            );

            drawArea = drawArea.CookieCutter(new Vector2(0f, 2.5f), Vector2.One);
        }
    }

    private void HandleCoverToggle()
    {
        var coverToggle = LogArea.CookieCutter(new Vector2(-1f, 1.03f), new Vector2(0.075f, 0.05f));
        var coverToggleHovered = false;

        if (coverToggle.Contains(MouseCanvas))
        {
            LockMouse();
            coverToggleHovered = true;
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Library.BackToCover");

            if (LeftMouseJustReleased)
            {
                pageFlippingTimer = 45;
                onCoverPage = true;
            }
        }

        DrawTasks.Add
        (sb =>
            {
                Texture2D backToCover = QuestAssets.BackToCover;
                var scale = coverToggle.Width / (float)backToCover.Width;
                sb.Draw(backToCover, coverToggle.Center(), null, coverToggleHovered ? Color.LightCyan : Color.White, 0f, backToCover.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            }
        );
    }
}