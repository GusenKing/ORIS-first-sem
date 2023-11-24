using XProtocol.XPacketAttributes;

namespace XProtocol.XPacketTypes;

public class XPacketHandshake
{
    [XPacketField(1)]
    public int MagicHandshakeNumber;
}