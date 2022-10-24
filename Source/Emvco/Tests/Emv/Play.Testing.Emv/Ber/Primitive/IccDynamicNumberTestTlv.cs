using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class IccDynamicNumberTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x56, 0x49, 0x53, 0x41, 0x20 };

    public IccDynamicNumberTestTlv() : base(_DefaultContentOctets) { }

    public IccDynamicNumberTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => IccDynamicNumber.Tag;
}
