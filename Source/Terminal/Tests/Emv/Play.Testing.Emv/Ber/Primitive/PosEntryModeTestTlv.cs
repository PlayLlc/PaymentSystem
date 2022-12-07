using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class PosEntryModeTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 66 };

    public PosEntryModeTestTlv() : base(_DefaultContentOctets) { }

    public PosEntryModeTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => PosEntryMode.Tag;
}
