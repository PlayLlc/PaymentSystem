using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IssuerPublicKeyRemainderTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = {
    33, 44, 55, 122, 16, 64, 36,
    33, 44, 55, 122, 16, 64, 36 };

    public IssuerPublicKeyRemainderTestTlv() : base(_DefaultContentOctets) {}

    public IssuerPublicKeyRemainderTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IssuerPublicKeyRemainder.Tag;
}
