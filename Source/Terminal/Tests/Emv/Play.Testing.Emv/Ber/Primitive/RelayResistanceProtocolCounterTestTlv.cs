using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class RelayResistanceProtocolCounterTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 15 };

    public RelayResistanceProtocolCounterTestTlv() : base(_DefaultContentOctets) { }

    public RelayResistanceProtocolCounterTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => RelayResistanceProtocolCounter.Tag;
}
