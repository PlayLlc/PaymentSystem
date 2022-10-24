using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageUnpredictableNumberTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x14, 0x2B, 0x24, 0xC1 };

    public DataStorageUnpredictableNumberTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageUnpredictableNumberTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageUnpredictableNumber.Tag;
}
