using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class MeasuredRelayResistanceProcessingTimeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 36, 98 };

    public MeasuredRelayResistanceProcessingTimeTestTlv() : base(_DefaultContentOctets) { }

    public MeasuredRelayResistanceProcessingTimeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => MeasuredRelayResistanceProcessingTime.Tag;
}
