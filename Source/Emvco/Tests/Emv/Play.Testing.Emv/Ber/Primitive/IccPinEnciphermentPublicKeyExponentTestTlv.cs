using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IccPinEnciphermentPublicKeyExponentTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 34, 114 };

    public IccPinEnciphermentPublicKeyExponentTestTlv() : base(_DefaultContentOctets) { }

    public IccPinEnciphermentPublicKeyExponentTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IccPinEnciphermentPublicKeyExponent.Tag;
}
