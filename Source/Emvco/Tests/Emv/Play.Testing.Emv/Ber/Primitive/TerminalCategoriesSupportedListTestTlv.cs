using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class TerminalCategoriesSupportedListTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets =
        {
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
            0x4d, 0x34, 0x78, 0x4d, 0xef, 0x2a, 0x3c, 0x16, 0xef, 0xaf,
        };

    public TerminalCategoriesSupportedListTestTlv() : base(_DefaultContentOctets) { }

    public TerminalCategoriesSupportedListTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => TerminalCategoriesSupportedList.Tag;
}
