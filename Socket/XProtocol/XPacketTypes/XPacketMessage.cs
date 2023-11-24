using XProtocol.XPacketAttributes;

namespace XProtocol.XPacketTypes;

public class XPacketMessage
{
    [XPacketField(1)]
    public byte[] From;

    [XPacketField(2)]
    public byte[] To;

    [XPacketField(3)]
    public byte[] DateTime;

    [XPacketField(4)]
    public byte[] Message;
}