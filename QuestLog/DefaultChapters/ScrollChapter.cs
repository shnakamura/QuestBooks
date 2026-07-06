using QuestBooks.Assets;
using QuestBooks.Utilities;
using Terraria.GameContent;

namespace QuestBooks.QuestLog.DefaultChapters;

/// <summary>
///     Represents a basic <see cref="QuestChapter" /> implementation. Always visible, always unlocked.
/// </summary>
[ChapterTooltip("ScrollChapter")]
public class ScrollChapter : BasicChapter
{
    /// <summary>
    ///     Performs the default drawing behavior for this <see cref="ScrollChapter" />. Assigns colors and calls
    ///     <see cref="DrawBasicChapter(SpriteBatch, string, Color, Color, Color, Color, Rectangle, float)(SpriteBatch, string, Color, Color, Color, Rectangle, float)" />.
    /// </summary>
    public override void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered)
    {
        var unlocked = IsUnlocked();
        var chapterColor = unlocked ? Color.White : Color.DarkGray;
        Color outlineColor = new(0, 0, 0, 0);
        var textColor = unlocked ? Color.White : Color.Gray;
        Color textOutlineColor = unlocked ? new Color(69, 69, 69, 255) : new Color(40, 40, 40, 255);

        if (selected)
        {
            outlineColor = new Color(225, 225, 0, 255);
        }

        else if (hovered)
        {
            outlineColor = new Color(200, 200, 200, 255);
        }

        DrawBasicChapter(spriteBatch, DisplayName, chapterColor, textColor, outlineColor, textOutlineColor, designatedArea, scale);
    }

    /// <summary>
    ///     Performs the default chapter drawing code to the spritebatch. Draws a simple container with the specified colors, and text inside the contianer.
    /// </summary>
    public static void DrawBasicChapter
        (SpriteBatch spriteBatch, string text, Color chapterColor, Color textColor, Color outlineColor, Color textOutlineColor, Rectangle area, float scale, Effect? shader = null)
    {
        if (outlineColor.A > 0)
        {
            spriteBatch.Draw(QuestAssets.ChapterScrollBorder, area.Center(), null, outlineColor, 0f, QuestAssets.ChapterScrollBorder.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        spriteBatch.Draw(QuestAssets.ChapterScroll, area.Center(), null, chapterColor, 0f, QuestAssets.ChapterScroll.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);

        spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, blend, SamplerState.LinearClamp, depth, raster, effect, matrix);

        DrawChapterText(spriteBatch, text, textColor, textOutlineColor, area, scale);

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
    }

    /// <summary>
    ///     Performs the default chapter text drawing code to the spritebatch. Draws the text as it should sit within the given rectangle with the specified colors.
    /// </summary>
    public static void DrawChapterText(SpriteBatch spriteBatch, string text, Color textColor, Color outlineColor, Rectangle area, float scale)
    {
        var nameRectangle = area.CreateScaledMargins(0.12f, 0.25f); //.CreateScaledMargins(left: 0.1f, right: 0.165f, top: 0.1f, bottom: 0.1f);

        var font = FontAssets.DeathText.Value;

        spriteBatch.DrawOutlinedStringInRectangle
            (nameRectangle.CookieCutter(new Vector2(0f, 0.36f), new Vector2(1f, 0.85f)), font, textColor, outlineColor, text, alignment: TextAlignment.Left, clipBounds: false);
    }
}