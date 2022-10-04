using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class RelayResistanceTransmissionTimeMismatchThresholdTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 134 };

    public RelayResistanceTransmissionTimeMismatchThresholdTestTlv() : base(_DefaultContentOctets) { }

    public RelayResistanceTransmissionTimeMismatchThresholdTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => RelayResistanceTransmissionTimeMismatchThreshold.Tag;
}
