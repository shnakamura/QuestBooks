using System.Runtime.CompilerServices;

namespace QuestBooks.Core.Graphics;

internal static class SpriteBatchScopeAccessor
{
    /// <summary>
    ///     Gets a value indicating whether a sprite batch has been started.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to check.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if the sprite batch has been started; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "beginCalled")]
    internal static extern ref readonly bool GetBeginCalled(SpriteBatch spriteBatch);
}

public readonly ref struct SpriteBatchScope
{
    /// <summary>
    ///     The parameters of the previously active sprite batch.
    /// </summary>
    /// <value>
    ///     The parameters of the previously active sprite batch if one was active when the scope was
    ///     created; otherwise, <see langword="null" />.
    /// </value>
    private readonly SpriteBatchParameters? cache;

    /// <summary>
    ///     The sprite batch managed by the scope.
    /// </summary>
    private readonly SpriteBatch spriteBatch;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteBatchScope"/> <see langword="struct"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to manage.
    /// </param>
    /// <param name="parameters">
    ///     The parameters used to begin the scoped sprite batch.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="spriteBatch"/> is <see langword="null" />.
    /// </exception>
    public SpriteBatchScope(SpriteBatch spriteBatch, in SpriteBatchParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);

        this.spriteBatch = spriteBatch;

        if (SpriteBatchScopeAccessor.GetBeginCalled(spriteBatch))
        {
            this.spriteBatch.End(out var previous);
            this.spriteBatch.Begin(in parameters);

            cache = previous;
        }
        else
        {
            this.spriteBatch.Begin(in parameters);
        }
    }

    public void Dispose()
    {
        if (SpriteBatchScopeAccessor.GetBeginCalled(spriteBatch))
        {
            spriteBatch.End();
        }

        if (cache.HasValue)
        {
            spriteBatch.Begin(cache.Value);
        }
    }
}

public static class SpriteBatchScopeExtensions
{
    public static SpriteBatchScope Scope(this SpriteBatch spriteBatch, in SpriteBatchParameters parameters) => new(spriteBatch, in parameters);
}