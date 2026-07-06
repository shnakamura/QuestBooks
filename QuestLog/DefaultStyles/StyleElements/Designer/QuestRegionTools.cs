using System.Collections.Generic;
using QuestBooks.Assets;
using QuestBooks.Systems;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultStyles;

public partial class BasicQuestLogStyle
{
    private readonly List<(Rectangle box, Type type)> elementSelections = [];
    private int elementTypeScrollOffset;
    private QuestLogElement placingElement;
    private const float zoomIncrement = 0.1f;

    private void HandleQuestRegionTools()
    {
        var enableShifting = LogArea.CookieCutter(new Vector2(0.12f, -1.1f), new Vector2(0.069f, 0.075f));
        var moveBounds = enableShifting.CookieCutter(new Vector2(0f, -2.3f), Vector2.One);
        var showMidpoint = moveBounds.CookieCutter(new Vector2(2.2f, 0f), Vector2.One);
        var showBackdrop = enableShifting.CookieCutter(new Vector2(2.2f, 0f), Vector2.One);
        var showGrid = showBackdrop.CookieCutter(new Vector2(2.2f, 0f), Vector2.One);
        var snapGrid = showGrid.CookieCutter(new Vector2(2.2f, 0f), Vector2.One);
        var scale = enableShifting.Width / (float)QuestAssets.ShiftingCanvas.Asset.Width;

        var gridSize = snapGrid.CookieCutter(new Vector2(2.2f, 0f), new Vector2(0.95f, 1f));
        var gridUp = gridSize.CookieCutter(new Vector2(1.4f, -0.5f), new Vector2(0.4f, 0.5f));
        var gridDown = gridUp.CookieCutter(new Vector2(0f, 2f), Vector2.One);

        var zoomScale = gridSize.CookieCutter(new Vector2(0f, -2.3f), Vector2.One);
        var zoomUp = zoomScale.CookieCutter(new Vector2(1.4f, -0.5f), new Vector2(0.4f, 0.5f));
        var zoomDown = zoomUp.CookieCutter(new Vector2(0f, 2f), Vector2.One);

        var enableShiftingHovered = false;
        var moveBoundsHovered = false;
        var showMidpointHovered = false;
        var showBackdropHovered = false;
        var showGridHovered = false;
        var snapGridHovered = false;

        var gridSizeHovered = false;
        var gridUpHovered = false;
        var gridDownHovered = false;

        var zoomScaleHovered = false;
        var zoomUpHovered = false;
        var zoomDownHovered = false;

        if (enableShifting.Contains(MouseCanvas))
        {
            LockMouse();
            enableShiftingHovered = true;
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ShiftingCanvas");

            if (LeftMouseJustReleased && SelectedChapter is not null)
            {
                var chapter = SelectedChapter;
                var oldAnchor = chapter.ViewAnchor;
                chapter.EnableShifting = !chapter.EnableShifting;

                if (chapter.EnableShifting)
                {
                    chapter.ViewAnchor = defaultAnchor;
                }

                else
                {
                    QuestAreaOffset = Vector2.Zero;
                }

                AddHistory
                (
                    () =>
                    {
                        chapter.EnableShifting = !chapter.EnableShifting;

                        if (chapter.EnableShifting)
                        {
                            chapter.ViewAnchor = oldAnchor;
                        }
                        else
                        {
                            QuestAreaOffset = Vector2.Zero;
                        }
                    },
                    () =>
                    {
                        chapter.EnableShifting = !chapter.EnableShifting;

                        if (chapter.EnableShifting)
                        {
                            chapter.ViewAnchor = defaultAnchor;
                        }
                        else
                        {
                            QuestAreaOffset = Vector2.Zero;
                        }
                    }
                );
            }
        }

        if (SelectedChapter?.EnableShifting ?? false)
        {
            if (moveBounds.Contains(MouseCanvas))
            {
                LockMouse();
                moveBoundsHovered = true;
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.MoveBounds");

                if (LeftMouseJustReleased)
                {
                    this.moveBounds = !this.moveBounds;
                }
            }

            else if (showMidpoint.Contains(MouseCanvas))
            {
                LockMouse();
                showMidpointHovered = true;
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ShowMidpoint");

                if (LeftMouseJustReleased)
                {
                    this.showMidpoint = !this.showMidpoint;
                }
            }

            else if (zoomScale.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ZoomScale");
                zoomScaleHovered = true;

                if (LeftMouseJustReleased)
                {
                    var chapter = SelectedChapter;
                    var oldZoom = chapter.DefaultZoom;

                    chapter.DefaultZoom = Zoom;
                    var newZoom = chapter.DefaultZoom;

                    if (oldZoom != newZoom)
                    {
                        AddHistory(() => { chapter.DefaultZoom = oldZoom; }, () => { chapter.DefaultZoom = newZoom; });
                    }
                }
            }

            else if (zoomUp.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ZoomUp");
                zoomUpHovered = true;

                if (LeftMouseJustReleased && SelectedChapter.DefaultZoom < 2f)
                {
                    var chapter = SelectedChapter;
                    chapter.DefaultZoom += zoomIncrement;

                    AddHistory(() => { chapter.DefaultZoom -= zoomIncrement; }, () => { chapter.DefaultZoom += zoomIncrement; });
                }
            }

            else if (zoomDown.Contains(MouseCanvas))
            {
                LockMouse();
                MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ZoomDown");
                zoomDownHovered = true;

                if (LeftMouseJustReleased && SelectedChapter.DefaultZoom > 0.1f)
                {
                    var chapter = SelectedChapter;
                    chapter.DefaultZoom -= 0.1f;

                    AddHistory(() => { chapter.DefaultZoom += zoomIncrement; }, () => { chapter.DefaultZoom -= zoomIncrement; });
                }
            }

            SelectedChapter.DefaultZoom = float.Round(SelectedChapter.DefaultZoom, 2);
        }

        if (showBackdrop.Contains(MouseCanvas))
        {
            LockMouse();
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ToggleBackdrop");
            showBackdropHovered = true;

            if (LeftMouseJustReleased)
            {
                this.showBackdrop = !this.showBackdrop;
            }
        }

        if (showGrid.Contains(MouseCanvas))
        {
            LockMouse();
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.ToggleGrid");
            showGridHovered = true;

            if (LeftMouseJustReleased)
            {
                this.showGrid = !this.showGrid;
            }
        }

        if (snapGrid.Contains(MouseCanvas))
        {
            LockMouse();
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.SnapGrid");
            snapGridHovered = true;

            if (LeftMouseJustReleased)
            {
                snapToGrid = !snapToGrid;
            }
        }

        if (gridSize.Contains(MouseCanvas))
        {
            LockMouse();
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.GridSize");
            gridSizeHovered = true;

            if (LeftMouseJustReleased)
            {
                this.gridSize = 20;
            }
        }

        else if (gridUp.Contains(MouseCanvas))
        {
            LockMouse();
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.GridSizeUp");
            gridUpHovered = true;

            if (LeftMouseJustReleased)
            {
                this.gridSize++;
            }
        }

        else if (gridDown.Contains(MouseCanvas))
        {
            LockMouse();
            MouseTooltip = Language.GetTextValue("Mods.QuestBooks.Tooltips.Designer.GridSizeDown");
            gridDownHovered = true;

            if (LeftMouseJustReleased && this.gridSize > 2)
            {
                this.gridSize--;
            }
        }

        DrawTasks.Add
        (sb =>
            {
                void DrawToggle(Rectangle area, bool hovered, Texture2D button, Texture2D buttonHovered, float opacity = 1f, bool outline = false)
                {
                    var texture = hovered ? buttonHovered : button;
                    var center = area.Center();

                    if (outline)
                    {
                        sb.Draw(QuestAssets.ToolOutline, center, null, Color.Yellow * opacity, 0f, QuestAssets.ToolOutline.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    }

                    sb.Draw(texture, center, null, Color.White * opacity, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                }

                var active = SelectedChapter is not null;

                DrawToggle
                (
                    enableShifting,
                    enableShiftingHovered,
                    QuestAssets.ShiftingCanvas,
                    active ? QuestAssets.ShiftingCanvasHovered : QuestAssets.ShiftingCanvas,
                    active ? 1f : 0.5f,
                    SelectedChapter?.EnableShifting ?? false
                );

                if (this.showBackdrop)
                {
                    DrawToggle(showBackdrop, showBackdropHovered, QuestAssets.ToggleBackdropEnabled, QuestAssets.ToggleBackdropEnabledHovered, outline: true);
                }

                else
                {
                    DrawToggle(showBackdrop, showBackdropHovered, QuestAssets.ToggleBackdropDisabled, QuestAssets.ToggleBackdropDisabledHovered);
                }

                DrawToggle(showGrid, showGridHovered, QuestAssets.ToggleGrid, QuestAssets.ToggleGridHovered, outline: this.showGrid);
                DrawToggle(snapGrid, snapGridHovered, QuestAssets.GridSnapping, QuestAssets.GridSnappingHovered, outline: snapToGrid);

                DrawToggle(gridSize, gridSizeHovered, QuestAssets.GridSize, QuestAssets.GridSizeHovered);
                DrawToggle(gridUp, gridUpHovered, QuestAssets.ScaleUp, QuestAssets.ScaleUpHovered);
                DrawToggle(gridDown, gridDownHovered, QuestAssets.ScaleDown, QuestAssets.ScaleDownHovered);

                var gridText = gridSize.CookieCutter(new Vector2(0.5f, 0.2f), new Vector2(0.75f, 0.75f));
                sb.DrawOutlinedStringInRectangle(gridText, FontAssets.DeathText.Value, Color.White, Color.Black, this.gridSize.ToString(), clipBounds: false);

                if (active && SelectedChapter.EnableShifting)
                {
                    DrawToggle(moveBounds, moveBoundsHovered, QuestAssets.MoveBounds, QuestAssets.MoveBoundsHovered, outline: this.moveBounds);
                    DrawToggle(showMidpoint, showMidpointHovered, QuestAssets.DisplayMidpoint, QuestAssets.DisplayMidpointHovered, outline: this.showMidpoint);

                    DrawToggle(zoomScale, zoomScaleHovered, QuestAssets.ZoomScale, QuestAssets.ZoomScaleHovered);
                    DrawToggle(zoomUp, zoomUpHovered, QuestAssets.ScaleUp, QuestAssets.ScaleUpHovered);
                    DrawToggle(zoomDown, zoomDownHovered, QuestAssets.ScaleDown, QuestAssets.ScaleDownHovered);

                    gridText = zoomScale.CookieCutter(new Vector2(0.5f, 0.2f), new Vector2(0.75f, 0.75f));
                    sb.DrawOutlinedStringInRectangle(gridText, FontAssets.DeathText.Value, Color.White, Color.Black, SelectedChapter.DefaultZoom.ToString("N1"), clipBounds: false);
                }
            }
        );

        if (SelectedChapter is not null)
        {
            var elementTypeSelection = LogArea.CookieCutter(new Vector2(1.24f, 0f), new Vector2(0.23f, 0.9f));
            AddRectangle(elementTypeSelection, Color.Gray * 0.6f, fill: true);
            AddRectangle(elementTypeSelection, Color.Black, 3f);

            var elementTypeDisplay = elementTypeSelection.CookieCutter(new Vector2(0f, -1.03f), new Vector2(1f, 0.075f));
            DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(elementTypeDisplay, FontAssets.DeathText.Value, Color.White, Color.Black, "Element Selection:"));

            var typeBox = elementTypeSelection.CreateScaledMargin(0.025f).CookieCutter(new Vector2(0f, -0.95f), new Vector2(1f, 0.078f));
            elementSelections.Clear();

            foreach (var elementType in QuestManager.AvailableQuestElementTypes.Keys)
            {
                elementSelections.Add((typeBox, elementType));
                typeBox = typeBox.CookieCutter(new Vector2(0, 2.2f), Vector2.One);
            }

            if (elementTypeSelection.Contains(MouseCanvas))
            {
                LockMouse();
                var data = PlayerInput.ScrollWheelDeltaForUI;

                if (data != 0)
                {
                    var scrollAmount = data / 6;
                    var initialOffset = elementTypeScrollOffset;
                    elementTypeScrollOffset += scrollAmount;

                    var lastBox = elementSelections[^1].box;
                    var minScrollValue = -(lastBox.Bottom - (elementTypeSelection.Height + elementTypeSelection.Y));

                    elementTypeScrollOffset = minScrollValue < 0 ? int.Clamp(elementTypeScrollOffset, minScrollValue, 0) : 0;
                }
            }

            DrawTasks.Add
            (sb =>
                {
                    sb.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
                    sb.End();

                    sb.GraphicsDevice.ScissorRectangle = elementTypeSelection;
                    raster.ScissorTestEnable = true;

                    sb.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
                }
            );

            foreach (var (box, elementType) in elementSelections)
            {
                var placing = (placingElement?.GetType() ?? null) == elementType;
                var otherPlacing = !placing && placingElement is not null;

                box.Offset(0, elementTypeScrollOffset);

                if (!placing)
                {
                    AddRectangle(box, Color.Gray, fill: true);
                    AddRectangle(box, Color.LightGray);
                }

                else
                {
                    AddRectangle(box, Color.PaleGoldenrod, fill: true);
                    AddRectangle(box, Color.Yellow);
                }

                var textArea = box.CookieCutter(new Vector2(0.2f, 0f), new Vector2(0.78f, 1f));
                var iconArea = box.CookieCutter(new Vector2(-0.775f, 0f), new Vector2(0.225f, 1f)).CreateScaledMargin(0.2f);

                DrawTasks.Add
                (sb =>
                    {
                        sb.DrawOutlinedStringInRectangle(textArea.CookieCutter(new Vector2(0f, 0.25f), Vector2.One), FontAssets.DeathText.Value, Color.White, Color.Black, elementType.Name);
                        QuestManager.AvailableQuestElementTypes[elementType].DrawDesignerIcon(sb, iconArea);
                    }
                );

                if (box.Contains(MouseCanvas) && elementTypeSelection.Contains(MouseCanvas))
                {
                    if (!placing)
                    {
                        AddRectangle(box, Color.White);
                    }

                    MouseTooltip = $"[c/CCC018:{elementType.FullName}]";

                    if (Attribute.GetCustomAttribute(elementType, typeof(TooltipAttribute)) is TooltipAttribute tooltip)
                    {
                        MouseTooltip += $"\n{Language.GetTextValue(tooltip.LocalizationKey)}";
                    }

                    if (LeftMouseJustReleased)
                    {
                        if ((placingElement?.GetType() ?? null) != elementType)
                        {
                            placingElement = (QuestLogElement)Activator.CreateInstance(elementType);
                            placingElement.PreviouslyPlaced = false;
                        }

                        else
                        {
                            placingElement = null;
                        }
                    }
                }
            }

            if (RightMouseJustReleased && (placingElement?.PreviouslyPlaced ?? false) && !JustMoved)
            {
                SelectedChapter.Elements.Add(placingElement);
                SortedElements = null;
            }
        }

        if (RightMouseJustReleased && !JustMoved)
        {
            placingElement = null;
        }
    }
}