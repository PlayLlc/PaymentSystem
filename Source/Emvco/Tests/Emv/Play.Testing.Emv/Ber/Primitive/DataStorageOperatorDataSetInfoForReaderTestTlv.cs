using Play.Ber.Tags;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class DataStorageOperatorDataSetInfoForReaderTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x3B };

    public DataStorageOperatorDataSetInfoForReaderTestTlv() : base(_DefaultContentOctets) { }

    public DataStorageOperatorDataSetInfoForReaderTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => DataStorageOperatorDataSetInfoForReader.Tag;
}
