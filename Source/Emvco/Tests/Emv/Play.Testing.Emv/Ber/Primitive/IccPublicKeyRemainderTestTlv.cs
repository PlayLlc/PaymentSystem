using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IccPublicKeyRemainderTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x56, 0x49, 0x53, 0x41, 0x20, 0x56, 0x49, 0x53, 0x41, 0x20 };

    public IccPublicKeyRemainderTestTlv() : base(_DefaultContentOctets) { }

    public IccPublicKeyRemainderTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IccPublicKeyRemainder.Tag;
}

