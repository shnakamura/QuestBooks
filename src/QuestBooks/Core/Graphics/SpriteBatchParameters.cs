namespace QuestBooks.Core.Graphics;

/// <summary>
///     Represents the rendering state used to begin a <see cref="SpriteBatch"/>.
/// </summary>
public record struct SpriteBatchParameters
{
    /// <summary>
    ///     Gets or sets the sprite sorting mode.
    /// </summary>
    public SpriteSortMode Sort { readonly get; set; }

    /// <summary>
    ///     Gets or sets the blend state.
    /// </summary>
    public BlendState Blend { readonly get; set; }

    /// <summary>
    ///     Gets or sets the sampler state.
    /// </summary>
    public SamplerState Sampler { readonly get; set; }

    /// <summary>
    ///     Gets or sets the depth stencil state.
    /// </summary>
    public DepthStencilState Depth { readonly get; set; }

    /// <summary>
    ///     Gets or sets the rasterizer state.
    /// </summary>
    public RasterizerState Rasterizer { readonly get; set; }

    /// <summary>
    ///     Gets or sets the shader effect.
    /// </summary>
    public Effect Effect { readonly get; set; }

    /// <summary>
    ///     Gets or sets the transformation matrix.
    /// </summary>
    public Matrix Matrix { readonly get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteBatchParameters"/> structure with the specified rendering state.
    /// </summary>
    /// <param name="sort">
    ///     The sprite sorting mode.
    /// </param>
    /// <param name="blend">
    ///     The blend state.
    /// </param>
    /// <param name="sampler">
    ///     The sampler state.
    /// </param>
    /// <param name="depth">
    ///     The depth stencil state.
    /// </param>
    /// <param name="rasterizer">
    ///     The rasterizer state.
    /// </param>
    /// <param name="effect">
    ///     The shader effect.
    /// </param>
    /// <param name="matrix">
    ///     The transformation matrix.
    /// </param>
    public SpriteBatchParameters
    (
        SpriteSortMode sort,
        BlendState blend,
        SamplerState sampler,
        DepthStencilState depth,
        RasterizerState rasterizer,
        Effect effect,
        Matrix matrix
    )
    {
        Sort = sort;
        Blend = blend;
        Sampler = sampler;
        Depth = depth;
        Rasterizer = rasterizer;
        Effect = effect;
        Matrix = matrix;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteBatchParameters"/> structure by capturing the state of a <see cref="SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to capture.
    /// </param>
    public SpriteBatchParameters(SpriteBatch spriteBatch)
    {
        Sort = spriteBatch.sortMode;
        Blend = spriteBatch.blendState;
        Sampler = spriteBatch.samplerState;
        Depth = spriteBatch.depthStencilState;
        Rasterizer = spriteBatch.rasterizerState;
        Effect = spriteBatch.spriteEffect;
        Matrix = spriteBatch.transformMatrix;
    }
}

/// <summary>
///     Provides <see cref="SpriteBatchParameters"/> extensions for working with <see cref="SpriteBatchParameters"/>.
/// </summary>
public static class SpriteBatchParametersExtensions
{
    /// <summary>
    ///     Captures the current rendering state of a <see cref="SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to capture.
    /// </param>
    /// <returns>
    ///     A <see cref="SpriteBatchParameters"/> instance containing the captured rendering state.
    /// </returns>
    public static SpriteBatchParameters Capture(this SpriteBatch spriteBatch) => new(spriteBatch);
    
    /// <summary>
    ///     Begins a <see cref="SpriteBatch"/> using the specified rendering state.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to begin.
    /// </param>
    /// <param name="parameters">
    ///     The rendering state to use.
    /// </param>
    public static void Begin(this SpriteBatch spriteBatch, in SpriteBatchParameters parameters)
    {
        spriteBatch.Begin
        (
            parameters.Sort,
            parameters.Blend,
            parameters.Sampler,
            parameters.Depth,
            parameters.Rasterizer,
            parameters.Effect,
            parameters.Matrix
        );
    }

    /// <summary>
    ///     Captures the current state of a <see cref="SpriteBatch"/> before ending it.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to end.
    /// </param>
    /// <param name="parameters">
    ///     When this method returns, contains the rendering state that was active before the sprite batch was ended.
    /// </param>
    public static void End(this SpriteBatch spriteBatch, out SpriteBatchParameters parameters)
    {
        parameters = spriteBatch.Capture();
        
        spriteBatch.End();
    }
}