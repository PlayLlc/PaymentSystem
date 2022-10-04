using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IccPinEnciphermentPublicKeyRemainderTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 34, 80, 51, 67, 113, 201, 91, 55, 67,
            32, 13, 44, 56, 13, 68};

    public IccPinEnciphermentPublicKeyRemainderTestTlv() : base(_DefaultContentOctets) { }

    public IccPinEnciphermentPublicKeyRemainderTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IccPinEnciphermentPublicKeyRemainder.Tag;
}
