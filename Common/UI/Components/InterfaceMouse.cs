using Terraria.UI;

namespace QuestBooks.Common.UI;

public sealed class InterfaceMouse : ElementComponent
{
    /// <inheritdoc/> 
    public override void Attach(Element element)
    {
        base.Attach(element);
        
        element.OnUpdate += Update;
    }

    /// <inheritdoc/> 
    public override void Detach(Element element)
    {
        base.Detach(element);
        
        element.OnUpdate -= Update;
    }

    private void Update(UIElement element) => Main.LocalPlayer.mouseInterface |= element.IsMouseHovering;
}