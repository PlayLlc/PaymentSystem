using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ApplicationIdentifierTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01, 0x12 };

    public ApplicationIdentifierTestTlv() : base(_DefaultContentOctets) { }

    public ApplicationIdentifierTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ApplicationIdentifier.Tag;
}
