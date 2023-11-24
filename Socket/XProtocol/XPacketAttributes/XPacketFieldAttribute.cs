namespace XProtocol.XPacketAttributes;

[AttributeUsage(AttributeTargets.Field)]
public class XPacketFieldAttribute : Attribute
{
    public byte FieldID { get; }
    public XPacketFieldAttribute(byte fieldId)
    {
        FieldID = fieldId;
    }
}