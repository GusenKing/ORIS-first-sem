namespace XProtocol.XPacket;

public class XPacketTypeManager
{
    private static readonly Dictionary<XPacketType, Tuple<byte, byte>> TypeDictionary = new();

    static XPacketTypeManager()
    {
        TypeDictionary[XPacketType.Handshake] = Tuple.Create<byte, byte>(0, 0);
        TypeDictionary[XPacketType.Message] = Tuple.Create<byte, byte>(1, 1);
    }

    public static void RegisterType(XPacketType type, byte btype, byte bsubtype)
    {

    }

    public static Tuple<byte, byte> GetType(XPacketType type)
    {
        if (!TypeDictionary.ContainsKey(type))
            throw new Exception($"Packet type {type: G} is not registered.");

        return TypeDictionary[type];
    }

    public static XPacketType GetTypeFromPacket(XPacket packet)
    {
        var type = packet.PacketType;
        var subtype = packet.PacketSubtype;

        var packetType = TypeDictionary.Keys.FirstOrDefault(key => TypeDictionary[key].Item1 == type 
                                                                   && TypeDictionary[key].Item2 == subtype);

        return packetType;
    }
}
