using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IccPublicKeyCertificateTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
    133, 121, 94, 13, 7, 1, 67, 123, 44, 15, 234, 9, 90, 15, 23, 34 ,119};

    public IccPublicKeyCertificateTestTlv() : base(_DefaultContentOctets) { }

    public IccPublicKeyCertificateTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IccPublicKeyCertificate.Tag;
}
