using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class ExtendedSelectionTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 12, 13 };

    public ExtendedSelectionTestTlv() : base(_DefaultContentOctets) { }

    public ExtendedSelectionTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => ExtendedSelection.Tag;
}
