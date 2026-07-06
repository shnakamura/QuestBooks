using System.IO;
using QuestBooks.Systems.NetCode;

namespace QuestBooks;

public sealed partial class QuestBooksMod
{
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        var packetType = PacketManager.IdToPacket[reader.ReadByte()];
        var packet = (QuestPacket)Activator.CreateInstance(packetType)!;
        
        packet.HandlePacket(in reader, whoAmI);
    }
}