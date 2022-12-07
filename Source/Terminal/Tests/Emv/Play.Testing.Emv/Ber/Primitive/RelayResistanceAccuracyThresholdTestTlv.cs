using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class RelayResistanceAccuracyThresholdTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 36, 119 };

    public RelayResistanceAccuracyThresholdTestTlv() : base(_DefaultContentOctets) { }

    public RelayResistanceAccuracyThresholdTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => RelayResistanceAccuracyThreshold.Tag;
}
