using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalCountryCodeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 8, 40 };

    public TerminalCountryCodeTestTlv() : base(_DefaultContentOctets) { }

    public TerminalCountryCodeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalCountryCode.Tag;
}
