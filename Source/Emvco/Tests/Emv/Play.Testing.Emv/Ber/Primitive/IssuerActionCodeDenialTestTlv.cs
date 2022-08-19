using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerActionCodeDenialTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 23, 14, 144, 51, 91 };

    public IssuerActionCodeDenialTestTlv() : base(_DefaultContentOctets) { }

    public IssuerActionCodeDenialTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerActionCodeDenial.Tag;
}
