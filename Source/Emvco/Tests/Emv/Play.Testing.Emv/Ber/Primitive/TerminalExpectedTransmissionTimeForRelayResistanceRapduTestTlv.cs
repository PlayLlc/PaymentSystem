using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalExpectedTransmissionTimeForRelayResistanceRapduTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 109, 14 };

    public TerminalExpectedTransmissionTimeForRelayResistanceRapduTestTlv() : base(_DefaultContentOctets) { }

    public TerminalExpectedTransmissionTimeForRelayResistanceRapduTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalExpectedTransmissionTimeForRelayResistanceRapdu.Tag;
}
