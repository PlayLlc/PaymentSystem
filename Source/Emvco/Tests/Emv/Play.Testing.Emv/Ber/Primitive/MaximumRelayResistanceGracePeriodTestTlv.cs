using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MaximumRelayResistanceGracePeriodTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 48 };

    public MaximumRelayResistanceGracePeriodTestTlv() : base(_DefaultContentOctets) { }

    public MaximumRelayResistanceGracePeriodTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MaximumRelayResistanceGracePeriod.Tag;
}
