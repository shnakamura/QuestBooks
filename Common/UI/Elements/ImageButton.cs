using QuestBooks.Core.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace QuestBooks.Common.UI.Elements;

public class ImageButton(Asset<Texture2D> texture, Rectangle? frame = null) : Image(texture, frame)
{
    /// <summary>
    ///     Gets the sound played when the cursor hover overs the button.
    /// </summary>
    public SoundStyle HoverSound { get; init; } = SoundID.MenuTick with
    {
        MaxInstances = 1,
        SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
    };

    /// <summary>
    ///     Gets the sound played when the cursor clicks the button.
    /// </summary>
    public SoundStyle ClickSound { get; init; } = SoundID.MenuOpen with
    {
        MaxInstances = 1,
        SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
    };

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);

        SoundEngine.PlaySound(HoverSound);
    }

    public override void MouseOut(UIMouseEvent evt)
    {
        base.MouseOut(evt);
        
        SoundEngine.PlaySound(HoverSound);
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);

        SoundEngine.PlaySound(ClickSound);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (IsMouseHovering)
        {
            var dimensions = GetDimensions();
        
            var texture = Texture.Value;
            var size = Frame.HasValue ? Frame.Value.Size() : texture.Size();
        
            var position = dimensions.Position() + size * Origin;

            position = position.Floor();

            var parameters = spriteBatch.Capture() with
            {
                SpriteSortMode = SpriteSortMode.Immediate
            };

            using var scope = spriteBatch.Scope(in parameters);

            Main.pixelShader.CurrentTechnique.Passes["ColorOnly"].Apply();
        
            spriteBatch.Draw(Texture.Value, position + new Vector2(0f, 2f), Frame, UICommon.DefaultUIBorderMouseOver * Opacity, Rotation, size * Origin, Scale, Effects, 0f);
            spriteBatch.Draw(Texture.Value, position + new Vector2(0f, -2f), Frame, UICommon.DefaultUIBorderMouseOver * Opacity, Rotation, size * Origin, Scale, Effects, 0f);
            spriteBatch.Draw(Texture.Value, position + new Vector2(2f, 0f), Frame, UICommon.DefaultUIBorderMouseOver * Opacity, Rotation, size * Origin, Scale, Effects, 0f);
            spriteBatch.Draw(Texture.Value, position + new Vector2(-2f, 0f), Frame, UICommon.DefaultUIBorderMouseOver * Opacity, Rotation, size * Origin, Scale, Effects, 0f);
        }
        
        base.DrawSelf(spriteBatch);
    }
}