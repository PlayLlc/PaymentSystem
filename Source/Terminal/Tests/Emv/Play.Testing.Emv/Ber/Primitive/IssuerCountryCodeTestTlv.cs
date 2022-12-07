using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerCountryCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 8, 40 };

    public IssuerCountryCodeTestTlv() : base(_DefaultContentOctets) { }

    public IssuerCountryCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerCountryCode.Tag;
}
