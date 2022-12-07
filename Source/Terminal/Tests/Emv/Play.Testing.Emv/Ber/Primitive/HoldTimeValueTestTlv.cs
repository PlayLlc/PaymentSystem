using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class HoldTimeValueTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12 };

    public HoldTimeValueTestTlv() : base(_DefaultContentOctets) { }

    public HoldTimeValueTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => HoldTimeValue.Tag;
}
