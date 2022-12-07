using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalFloorLimitTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 14, 136, 24, 78 };

    public TerminalFloorLimitTestTlv() : base(_DefaultContentOctets) { }

    public TerminalFloorLimitTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalFloorLimit.Tag;
}
