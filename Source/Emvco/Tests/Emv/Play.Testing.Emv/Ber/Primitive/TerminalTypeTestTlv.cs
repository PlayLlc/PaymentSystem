using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalTypeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 38 };

    public TerminalTypeTestTlv() : base(_DefaultContentOctets) { }

    public TerminalTypeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalType.Tag;
}
