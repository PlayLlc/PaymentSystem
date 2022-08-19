using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageSummary2TestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x13, 0x25, 0xC9, 0x25, 0xE3, 0x18, 0x22, 0x99, 0x10 };

    public DataStorageSummary2TestTlv() : base(_DefaultContentOctets) { }

    public DataStorageSummary2TestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageSummary2.Tag;
}
