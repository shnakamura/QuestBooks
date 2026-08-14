namespace QuestBooks.Common.UI.Elements;

// ReSharper disable CompareOfFloatsByEqualityOperator
public class ProgressBar : Element
{
    public delegate void ProgressBarChangeCallback(float progress);
    
    /// <summary>
    ///     Raised when the progress of the progress bar is changed.
    /// </summary>
    public event ProgressBarChangeCallback OnChangeProgress;
    
    private float progress;
    
    /// <summary>
    ///     Gets or sets the progress of the progress bar.
    /// </summary>
    /// <value>
    ///     A value in the range of <c>[0f - 1f]</c>, where <c>0f</c> represents fully incomplete, and <c>1f</c> represents fully complete.
    /// </value>
    public float Progress
    {
        get => progress;
        set
        {
            progress = Math.Clamp(value, 0f, 1f);
            
            OnChangeProgress?.Invoke(progress);
        }
    }

    /// <summary>
    ///     Gets a value indicating whether the progress bar is complete.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the progress bar is complete; otherwise, <see langword="false"/>.
    /// </value>
    public bool Complete => Progress == 1f;

    /// <summary>
    ///     Gets a value indicating whether the progress bar is incomplete.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the progress bar is incomplete; otherwise, <see langword="false"/>.
    /// </value>
    public bool Incomplete => Progress != 1f;
}
// ReSharper restore CompareOfFloatsByEqualityOperator
