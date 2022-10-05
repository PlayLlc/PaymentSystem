using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalActionCodeDefaultTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 114, 216, 34, 01, 26 };

    public TerminalActionCodeDefaultTestTlv() : base(_DefaultContentOctets) { }

    public TerminalActionCodeDefaultTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalActionCodeDefault.Tag;
}
