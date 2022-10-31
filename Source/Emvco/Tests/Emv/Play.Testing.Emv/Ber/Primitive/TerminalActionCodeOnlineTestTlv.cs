using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalActionCodeOnlineTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 114, 216, 34, 01, 26 };

    public TerminalActionCodeOnlineTestTlv() : base(_DefaultContentOctets) { }

    public TerminalActionCodeOnlineTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalActionCodeOnline.Tag;
}
