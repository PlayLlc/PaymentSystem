using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MobileSupportIndicatorTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 198 };

    public MobileSupportIndicatorTestTlv() : base(_DefaultContentOctets) { }

    public MobileSupportIndicatorTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MobileSupportIndicator.Tag;
}
