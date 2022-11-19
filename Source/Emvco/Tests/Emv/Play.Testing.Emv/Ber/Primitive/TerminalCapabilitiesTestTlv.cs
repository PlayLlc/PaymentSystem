using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalCapabilitiesTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 134, 91 };

    public TerminalCapabilitiesTestTlv() : base(_DefaultContentOctets) { }

    public TerminalCapabilitiesTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalCapabilities.Tag;
}
