using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerPublicKeyCertificateTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 40, 81, 38 };

    public IssuerPublicKeyCertificateTestTlv() : base(_DefaultContentOctets) { }

    public IssuerPublicKeyCertificateTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerPublicKeyCertificate.Tag;
}
