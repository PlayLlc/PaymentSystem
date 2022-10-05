using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IccPublicKeyExponentTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 33, 124 };

    public IccPublicKeyExponentTestTlv() : base(_DefaultContentOctets) { }

    public IccPublicKeyExponentTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IccPublicKeyExponent.Tag;
}
