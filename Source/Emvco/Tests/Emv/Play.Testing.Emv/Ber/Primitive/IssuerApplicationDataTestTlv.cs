using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerApplicationDataTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 18, 32, 34, 44, 67, 18, 114, 53, 109, 61, 28, 15, 209, 20 };

    public IssuerApplicationDataTestTlv() : base(_DefaultContentOctets) { }

    public IssuerApplicationDataTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerApplicationData.Tag;
}
