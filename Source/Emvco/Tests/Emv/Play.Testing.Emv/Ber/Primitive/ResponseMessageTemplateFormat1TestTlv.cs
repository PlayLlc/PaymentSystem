using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ResponseMessageTemplateFormat1TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
    {
        0x54, 0x44, 0x43, 0x20, 0x42, 0x4C, 0x41, 0x43, 0x4B, 0x20,
        0x55, 0x4E, 0x4C, 0x49, 0x4D, 0x49, 0x54, 0x45, 0x44, 0x20,
        0x56, 0x49, 0x53, 0x41, 0x20, 0x20
    };

    public ResponseMessageTemplateFormat1TestTlv() : base(_DefaultContentOctets) { }

    public ResponseMessageTemplateFormat1TestTlv(byte[] contentOctets) : base(contentOctets) { }

    public override Tag GetTag() => ResponseMessageTemplateFormat1.Tag;
}
