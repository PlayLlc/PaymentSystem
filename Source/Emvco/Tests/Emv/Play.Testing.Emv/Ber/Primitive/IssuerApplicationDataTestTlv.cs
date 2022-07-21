using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerApplicationDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { };

    public IssuerApplicationDataTestTlv() : base(_DefaultContentOctets) { }

    public IssuerApplicationDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerApplicationData.Tag;
}
