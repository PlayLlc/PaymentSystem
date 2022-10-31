using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TimeoutValueTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 8 };

    public TimeoutValueTestTlv() : base(_DefaultContentOctets) { }

    public TimeoutValueTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TimeoutValue.Tag;
}
