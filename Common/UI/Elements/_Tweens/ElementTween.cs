using QuestBooks.Common.Mathematics;

namespace QuestBooks.Common.UI;

public sealed class ElementTween<TValue> : IElementTween
{
    public Func<TValue, TValue, float, TValue> Interpolator { get; }

    public Func<float, float> Ease { get; } = Easings.Cubic.In;
    
    /// <summary>
    ///     Gets the getter function for the tweened value.
    /// </summary>
    public Func<TValue> Getter { get; }
    
    /// <summary>
    ///     Gets the setter function for the tweened value.
    /// </summary>
    public Action<TValue> Setter { get; }

    /// <summary>
    ///     Gets the starting value of the tween.
    /// </summary>
    public TValue Start { get; }

    /// <summary>
    ///     Gets the ending value of the tween.
    /// </summary>
    public TValue End { get; }

    /// <summary>
    ///     Gets the duration of the tween, in frames.
    /// </summary>
    public int Duration { get; }
    
    /// <summary>
    ///     Gets the elapsed time of the tween, in frames.
    /// </summary>
    public int Elapsed { get; private set; }
    
    /// <summary>
    ///     Gets a value indicating whether the tween is complete.
    /// </summary>
    /// <value>
    ///     <see langword="true"/> if the tween is complete; otherwise, <see langword="false"/>.
    /// </value>
    public bool Complete => Elapsed >= Duration;

    /// <inheritdoc/> 
    void IElementTween.Update() => Setter.Invoke(Interpolator.Invoke(Start, End, Ease.Invoke(Elapsed++ / (float)Duration)));
}