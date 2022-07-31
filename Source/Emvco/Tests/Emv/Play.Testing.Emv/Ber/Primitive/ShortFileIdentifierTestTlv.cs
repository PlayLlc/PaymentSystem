using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ShortFileIdentifierTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 14 };

    public ShortFileIdentifierTestTlv() : base(_DefaultContentOctets) { }

    public ShortFileIdentifierTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ShortFileIdentifier.Tag;
}
