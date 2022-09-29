using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MinimumRelayResistanceGracePeriodTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 34, 116 };

    public MinimumRelayResistanceGracePeriodTestTlv() : base(_DefaultContentOctets) { }

    public MinimumRelayResistanceGracePeriodTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MinimumRelayResistanceGracePeriod.Tag;
}
