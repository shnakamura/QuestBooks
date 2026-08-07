using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace QuestBooks.Core.Graphics;

internal static class SpriteBatchParametersAccessor
{
    /// <summary>
    ///     Gets the sprite sort mode of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to get the sprite sort mode of.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSortMode" /> currently used by the sprite batch.
    /// </returns>
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "sortMode")]
    internal static extern ref readonly SpriteSortMode GetSpriteSortMode(SpriteBatch spriteBatch);

    /// <summary>
    ///     Gets the blend state of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to get the blend state of.
    /// </param>
    /// <returns>
    ///     The <see cref="BlendState" /> currently used by the sprite batch.
    /// </returns>
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "blendState")]
    internal static extern ref readonly BlendState GetBlendState(SpriteBatch spriteBatch);

    /// <summary>
    ///     Gets the sampler state of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to get the sampler state of.
    /// </param>
    /// <returns>
    ///     The <see cref="SamplerState" /> currently used by the sprite batch.
    /// </returns>
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "samplerState")]
    internal static extern ref readonly SamplerState GetSamplerState(SpriteBatch spriteBatch);

    /// <summary>
    ///     Gets the depth-stencil state of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to get the depth-stencil state of.
    /// </param>
    /// <returns>
    ///     The <see cref="DepthStencilState" /> currently used by the sprite batch.
    /// </returns>
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "depthStencilState")]
    internal static extern ref readonly DepthStencilState GetDepthStencilState(SpriteBatch spriteBatch);

    /// <summary>
    ///     Gets the rasterizer state of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to get the rasterizer state of.
    /// </param>
    /// <returns>
    ///     The <see cref="RasterizerState" /> currently used by the sprite batch.
    /// </returns>
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "rasterizerState")]
    internal static extern ref readonly RasterizerState GetRasterizerState(SpriteBatch spriteBatch);

    /// <summary>
    ///     Gets the effect of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to get the effect of.
    /// </param>
    /// <returns>
    ///     The <see cref="Effect" /> currently used by the sprite batch.
    /// </returns>
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "customEffect")]
    internal static extern ref readonly Effect GetEffect(SpriteBatch spriteBatch);

    /// <summary>
    ///     Gets the transform matrix of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to get the transform matrix of.
    /// </param>
    /// <returns>
    ///     The transformation <see cref="Matrix" /> currently used by the sprite batch.
    /// </returns>
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "transformMatrix")]
    internal static extern ref readonly Matrix GetTransformMatrix(SpriteBatch spriteBatch);
}

public record struct SpriteBatchParameters
{
    /// <summary>
    ///     Gets or sets the sprite sort mode.
    /// </summary>
    public required SpriteSortMode SpriteSortMode { readonly get; set; }

    /// <summary>
    ///     Gets or sets the blend state.
    /// </summary>
    public required BlendState BlendState { readonly get; set; }

    /// <summary>
    ///     Gets or sets the sampler state.
    /// </summary>
    public required SamplerState SamplerState { readonly get; set; }

    /// <summary>
    ///     Gets or sets the depth-stencil state.
    /// </summary>
    public required DepthStencilState DepthStencilState { readonly get; set; }

    /// <summary>
    ///     Gets or sets the rasterizer state.
    /// </summary>
    public required RasterizerState RasterizerState { readonly get; set; }

    /// <summary>
    ///     Gets or sets the effect applied during rendering.
    /// </summary>
    public required Effect Effect { readonly get; set; }

    /// <summary>
    ///     Gets or sets the transformation matrix applied during rendering.
    /// </summary>
    public required Matrix TransformMatrix { readonly get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteBatchParameters" />
    ///     <see langword="struct" /> from the current rendering state of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to capture the rendering state from.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="spriteBatch" /> is <see langword="null" />.
    /// </exception>
    [SetsRequiredMembers]
    internal SpriteBatchParameters(SpriteBatch spriteBatch)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);

        SpriteSortMode = SpriteBatchParametersAccessor.GetSpriteSortMode(spriteBatch);
        BlendState = SpriteBatchParametersAccessor.GetBlendState(spriteBatch);
        SamplerState = SpriteBatchParametersAccessor.GetSamplerState(spriteBatch);
        DepthStencilState = SpriteBatchParametersAccessor.GetDepthStencilState(spriteBatch);
        RasterizerState = SpriteBatchParametersAccessor.GetRasterizerState(spriteBatch);
        Effect = SpriteBatchParametersAccessor.GetEffect(spriteBatch);
        TransformMatrix = SpriteBatchParametersAccessor.GetTransformMatrix(spriteBatch);
    }
}

public static class SpriteBatchParametersExtensions
{
    /// <summary>
    ///     Begins a sprite batch with the specified parameters.    
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to begin.
    /// </param>
    /// <param name="parameters">
    ///     The parameters to begin the sprite batch with.
    /// </param>
    public static void Begin(this SpriteBatch spriteBatch, in SpriteBatchParameters parameters)
    {
        spriteBatch.Begin
        (
            parameters.SpriteSortMode, 
            parameters.BlendState, 
            parameters.SamplerState, 
            parameters.DepthStencilState,
            parameters.RasterizerState, 
            parameters.Effect,
            parameters.TransformMatrix
        );
    }
    
    /// <summary>
    ///     Ends a sprite batch 
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to end.
    /// </param>
    /// <param name="parameters">
    ///     
    /// </param>
    public static void End(this SpriteBatch spriteBatch, out SpriteBatchParameters parameters)
    {
        parameters = spriteBatch.Capture();
        
        spriteBatch.End();
    }

    /// <summary>
    ///     Captures the parameters of a sprite batch.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The sprite batch to capture the parameters of.
    /// </param>
    /// <returns>
    ///     The captured sprite batch parameters.
    /// </returns>
    public static SpriteBatchParameters Capture(this SpriteBatch spriteBatch) => new SpriteBatchParameters(spriteBatch);
}