using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerCountryCodeAlpha2TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { (byte)'R', (byte)'O' };

    public IssuerCountryCodeAlpha2TestTlv() : base(_DefaultContentOctets) { }

    public IssuerCountryCodeAlpha2TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerCountryCodeAlpha2.Tag;
}
