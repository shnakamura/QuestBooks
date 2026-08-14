using Terraria.UI;

namespace QuestBooks.Common.UI.Elements; 

public class Element : UIElement
{
    /// <summary>
    ///     Sets the padding for all sides of the element, in pixels.
    /// </summary>
    public float Padding
    {
        set => this.Padding(value);
    }

    public (float Width, float Height) Fill
    {
        set => this.Fill(value.Width, value.Height);
    }

    public (float Horizontal, float Vertical) Allign
    {
        set => this.Allign(value.Horizontal, value.Vertical);
    }
}