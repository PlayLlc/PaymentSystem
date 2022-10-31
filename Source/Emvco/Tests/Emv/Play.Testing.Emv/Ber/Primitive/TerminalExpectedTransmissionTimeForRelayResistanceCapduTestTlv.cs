using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalExpectedTransmissionTimeForRelayResistanceCapduTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 74, 128 };

    public TerminalExpectedTransmissionTimeForRelayResistanceCapduTestTlv() : base(_DefaultContentOctets) { }

    public TerminalExpectedTransmissionTimeForRelayResistanceCapduTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalExpectedTransmissionTimeForRelayResistanceCapdu.Tag;
}
