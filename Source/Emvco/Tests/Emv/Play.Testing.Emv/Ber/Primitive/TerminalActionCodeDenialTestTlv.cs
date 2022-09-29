using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalActionCodeDenialTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 114, 216, 34, 01, 26 };

    public TerminalActionCodeDenialTestTlv() : base(_DefaultContentOctets) { }

    public TerminalActionCodeDenialTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalActionCodeDenial.Tag;
}
