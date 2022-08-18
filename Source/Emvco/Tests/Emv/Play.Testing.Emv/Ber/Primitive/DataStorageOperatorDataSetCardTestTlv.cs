using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageOperatorDataSetCardTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x32, 0x12, 0x08, 0x3c, 0x23, 0x9a, 0x18, 0x20, 0x32, 0x12, 0x08, 0x3c, 0x23, 0x9a, 0x18, 0x20 };

    public DataStorageOperatorDataSetCardTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageOperatorDataSetCardTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageOperatorDataSetCard.Tag;
}
