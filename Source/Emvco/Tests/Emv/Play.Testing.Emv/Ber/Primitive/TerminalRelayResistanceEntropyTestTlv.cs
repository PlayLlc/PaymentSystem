using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalRelayResistanceEntropyTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 101, 16, 38, 216 };

    public TerminalRelayResistanceEntropyTestTlv() : base(_DefaultContentOctets) { }

    public TerminalRelayResistanceEntropyTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalRelayResistanceEntropy.Tag;
}
