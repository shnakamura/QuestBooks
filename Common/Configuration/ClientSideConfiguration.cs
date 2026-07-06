using Terraria.ModLoader.Config;

namespace QuestBooks.Common.Configuration;

public sealed class ClientSideConfiguration : ModConfig
{
    /// <summary>
    ///     Gets the singleton instance of the <see cref="ClientSideConfiguration"/> class.
    /// </summary>
    /// <remarks>
    ///     Shorthand for <see cref="ModContent.GetInstance{T}"/>.
    /// </remarks>
    public static ClientSideConfiguration Instance => ModContent.GetInstance<ClientSideConfiguration>();
    
    public override ConfigScope Mode => ConfigScope.ClientSide;
}