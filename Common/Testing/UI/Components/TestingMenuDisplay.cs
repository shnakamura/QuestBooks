using QuestBooks.Common.UI;
using QuestBooks.Quests;
using Terraria.UI;

namespace QuestBooks.Common.Testing.UI.Components;

public sealed class TestingMenuDisplay : UIElement
{
    private TestingMenuQuestDisplay questDisplay;
    
    private TestingMenuEmptyDisplay emptyDisplay;
    
    private TestingMenuFilterList filterList;
    
    public override void OnInitialize()
    {
        base.OnInitialize();

        questDisplay = new TestingMenuQuestDisplay
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };

        emptyDisplay = new TestingMenuEmptyDisplay
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };

        filterList = new TestingMenuFilterList
        {
            Width = StyleDimension.FromPercent(1f),
            Height = StyleDimension.FromPercent(1f)
        };
        
        Append(filterList);
    }

    public void ToggleQuestDisplay(Quest quest)
    {
        ArgumentNullException.ThrowIfNull(quest);
        
        this.TryClear();

        if (questDisplay.TrySetQuest(quest))
        {
            SetQuestDisplay(quest);
        }
        else
        {
            SetEmptyDisplay();
        }
    }

    public void SetQuestDisplay(Quest quest)
    {
        ArgumentNullException.ThrowIfNull(quest);
        
        questDisplay.TrySetQuest(quest);
        
        this.TryClear();
        
        Append(questDisplay);
    }

    public void SetEmptyDisplay()
    {
        questDisplay.TryClearQuest();
        
        this.TryClear();
        
        Append(emptyDisplay);
    }

    public void SetFiltersDisplay()
    {
        this.TryClear();
        
        Append(filterList);
    }
}